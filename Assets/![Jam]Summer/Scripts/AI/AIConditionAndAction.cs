using System;
using Unity.VisualScripting;
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
    public TypeCard Card;

    public override bool IsMet(EnemyMaster enemyMaster)
    {
        return enemyMaster.MoneyFromBuild >= Count * enemyMaster.GetCardType(Card).Price;
    }
}
[Serializable]
public class HasMoneyEntityCondition : AICondition
{
    [Header("Has Money Entity")]
    public int Count;
    public TypeCard Card;
    public bool UseBuildMoney;

    public override bool IsMet(EnemyMaster enemyMaster)
    {
        return enemyMaster.GetMoney(UseBuildMoney) >= Count * enemyMaster.GetCardType(Card).Price;
    }
}
[Serializable]
public class CountCardCategoryCondition : AICondition
{
    [Header("Count Card Category")]
    public CategoryCard Card;
    public int CountMin;
    public bool IsEnemy;

    public override bool IsMet(EnemyMaster enemyMaster)
    {
        return GridMaster.Instance.GetCountCategory(Card, IsEnemy ? !enemyMaster.Team : enemyMaster.Team) >= CountMin;
    }
}
[Serializable]
public class CountCardTypeCondition : AICondition
{
    [Header("Count Card Type")]
    public TypeCard Card;
    public int CountMin;
    public bool IsEnemy;

    public override bool IsMet(EnemyMaster enemyMaster)
    {
        return GridMaster.Instance.GetCountType(Card, IsEnemy ? !enemyMaster.Team : enemyMaster.Team) >= CountMin;
    }
}
[Serializable]
public class CountCardCategoryInSquareCondition : AICondition
{
    [Header("Count Card Category In Square")]
    public CategoryCard CategoryCard;
    public int CountMin;
    public bool IsEnemy;
    public Vector2Int point1;
    public Vector2Int point2;

    public override bool IsMet(EnemyMaster enemyMaster)
    {
        return GridMaster.Instance.GetCountCategoryInSquare(CategoryCard, point1, point2, IsEnemy ? !enemyMaster.Team : enemyMaster.Team) >= CountMin;
    }
}
[Serializable]
public class CountCardTypeInSquareCondition : AICondition
{
    [Header("Count Card Type In Square")]
    public TypeCard TypeCard;
    public int CountMin;
    public bool IsEnemy;
    public Vector2Int point1;
    public Vector2Int point2;

    public override bool IsMet(EnemyMaster enemyMaster)
    {
        return GridMaster.Instance.GetCountTypeInSquare(TypeCard, point1, point2, IsEnemy ? !enemyMaster.Team : enemyMaster.Team) >= CountMin;
    }
}

[Serializable]
public class SpawnCardAction : AIAction
{
    public TypeCard CardToSpawn;
    public bool UseBuildMoney;
    public bool UseBuildMoneyIfNecessary;
    public int Count;

    public override bool Execute(EnemyMaster enemyMaster)
    {
        Card _cardToSpawn = enemyMaster.GetCardType(CardToSpawn);
        bool _useBuildMoney = UseBuildMoney;
        bool res = false;
        for (int i = 0; i < Count; i++)
        {
            if (UseBuildMoneyIfNecessary && enemyMaster.GetMoney(false) < _cardToSpawn.Price)
            {
                _useBuildMoney = true;
            }
            res = enemyMaster.SpawnCard(_cardToSpawn, enemyMaster.GetMoney(_useBuildMoney));
            if (res && _useBuildMoney)
            {
                enemyMaster.MoneyFromBuild -= _cardToSpawn.Price;
            }
        }
        return res;

    }
}
