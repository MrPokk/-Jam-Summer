using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridMaster : ObjectGridMono
{
    private IEnumerable<Card> cards;
    public static GridMaster instant { private set; get; }
    public void OnEnable()
    {
        instant = this;
    }

    public IEnumerator Step()
    {
        cards = from p in Grid.GetDictionary().Values select p as Card;
        foreach (var card in cards)
        {
            yield return card.TurnStart();
        }

        foreach (var card in cards)
        {
            yield return card.TurnEnd();
        }
    }
    public bool TryFindNearestEnemy(Vector2Int pos, bool team, out Card enemy, out float minDistance)
    {
        enemy = null;
        var enemies = from p in cards where p.IsPlayer == team select p;
        minDistance = float.MaxValue;
        foreach (var select in enemies)
        {
            float dist = Vector2Int.Distance(select.PosGrid, pos);
            if (dist < minDistance)
            {
                minDistance = dist;
                enemy = select;
            }
        }
        if (enemy) return true;
        return false;
    }
}
