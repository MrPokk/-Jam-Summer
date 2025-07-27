using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "AISetting", fileName = "AISetting")]
public class AISetting : ScriptableObject
{
    [Header("Basic Settings")]
    public float SaveMoneyBuild = 0.5f;
    public CardList CardList;
    public AIDifficulty Difficulty = AIDifficulty.Normal;

    [Header("AI Behaviors")]
    public List<AIBehavior> Behaviors = new List<AIBehavior>();
}

public enum AIDifficulty
{
    Easy,
    Normal,
    Hard
}

[Serializable]
public class CardWeight
{
    [HideInInspector]
    public string Name;
    public Card Card; // Ссылка на карту (для отображения в инспекторе)
    public int Weight; // Вес карты
}
[Serializable]
public class AIKeepProportions
{
    public List<CardWeight> CardsWeight = new List<CardWeight>(); // Теперь List<CardWeight>

    public int GetIndex(CardList cardAll, int[] cardsCount)
    {
        if (CardsWeight == null || CardsWeight.Count == 0 ||
            cardAll == null || cardAll.Entities == null ||
            cardAll.Entities.Count == 0)
        {
            return 0;
        }

        // Рассчитываем общий желаемый вес (только для карт с Weight > 0)
        int totalDesired = 0;
        foreach (var cardWeight in CardsWeight)
        {
            if (cardWeight.Weight > 0)
                totalDesired += cardWeight.Weight;
        }

        if (totalDesired == 0)
            return 0;

        // Считаем общее количество карт (только для разрешённых весов)
        int totalCurrent = 0;
        for (int i = 0; i < cardsCount.Length; i++)
        {
            if (i < CardsWeight.Count && CardsWeight[i].Weight > 0)
                totalCurrent += cardsCount[i];
        }

        // Находим карту с максимальным отклонением от желаемой пропорции
        float maxDeviation = float.MinValue;
        int bestIndex = -1;

        for (int i = 0; i < CardsWeight.Count; i++)
        {
            if (CardsWeight[i].Weight <= 0 ||
                i >= cardAll.Entities.Count ||
                i >= cardsCount.Length)
            {
                continue;
            }

            float desiredRatio = (float)CardsWeight[i].Weight / totalDesired;
            float currentRatio = (totalCurrent > 0) ? (float)cardsCount[i] / totalCurrent : 0f;
            float deviation = desiredRatio - currentRatio;

            if (deviation > maxDeviation || bestIndex == -1)
            {
                maxDeviation = deviation;
                bestIndex = i;
            }
        }

        // Если ничего не найдено, возвращаем первый разрешённый индекс
        if (bestIndex == -1)
        {
            for (int i = 0; i < CardsWeight.Count; i++)
            {
                if (CardsWeight[i].Weight > 0 && i < cardAll.Entities.Count && i < cardsCount.Length)
                {
                    return i;
                }
            }
            return 0;
        }

        return bestIndex;
    }
}
