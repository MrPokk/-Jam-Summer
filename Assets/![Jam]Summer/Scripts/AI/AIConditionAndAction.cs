using System;
using UnityEngine;

[Serializable]
public class HasEnoughMoneyCondition : AICondition
{
    [Header("Has Enough Money")]
    public int MinimumMoney = 10;
    public bool UseBuildMoney;

    public override bool IsMet(EnemyMaster enemyMaster)
    {
        return enemyMaster.GetMoney(UseBuildMoney) >= MinimumMoney;
    }
}
[Serializable]
public class HasMoneyBuildCondition : AICondition
{
    [Header("Has Money Build")]
    public int Count;
    public Card Card;

    public override bool IsMet(EnemyMaster enemyMaster)
    {
        return enemyMaster.MoneyFromBuild >= Count * Card.Price;
    }
}
[Serializable]
public class HasMoneyEntityCondition : AICondition
{
    [Header("Has Money Entity")]
    public int Count;
    public Card Card;
    public bool UseBuildMoney;

    public override bool IsMet(EnemyMaster enemyMaster)
    {
        return enemyMaster.GetMoney(UseBuildMoney) >= Count * Card.Price;
    }
}
[Serializable]
public class CountCardTypeCondition : AICondition
{
    [Header("Count Card Type")]
    public int CountMin;
    public Card Card;
    public bool IsEnemy;

    public override bool IsMet(EnemyMaster enemyMaster)
    {
        return GridMaster.Instance.GetCountType(IsEnemy ? !enemyMaster.Team : enemyMaster.Team, Card) >= CountMin;
    }
}
[Serializable]
public class CountCardInSquareCondition : AICondition
{
    [Header("Count Card In Square")]
    public int CountMin;
    public bool IsEnemy;
    public Vector2Int point1;
    public Vector2Int point2;

    public override bool IsMet(EnemyMaster enemyMaster)
    {
        return GridMaster.Instance.GetCountTypeInSquare<CardEntity>(point1, point2, IsEnemy ? !enemyMaster.Team : enemyMaster.Team) >= CountMin;
    }
}

[Serializable]
public class SpawnCardAction : AIAction
{
    public Card CardToSpawn;
    public bool UseBuildMoney;

    public override bool Execute(EnemyMaster enemyMaster)
    {
        bool res = enemyMaster.SpawnCard(CardToSpawn, enemyMaster.GetMoney(UseBuildMoney));
        if (res && UseBuildMoney)
        {
            enemyMaster.MoneyFromBuild -= CardToSpawn.Price;
        }
        return res;

    }
}
