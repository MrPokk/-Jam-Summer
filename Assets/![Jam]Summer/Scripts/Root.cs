using System;
using System.Collections;
using BitterCMS.UnityIntegration;
using BitterCMS.UnityIntegration.Utility;
using UnityEngine;

public class Root : RootMonoBehavior
{
    [field: SerializeField]
    public UIRoot UIRoot { get; private set; }

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

        UIRoot.InitializeUI();

        yield return LoadAnimation(4f);
        UIRoot.ToggleCanvas();

        CoroutineUtility.Run(Loop());
        yield break;
    }

    private Coroutine LoadAnimation(float duration = -1f)
    {
        UIRoot.HudRoot.RadialDissolveController.gameObject.SetActive(true);
        return UIRoot.HudRoot.RadialDissolveController.StartDissolveAnimation(duration);
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
