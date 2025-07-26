using System;
using System.Collections;
using BitterCMS.UnityIntegration;
using BitterCMS.UnityIntegration.Utility;
using UnityEngine;

public class Root : RootMonoBehavior
{
    [SerializeField]
    private UIRoot _uIRoot;

    public GridMaster Grid;
    public CardList CardList;
    public PlayerMaster Player;
    public EnemyMaster Enemy;

    protected override void GlobalStart()
    {
        _uIRoot.CanvasRoot.gameObject.SetActive(false);

        CoroutineUtility.Run(LoadGame());
    }
    private IEnumerator LoadGame()
    {
        Player = GetComponent<PlayerMaster>();
        Enemy = GetComponent<EnemyMaster>();

        Grid.Init();
        Player.Init();
        Enemy.Init();

        yield return _uIRoot.HudRoot.RadialDissolveController.StartDissolveAnimation(4f);
        _uIRoot.CanvasRoot.gameObject.SetActive(true);

        CoroutineUtility.Run(Loop());
        yield break;
    }

    private IEnumerator Loop()
    {
        while (Grid.GetCountType<CardCastle>() == 2)
        {
            yield return Player.Step();
            yield return Enemy.Step();
            yield return Grid.Step();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
