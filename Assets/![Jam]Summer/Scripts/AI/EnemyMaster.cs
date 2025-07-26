using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaster : ControlMaster
{
    [SerializeField] protected AISetting Setting;
    protected int[] CountCard;
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
        CountCard = new int[Setting.CardList.Entities.Count];
    }

    public override IEnumerator Step()
    {
        MoneyFromBuild += IncomeStep * Setting.SaveMoneyBuild;

        // Обновляем счетчики карт
        for (int i = 0; i < Setting.CardList.Entities.Count; i++)
        {
            CountCard[i] = GridMaster.Instance.GetCountType(Team, Setting.CardList.Entities[i]);
        }

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
}
