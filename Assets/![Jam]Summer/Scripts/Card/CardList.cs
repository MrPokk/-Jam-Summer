using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardList", fileName = "CardList")]
public class CardList : ScriptableObject
{
    public List<CardBuild> Builds;
    public List<CardEntity> Entities;

    public IReadOnlyCollection<Card> GetAll()
    {
        var all = new List<Card>(Builds.Count + Entities.Count);
        all.AddRange(Entities);
        all.AddRange(Builds);
        return all;
    }
}
