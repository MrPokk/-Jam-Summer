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
    public int RoundCurrent { get; private set; } = 0 ;

    [Header("Player and Enemy")]
    public PlayerMaster Player;
    public EnemyMaster Enemy;

    protected override void GlobalStart()
    {
        if (!AISettingsSetups.Any())
            throw new Exception("AISettingsSetups is empty");

        Player = GetComponent<PlayerMaster>();
        Enemy = GetComponent<EnemyMaster>();

        Grid.Init();
        Player.Init();
        Enemy.Init(AISettingsSetups.FirstOrDefault());

        UIRoot.InitializeUI();
        StartCoroutine(StartRound());
    }

    private IEnumerator StartRound()
    {
        UIRoot.ForcedLoseCanvas(false);
        yield return UIRoot.HudRoot.RadialDissolveController.DissolveAnimation(3f, DissolveType.Decrease);
        UIRoot.ResetUI();
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (Grid.GetCountType<CardCastle>() == 2)
        {
            yield return Player.Step();
            yield return Enemy.Step();
            yield return Grid.Step();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void RestartBattle()
    {
        LoadBattle(RoundCurrent);
    }

    public void ChangeEnemy(bool next)
    {
        RoundCurrent = Mathf.Clamp(RoundCurrent + (next ? 1 : -1), 0, AISettingsSetups.Count - 1);
        StartCoroutine(SwapEnemy());
    }

    private void LoadBattle(int index)
    {
        RoundCurrent = index;
        Grid.Clear();
        Player.Init();
        Enemy.Init(AISettingsSetups[RoundCurrent]);

        Player.Money = 0;
        Enemy.Money = 0;
        StartCoroutine(StartRound());
    }

    public IEnumerator HandleRoundEnd(bool isWin)
    {

        if (isWin)
        {
            UIRoot.ForcedCanvas(false);

            yield return UIRoot.HudRoot.RadialDissolveController.DissolveAnimation(isWin ? 3f : 4f, DissolveType.Increase);

            if (++RoundCurrent >= AISettingsSetups.Count)
            {
                yield return EndGame();
                yield break;
            }
            LoadBattle(RoundCurrent);
        }
        else
        {
            UIRoot.ForcedManagementPanel(false);
            yield return UIRoot.HudRoot.RadialDissolveController.DissolveAnimation(isWin ? 3f : 4f, DissolveType.Increase);
            UIRoot.ForcedLoseCanvas(true);
        }
    }

    private IEnumerator SwapEnemy()
    {
        yield return UIRoot.HudRoot.RadialDissolveController.DissolveAnimation(3f, DissolveType.Increase);
        UIRoot.ForcedCanvas(false);
        LoadBattle(RoundCurrent);
    }

    private IEnumerator EndGame()
    {
        yield break;
    }
}
