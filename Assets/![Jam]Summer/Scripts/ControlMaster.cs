﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ControlMaster : MonoBehaviour
{
    public Card CardCastle;
    public int Money;
    public bool Team;

    public Vector2Int PosCastle;
    public int LineFront;
    public int LineBack;
    public int LineMaxBuild;

    protected int IncomeStep;
    protected int MaxY => GridMaster.Instance.Size.y;

    public virtual void Init()
    {
        if (!SpawnCardToGrid(CardCastle, PosCastle)) throw new Exception("Castle has not been created");
    }
    public virtual IEnumerator Step()
    {
        yield break;
    }
    public virtual void GiveMoney(int count)
    {
        Money += count;
        IncomeStep += count;
    }
    public virtual bool SpawnCard(Card card) => SpawnCard(card, Money);
    public virtual bool SpawnCard(Card card, int money)
    {
        if (card == null) throw new NullReferenceException("Card");
        if (card.Price <= money && card.Price <= Money)
        {
            if (card is CardEntity entity)
            {
                int selectLine = LineBack;
                if (entity.IsFront) selectLine = LineFront;
                bool res = SpawnCardToGridLine(entity, selectLine);
                if (res) Money -= entity.Price;
                return res;
            }
            if (card is CardBuild build)
            {
                int posX = PosCastle.x;
                int step;
                for (step = 0; GridMaster.Instance.GetCountTypeInSquare<CardBuild>(new(posX, 0), new(posX, GridMaster.Instance.Size.y), Team) > 1 && step < LineMaxBuild; step++)
                {
                    posX += Team ? 1 : -1;
                }
                if (step == LineMaxBuild) posX = PosCastle.x;
                bool res = SpawnCardToGridLine(build, posX);
                if (res) Money -= build.Price;
                return res;
            }
        }
        return false;
    }
    public bool SpawnCardToGrid(Card card, Vector2Int pos)
    {
        if (!GridMaster.Instance.TryGetAtPos(pos, out var _null))
        {
            Card entity = Instantiate(card);
            entity.Init();
            GridMaster.Instance.Add(entity, pos);
            entity.SetPos(pos);
            entity.SetTeam(Team);
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
            if (!GridMaster.Instance.TryGetAtPos(pos, out var _null))
            {
                empty.Add(pos);
            }
        }
        if (empty.Count == 0) return false;
        return SpawnCardToGrid(card, empty[Random.Range(0, empty.Count)]);
    }
}
