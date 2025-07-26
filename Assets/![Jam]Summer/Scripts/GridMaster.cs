using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class GridMaster : ObjectGridMono
{
    private IReadOnlyCollection<Card> _cards;
    public static GridMaster Instance { get; private set; }
    
    public override void Init()
    {
        base.Init();
        Instance = this;
    }

    public IEnumerator Step()
    {
        // Оптимизированный запрос LINQ с кэшированием результатов
        _cards = Grid.GetDictionary().Values
            .OfType<Card>()
            .OrderByDescending(c => c.Priority)
            .ThenBy(c => c.IsPlayer) // Сортировка по IsPlayer в возрастающем порядке (false будет перед true)
            .ToList()
            .AsReadOnly();

        foreach (var card in _cards)
        {
            yield return card.TurnStart();
        }

        foreach (var card in _cards)
        {
            yield return card.TurnEnd();
        }
    }

    protected override void SetPosObj(MonoBehaviour value, Vector2Int pos, bool res)
    {
        if (res && AutoPos)
        {
            Vector3 movePos = GridToWorldCentre(pos);
            movePos.z = -pos.y;
            
            float distance = Vector2.Distance(value.transform.position, movePos);
            if (distance < 3)
            {
                value.transform.DOLocalJump(movePos, 0.5f, 1, CardEntity.TimeMove)
                    .SetEase(Ease.Linear)
                    .Play();
            }
            else
            {
                value.transform.position = movePos;
            }
        }
    }

    public int GetCountType<TCard>() where TCard : Card
    {
        return Grid.GetDictionary().Values
            .OfType<Card>()
            .Count(c => c is TCard);
    }

    public int GetCountType(bool team, Card card)
    {
        return Grid.GetDictionary().Values
            .OfType<Card>()
            .Count(c => c.name.StartsWith(card.name) && c.IsPlayer == team);
    }

    public bool TryFindNearestEntity(Vector2Int pos, bool team, out Card enemy, out float minDistance)
    {
        enemy = null;
        minDistance = float.MaxValue;
        
        var enemies = _cards?.Where(c => c.IsPlayer == team);
        if (enemies == null || !enemies.Any())
            return false;

        foreach (var candidate in enemies)
        {
            float dist = Vector2Int.Distance(candidate.PosGrid, pos);
            if (dist < minDistance)
            {
                minDistance = dist;
                enemy = candidate;
            }
        }
        
        return enemy != null;
    }

    public int GetCountTypeInSquare<TCard>(Vector2Int point1, Vector2Int point2, bool team) where TCard : Card
    {
        int minX = Mathf.Min(point1.x, point2.x);
        int maxX = Mathf.Max(point1.x, point2.x);
        int minY = Mathf.Min(point1.y, point2.y);
        int maxY = Mathf.Max(point1.y, point2.y);

        return Grid.GetDictionary().Values
            .OfType<Card>()
            .Count(c => c is TCard && 
                   c.IsPlayer == team && 
                   c.PosGrid.x >= minX && 
                   c.PosGrid.x <= maxX && 
                   c.PosGrid.y >= minY && 
                   c.PosGrid.y <= maxY);
    }
}
