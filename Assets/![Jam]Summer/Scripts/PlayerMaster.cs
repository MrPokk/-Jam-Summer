using System;
using System.Collections;

public class PlayerMaster : ControlMaster
{
    public event Action<int> MoneyChangeUI;

    public override IEnumerator Step()
    {
        base.Step();
        MoneyChangeUI?.Invoke(Money);
        yield break;
    }
    public bool SpawnBowman()
    {
        bool res = SpawnCard(Cards.Bow);
        if (res)
            MoneyChangeUI?.Invoke(Money);
        return res;
    }

    public bool SpawnSwordsman()
    {
        bool res = SpawnCard(Cards.Sword);
        if (res)
            MoneyChangeUI?.Invoke(Money);
        return res;
    }

    public bool SpawnBuild()
    {
        bool res = SpawnCard(Cards.Build);
        if (res)
            MoneyChangeUI?.Invoke(Money);
        return res;
    }
}
