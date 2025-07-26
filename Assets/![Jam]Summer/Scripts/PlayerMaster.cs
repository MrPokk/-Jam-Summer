using System;
using System.Collections;

public class PlayerMaster : ControlMaster
{
    public event Action<int> MoneyChangeUI;

    public IEnumerator Step()
    {
        yield break;
    }
    public bool SpawnBowman()
    {
        MoneyChangeUI.Invoke(Money);
        return SpawnCard(Cards.Bow);
    }

    public bool SpawnSwordsman()
    {
        MoneyChangeUI.Invoke(Money);
        return SpawnCard(Cards.Sword);
    }

    public bool SpawnBuild()
    {
        MoneyChangeUI.Invoke(Money);
        return SpawnCard(Cards.Build);
    }
}
