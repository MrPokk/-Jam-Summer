using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using System.Linq;

public class EnemyMaster : ControlMaster
{
    [SerializeField]
    private AISetting _setting;

    public float MoneyFromBuild { get; set; }
    public AISetting Setting => _setting;

    public int GetMoney(bool useBuild)
    {
        return useBuild ? Money : Mathf.FloorToInt(Money - MoneyFromBuild);
    }

    public void Init(AISetting setting)
    {
        if (setting == null)
        {
            Debug.LogError($"{name}: {nameof(AISetting)} не может быть null");
            return;
        }


        if (_setting != null)
        {
            Debug.LogError($"{name}: Настройки ИИ уже установлены");
        }
        else
        {
            _setting = setting;
        }

        Init();
    }

    public override void Init()
    {
        base.Init();
        ResetBuildMoney();
    }

    public void ResetBuildMoney()
    {
        MoneyFromBuild = 0f;
    }

    public override IEnumerator Step()
    {
        if (_setting == null)
        {
            Debug.LogError($"{name}: Настройки ИИ не установлены");
            yield break;
        }

        AddBuildIncome();
        ExecuteBehaviors();

        IncomeStep = 0;
        yield break;
    }

    private void AddBuildIncome()
    {
        MoneyFromBuild += IncomeStep * _setting.SaveMoneyBuild;
    }

    private void ExecuteBehaviors()
    {
        if (_setting.Behaviors == null || _setting.Behaviors.Count == 0)
        {
            Debug.LogWarning($"{name}: Нет поведений для выполнения");
            return;
        }

        foreach (var behavior in _setting.Behaviors.Where(b => b != null))
        {
            try
            {
                if (behavior.ShouldExecute(this) && behavior.Execute(this) && behavior.ExitWhenExecuting)
                {
                    break;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{name}: Ошибка выполнения поведения {behavior.GetType().Name}: {ex.Message}");
            }
        }
    }
    public Card GetCardType(TypeCard type)
    {
        var card = (Card)Setting.CardList.Entities.Find(x => x.Type == type)
        ?? Setting.CardList.Builds.Find(x => x.Type == type);
        return card;
    }

}
