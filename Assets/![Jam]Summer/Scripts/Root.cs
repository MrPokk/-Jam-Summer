using System.Collections;
using BitterCMS.UnityIntegration;
using BitterCMS.UnityIntegration.Utility;
using UnityEngine;

public class Root : RootMonoBehavior
{
    public Card Bow;
    public Card Sword;
    public int Step;
    public int CountBow;
    public int CountSword;
    public GridMaster GridMaster;


    protected override void GlobalStart()
    {

        GridMaster.Create();
        for (int i = 0; i < CountBow; i++)
        {
            Card entity = Instantiate(Bow);
            entity.Init();
            GridMaster.AddRandomPos(entity, out var pos);
            entity.SetPos(pos);
            entity.SetTeam(i % 2 == 0);
        }
        for (int i = 0; i < CountSword; i++)
        {
            Card entity = Instantiate(Sword);
            entity.Init();
            GridMaster.AddRandomPos(entity, out var pos);
            entity.SetPos(pos);
            entity.SetTeam(i % 2 == 0);
        }
        CoroutineUtility.Run(Loop());
    }
    private IEnumerator Loop()
    {
        yield return GridMaster.Step();
        Step--;
        if (Step > 0) yield return Loop();
    }
}
