using BitterCMS.UnityIntegration.Utility;
using UnityEngine;

public class CardCastle : CardBuild
{
    public override void Dead()
    {
        base.Dead();
        CoroutineUtility.StopAll();
        if (IsPlayer)
        {
            Debug.Log("Enemy Win");
        }
        else
        {
            Debug.Log("Player Win");
        }
    }
}
