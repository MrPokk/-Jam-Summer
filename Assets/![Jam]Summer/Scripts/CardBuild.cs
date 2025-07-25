using System.Collections;

public class CardBuild : Card
{
    public int Income;
    public int HealingHp;
    public override IEnumerator TurnEnd()
    {
        if (IsPlayer) Root.Player.GiveMoney(Income);
        else Root.Enemy.GiveMoney(Income);
        yield break;
    }

    public override IEnumerator TurnStart()
    {
        Healing(HealingHp);
        yield break;
    }
}
