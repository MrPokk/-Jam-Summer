using System.Collections;

public class PlayerMaster : ControlMaster
{
    public IEnumerator Step()
    {
        yield break;
    }
    public bool SpawnBowman() => SpawnCard(Cards.Bow);
    public bool SpawnSwordsman() => SpawnCard(Cards.Sword);
    public bool SpawnBuild() => SpawnCard(Cards.Build);
}
