using System.Collections;
using BitterCMS.UnityIntegration;

public class CardBuild : Card
{
    public int Income;
    public int HealingHp;
    public override CategoryCard Category => CategoryCard.Build;
    public override IEnumerator TurnEnd()
    {
        if (IsPlayer) GlobalState.GetRoot<Root>().Player.GiveMoney(Income);
        else GlobalState.GetRoot<Root>().Enemy.GiveMoney(Income);
        yield break;
    }

    public override IEnumerator TurnStart()
    {
        Healing(HealingHp);
        yield break;
    }
}
