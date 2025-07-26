using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class EnemyMaster : ControlMaster
{
    [SerializeField]
    protected AISetting Setting;
    [SerializeField]
    protected int[] CountCard;
    public float MoneyFromBuild;
    public float BowOnSword;
    public override void Init()
    {
        base.Init();
        MoneyFromBuild = 0;
        CountCard = new int[Setting.CardList.Entities.Count];
    }
    public override IEnumerator Step()
    {
        MoneyFromBuild += IncomeStep * Setting.SaveMoneyBuild;
        for (int i =  0; i < Setting.CardList.Entities.Count; i++)
        {
            CountCard[i] = GridMaster.instant.GetCountType(Team, Setting.CardList.Entities[i]);
        }
        int select = Setting.KeepProportions.GetIndex(Setting.CardList, CountCard);
        SpawnCard(Setting.CardList.Entities[select], (int)(Money - MoneyFromBuild));
        /*
        CountBow = GridMaster.instant.GetCountType<CardBowman>(Team);
        CountSword = GridMaster.instant.GetCountType<CardSwordsman>(Team);
        while (MoneyFromBuild > Cards.Build.Price)
        {
            if (SpawnCard(Cards.Build))
            {
                yield return new WaitForSeconds(0.3f);
            }
            else
            {
                MoneyFromBuild = Cards.Build.Price;
                break;
            }
        }

        {
            bool res;
            do
            {
                int money = (int)(Money - MoneyFromBuild);
                if (SpawnPrioritySword()) res = SpawnCard(Cards.Sword, money);
                else res = SpawnCard(Cards.Bow, money);
                yield return new WaitForSeconds(0.3f);
            }
            while (res);
        }
        */
        IncomeStep = 0;
        yield break;
    }
    public override bool SpawnCard(Card card, int money)
    {
        bool res = base.SpawnCard(card, money);
        if (res)
        {
            if (card is CardBuild) MoneyFromBuild -= card.Price;
            if (card is CardEntity);
            else throw new System.Exception("Not standard card");
        }
        return res;
    }
}
