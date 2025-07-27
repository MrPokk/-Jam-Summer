using System;
using System.Collections;

public class PlayerMaster : ControlMaster
{
    public event Action<int> MoneyChangeUI;
    public CardList Cards;

    public override IEnumerator Step()
    {
        base.Step();
        MoneyChangeUI?.Invoke(Money);
        yield break;
    }
    public bool SpawnEntity(TypeCard card)
    {
        bool res = SpawnCard(Cards.Entities.Find(x => x.Type == card));
        if (res)
            MoneyChangeUI?.Invoke(Money);
        return res;
    }

    public bool SpawnBuild(TypeCard card)
    {
        bool res = SpawnCard(Cards.Builds.Find(x => x.Type == card));
        if (res)
            MoneyChangeUI?.Invoke(Money);
        return res;
    }
}
