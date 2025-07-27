using BitterCMS.UnityIntegration;
using UnityEngine;

public class CardCastle : CardBuild
{
    public override void Dead()
    {
        base.Dead();
        if (IsPlayer)
            CoroutineUtility.Run(GlobalState.GetRoot<Root>().HandleRoundEnd(false));
        else
            CoroutineUtility.Run(GlobalState.GetRoot<Root>().HandleRoundEnd(true));
    }
}
