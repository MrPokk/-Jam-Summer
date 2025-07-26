using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AICondition
{
    public abstract bool IsMet(EnemyMaster enemyMaster);
    public bool Inversion = false;
}

[Serializable]
public abstract class AIAction
{
    public abstract bool Execute(EnemyMaster enemyMaster);
}

[Serializable]
public class AIBehavior
{
    [SerializeReference] public List<AICondition> Conditions = new List<AICondition>();
    [SerializeReference] public List<AIAction> Actions = new List<AIAction>();
    public bool ExitWhenExecuting = false;

    public AIBehavior(){}
    public bool ShouldExecute(EnemyMaster enemyMaster)
    {
        if (Conditions == null || Conditions.Count == 0)
            return true;

        foreach (var condition in Conditions)
        {
            if (condition.Inversion ? condition.IsMet(enemyMaster) : !condition.IsMet(enemyMaster))
                return false;
        }
        return true;
    }

    public bool Execute(EnemyMaster enemyMaster)
    {
        if (Actions == null) return false;
        bool res = false;
        foreach (var action in Actions)
        {
            if (action.Execute(enemyMaster))
                res = true;
        }
        return res;
    }
}
