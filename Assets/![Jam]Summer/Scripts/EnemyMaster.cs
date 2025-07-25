using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyMaster : ControlMaster
{
    protected int IncomeStep;
    [SerializeField]
    protected float MoneyFromBuild;
    public float SaveMoneyBuild;
    public float BowOnSword;

    public int CountBow;
    public int CountSword;
    public override void Init()
    {
        base.Init();
        IncomeStep = 0;
        MoneyFromBuild = 0;
    }
    public override void GiveMoney(int count)
    {
        base.GiveMoney(count);
        IncomeStep += count;
    } 
    public IEnumerator Step()
    {
        MoneyFromBuild += IncomeStep * SaveMoneyBuild;
        CountBow = GridMaster.instant.GetCountType(Cards.Bow, Team);
        CountSword = GridMaster.instant.GetCountType(Cards.Sword, Team);
        while (MoneyFromBuild > Cards.Build.Price)
        {
            if (SpawnCardToGridLine(Cards.Build, PosCastle.x))
            {
                MoneyFromBuild -= Cards.Build.Price;
                Money -= Cards.Build.Price;
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
                if (SpawnPrioritySword()) res = SpawnBow();
                else res = SpawnSword();
                yield return new WaitForSeconds(0.3f);
            }
            while (res);
        }
        IncomeStep = 0;
        yield break;
    }
    protected bool SpawnSword()
    {
        bool res = Money - MoneyFromBuild >= Cards.Sword.Price && SpawnCardToGridLine(Cards.Sword, LineFront);
        if (res)
        {
            CountSword++;
            Money -= Cards.Sword.Price;
        }
        return res;
    }
    protected bool SpawnBow()
    {
        bool res = Money - MoneyFromBuild >= Cards.Bow.Price && SpawnCardToGridLine(Cards.Bow, LineBack);
        if (res)
        {
            CountBow++;
            Money -= Cards.Bow.Price;
        }
        return res;
    }
    protected bool SpawnPrioritySword() => (float)CountBow / (CountSword + 0.00001f) < BowOnSword;
}
