using System.Collections;
using BitterCMS.UnityIntegration;
using BitterCMS.UnityIntegration.Utility;
using UnityEngine;


public class Root : RootMonoBehavior
{
    public GridMaster Grid;
    public CardList CardList;
    public PlayerMaster Player;
    public EnemyMaster Enemy;

    protected override void GlobalStart()
    {
        Player = GetComponent<PlayerMaster>();
        Enemy = GetComponent<EnemyMaster>();

        Grid.Init();
        Player.Init();
        Enemy.Init();

        CoroutineUtility.Run(Loop());
    }
    private IEnumerator Loop()
    {
        yield return Player;
        yield return Enemy.Step();
        yield return Grid.Step();
        yield return Loop();
    }
}
