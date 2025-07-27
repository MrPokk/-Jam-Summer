using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaster : ControlMaster
{
    [SerializeField] protected AISetting Setting;
    public float MoneyFromBuild;
    public float BowOnSword;
    public int GetMoney(bool UseBuild)
    {
        return UseBuild ? Money : (int)(Money - MoneyFromBuild);
    }

    public override void Init()
    {
        base.Init();
        MoneyFromBuild = 0;
    }

    public override IEnumerator Step()
    {
        MoneyFromBuild += IncomeStep * Setting.SaveMoneyBuild;

        // Проверяем все поведения
        foreach (var behavior in Setting.Behaviors)
        {
            if (behavior.ShouldExecute(this))
            {
                if (behavior.Execute(this) && behavior.ExitWhenExecuting)
                    break;
            }
        }
        IncomeStep = 0;
        yield break;
    }
    public Card GetCardType(TypeCard type)
    {
        Card card = Setting.CardList.Entities.Find(x => x.Type == type);
        if (card == null) card = Setting.CardList.Builds.Find(x => x.Type == type);
        return card;
    }
}
