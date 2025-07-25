using System.Collections;
using BitterCMS.UnityIntegration;
using BitterCMS.UnityIntegration.Utility;

public class Root : RootMonoBehavior
{
    public Card Bow;
    public Card Sword;
    public int Step;
    public int CountBow;
    public int CountSword;
    protected override void GlobalStart()
    {
        for (int i = 0; i < CountBow; i++)
        {
            Card entity = Instantiate(Bow);
            entity.Init();
            GridMaster.instant.AddRandomPos(entity, out var pos);
            entity.SetPos(pos);
            entity.SetTeam(i % 2 == 0);
        }
        for (int i = 0; i < CountSword; i++)
        {
            Card entity = Instantiate(Sword);
            entity.Init();
            GridMaster.instant.AddRandomPos(entity, out var pos);
            entity.SetPos(pos);
            entity.SetTeam(i % 2 == 0);
        }
        CoroutineUtility.Run(Loop());
    }
    private IEnumerator Loop()
    {
        yield return GridMaster.instant.Step();
        Step--;
        if (Step > 0) yield return Loop();
    }
}
