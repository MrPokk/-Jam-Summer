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
        CoroutineUtility.Run(LoadGame());
    }
    private IEnumerator LoadGame()
    {
        Player = GetComponent<PlayerMaster>();
        Enemy = GetComponent<EnemyMaster>();

        Grid.Init();
        Player.Init();
        Enemy.Init();

        _uIRoot.InitializeUI();

        yield return LoadAnimation(4f);
        _uIRoot.ToggleCanvas();

        CoroutineUtility.Run(Loop());
        yield break;
    }

    private Coroutine LoadAnimation(float duration = -1f)
    {
        _uIRoot.HudRoot.RadialDissolveController.gameObject.SetActive(true);
        return _uIRoot.HudRoot.RadialDissolveController.StartDissolveAnimation(duration);
    }


    private IEnumerator Loop()
    {
        while (Grid.GetCountType<CardCastle>(false) + Grid.GetCountType<CardCastle>(true) == 2)
        {
            yield return Player.Step();
            yield return Enemy.Step();
            yield return Grid.Step();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
