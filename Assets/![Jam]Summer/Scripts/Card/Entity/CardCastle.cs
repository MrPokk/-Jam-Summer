using UnityEngine;

public class CardCastle : CardBuild
{
    public override void Dead()
    {
        base.Dead();
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
