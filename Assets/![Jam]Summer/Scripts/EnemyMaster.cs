using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class EnemyMaster : ControlMaster
{
    
    [SerializeField]
    protected float MoneyFromBuild;
    public float SaveMoneyBuild;
    public float BowOnSword;

    public int CountBow;
    public int CountSword;
    public override void Init()
    {
        base.Init();
        MoneyFromBuild = 0;
    }
    public override IEnumerator Step()
    {
        MoneyFromBuild += IncomeStep * SaveMoneyBuild;
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
        IncomeStep = 0;
        yield break;
    }
    public override bool SpawnCard(Card card, int money)
    {
        bool res = base.SpawnCard(card, money);
        if (res)
        {
            if (card is CardSwordsman) CountSword++;
            else if (card is CardBowman) CountBow++;
            else if (card is CardBuild) MoneyFromBuild -= card.Price;
            else throw new System.Exception("Not standard card");
        }
        return res;
    }
    protected bool SpawnPrioritySword() => CountSword / (CountSword + CountBow + 1.0f) > BowOnSword;
}
