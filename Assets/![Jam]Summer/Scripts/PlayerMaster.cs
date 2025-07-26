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
    public bool SpawnEntity(int index)
    {
        bool res = SpawnCard(Cards.Entities[index]);
        if (res)
            MoneyChangeUI?.Invoke(Money);
        return res;
    }

    public bool SpawnBuild(int index)
    {
        bool res = SpawnCard(Cards.Builds[index]);
        if (res)
            MoneyChangeUI?.Invoke(Money);
        return res;
    }
}
