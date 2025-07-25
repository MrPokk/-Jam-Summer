using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ControlMaster : MonoBehaviour
{
    public CardList Cards;
    public int Money;
    public bool Team;

    public Vector2Int PosCastle;
    public int LineFront;
    public int LineBack;

    protected int MaxY => GridMaster.instant.Size.y;
    public virtual void Init()
    {
        if (!SpawnCardToGrid(Cards.Castle, PosCastle, Team)) throw new Exception("Castle has not been created");
    }
    public virtual void GiveMoney(int count)
    {
        Money += count;
    }
    public bool SpawnCardToGrid(Card card, Vector2Int pos, bool team)
    {
        if (!GridMaster.instant.TryGetAtPos(pos, out var _null))
        {
            Card entity = Instantiate(card);
            entity.Init();
            GridMaster.instant.Add(entity, pos);
            entity.SetPos(pos);
            entity.SetTeam(team);
            return true;
        }

        return false;
    }
    public bool SpawnCardToGridLine(Card card, int line)
    {
        List<Vector2Int> empty = new();
        Vector2Int pos = new Vector2Int(line, 0);
        for (pos.y = 0; pos.y < MaxY; pos.y++)
        {
            if (!GridMaster.instant.TryGetAtPos(pos, out var _null))
            {
                empty.Add(pos);
            }
        }
        if (empty.Count == 0) return false;
        return SpawnCardToGrid(card, empty[Random.Range(0, empty.Count)], Team);
    }
}
