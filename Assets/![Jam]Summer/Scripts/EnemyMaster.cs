using System.Collections;
using UnityEngine;

public class EnemyMaster : ControlMaster
{
    protected int IncomeStep;
    [SerializeField]
    protected float MoneyFromBuild;
    public float SaveMoneyBuild;
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
        while (Money - MoneyFromBuild >= Cards.Sword.Price && SpawnCardToGridLine(Cards.Sword, LineFront))
        {
            Money -= Cards.Sword.Price;
            yield return new WaitForSeconds(0.3f);
        }
        IncomeStep = 0;
        yield break;
    }
}
