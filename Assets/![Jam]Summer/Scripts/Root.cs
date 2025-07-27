using System.Collections;
using System.Collections.Generic;
using BitterCMS.UnityIntegration;
using BitterCMS.Utility.Interfaces;
using System.Linq;
using UnityEngine;
using System;
using static UnityEngine.EventSystems.EventTrigger;

public class Root : RootMonoBehavior
{
    [Header("UI References")]
    [field: SerializeField]
    public UIRoot UIRoot { get; private set; }

    [Header("Game Systems")]
    public GridMaster Grid;
    public CardList CardList;

    [field: SerializeField] public List<AISetting> AISettingsSetups { get; private set; } = new();
    private int _roundCurrent = 0;


    [Header("Player and Enemy")]
    public PlayerMaster Player;
    public EnemyMaster Enemy;

    protected override void GlobalStart()
    {
        var fistRound = AISettingsSetups.FirstOrDefault();

        if (!AISettingsSetups.Any())
            throw new Exception("AISettingsSetups is empty");

        Player = GetComponent<PlayerMaster>();
        Enemy = GetComponent<EnemyMaster>();

        Grid.Init();
        Player.Init();
        Enemy.Init(fistRound);

        UIRoot.InitializeUI();
        CoroutineUtility.Run(PreLoadRound());
    }

    private IEnumerator PreLoadRound()
    {
        yield return LoadAnimationRunRound(3f);
        UIRoot.ToggleCanvas();

        CoroutineUtility.Run(Loop());
        yield break;
    }

    private Coroutine LoadAnimationRunRound(float duration)
    {
        UIRoot.HudRoot.RadialDissolveController.gameObject.SetActive(true);
        return UIRoot.HudRoot.RadialDissolveController.DissolveAnimation(duration, DissolveType.Decrease);
    }

    private Coroutine LoadAnimationEndRound(float duration)
    {
        UIRoot.HudRoot.RadialDissolveController.gameObject.SetActive(true);
        return UIRoot.HudRoot.RadialDissolveController.DissolveAnimation(duration, DissolveType.Increase);
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

    //___Относительно_игрока___
    public IEnumerator Win()
    {
        yield return LoadAnimationEndRound(3f);
        UIRoot.ToggleCanvas();

        _roundCurrent++;
        if (_roundCurrent == AISettingsSetups.Count)
        {
            yield return EndGame();
            yield break;
        }
        LoadBattleEnemy(_roundCurrent);
        yield break;
    }

    public void LoadLastEnemy()
    {
        _roundCurrent--;
        if (_roundCurrent < 0)
        {
            _roundCurrent = 0;
        }
        CoroutineUtility.Run(SwapEnemy(_roundCurrent));
        return;
    }
    public void LoadNextEnemy()
    {
        _roundCurrent++;
        if (_roundCurrent >= AISettingsSetups.Count)
        {
            _roundCurrent = AISettingsSetups.Count - 1;
        }
        CoroutineUtility.Run(SwapEnemy(_roundCurrent));
        return;
    }
    public IEnumerator SwapEnemy(int index)
    {
        yield return LoadAnimationEndRound(3f);
        UIRoot.ToggleCanvas();
        LoadBattleEnemy(index);
        yield break;
    }

    public void RestartBattle() => LoadBattleEnemy(_roundCurrent);
    private void LoadBattleEnemy(int index)
    {
        _roundCurrent = index;
        var currentSetupRound = AISettingsSetups[_roundCurrent];
        Grid.Clear();
        Player.Init();
        Enemy.Init(currentSetupRound);

        Player.Money = 0;
        Enemy.Money = 0;
        CoroutineUtility.Run(PreLoadRound());
    }

    public IEnumerator Lose()
    {
        UIRoot.ToggleManagementPanel();
        yield return LoadAnimationEndRound(4f);
        UIRoot.ShowLoseCanvas();

        yield break;
    }

    public IEnumerator EndGame()
    {
        yield break;
    }
}
