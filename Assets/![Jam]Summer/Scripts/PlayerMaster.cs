using System;

public class PlayerMaster : ControlMaster
{
    public event Action<int> MoneyChanged;

    public void SpawnSword()
    {
        var delta = Money - Cards.Sword.Price;
        if (delta <= 0)
        {
            //TODO animation
            return;
        }

        SpawnCardToGridLine(Cards.Sword, LineFront);
        Money = delta;
        MoneyChanged?.Invoke(Money);
    }

    public void SpawnBow()
    {
        var delta = Money - Cards.Sword.Price;
        if (delta <= 0)
        {
            //TODO animation
            return;
        }
        SpawnCardToGridLine(Cards.Bow, LineBack);
        Money = delta;
        MoneyChanged?.Invoke(Money);
    }
}
