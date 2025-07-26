using System.Collections;
using BitterCMS.UnityIntegration;

public class CardBuild : Card
{


    public int Income;
    public int HealingHp;
    public override IEnumerator TurnEnd()
    {
        GlobalState.GetRoot<Root>().Enemy.GiveMoney(Income);
        yield break;
    }

    public override IEnumerator TurnStart()
    {
        Healing(HealingHp);
        yield break;
    }
}
