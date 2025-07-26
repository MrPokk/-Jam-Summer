using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class GridMaster : ObjectGridMono
{
    private IEnumerable<Card> cards;
    public static GridMaster instant { private set; get; }
    public override void Init()
    {
        base.Init();
        instant = this;
    }

    public IEnumerator Step()
    {
        cards = from s in
                    from p in Grid.GetDictionary().Values
                    select p as Card 
                orderby s.Priority descending, s.IsPlayer  
                select s;
        foreach (var card in cards)
        {
            yield return card.TurnStart();
        }

        foreach (var card in cards)
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
            if (Vector2.Distance(value.transform.position, movePos) < 3)
                value.transform.DOLocalJump(movePos, 0.5f, 1, CardEntity.TimeMove).SetEase(Ease.Linear).Play();
            else
                value.transform.position = movePos;
        }
    }
    public int GetCountType<TCard>() where TCard : Card
    {
        var select = from s in
                         from p in Grid.GetDictionary().Values
                         select p as Card
                     where s is TCard
                     select s;
        return select.Count();
    }
    public int GetCountType(bool team, Card card)
    {
        var select = from s in
                         from p in Grid.GetDictionary().Values
                         select p as Card
                     where s.name.StartsWith(card.name) && s.IsPlayer == team
                     select s;
        return select.Count();
    }
    public bool TryFindNearestEntity(Vector2Int pos, bool team, out Card enemy, out float minDistance)
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
    public int GetCountTypeInSquare<TCard>(Vector2Int point1, Vector2Int point2, bool team) where TCard : Card
    {
        var select = from card in
                         from s in Grid.GetDictionary().Values
                         select s as Card
                     where card is TCard && card.IsPlayer == team && card.PosGrid.x >= point1.x && card.PosGrid.y >= point1.y && card.PosGrid.x <= point2.x && card.PosGrid.y <= point2.y
                     select card;
        return select.Count();
    }
}
