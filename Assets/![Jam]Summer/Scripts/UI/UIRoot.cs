using BitterCMS.UnityIntegration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIRoot : MonoBehaviour
{
    private Root _root = null;

    [field: SerializeField] public Canvas CanvasRoot { get; private set; }
    [field: SerializeField] public HUD HudRoot { get; private set; }

    private Vector3 _changesTextInitialPosition;
    private Vector3 _changesTextInitialScale;

    [SerializeField] private TMP_Text _priseHouseTexts;
    [SerializeField] private TMP_Text _priseBowmanTexts;
    [SerializeField] private TMP_Text _priseSwordsmanTexts;
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private TMP_Text _changesMoneyText;

    private void Start()
    {
        _root = GlobalState.GetRoot<Root>();
        _root.Player.MoneyChangeUI += MoneyChangeUI;
        _moneyText.text = _root.Player.Money.ToString();

        SetChangesMoney();

        SetPrises();
    }

    private void SetPrises()
    {
        _priseHouseTexts.text = _root.Player.Cards.Build.Price.ToString();
        _priseBowmanTexts.text = _root.Player.Cards.Bow.Price.ToString();
        _priseSwordsmanTexts.text = _root.Player.Cards.Sword.Price.ToString();
    }

    private void SetChangesMoney()
    {
        _changesMoneyText.alpha = 0f;
        _changesTextInitialPosition = _changesMoneyText.transform.localPosition;
        _changesTextInitialScale = _changesMoneyText.transform.localScale;
    }

    public void MoneyChangeUI(int money)
    {
        var previousMoney = int.Parse(_moneyText.text);
        var difference = money - previousMoney;

        _moneyText.text = money.ToString();

        AnimationMoneyChangeUI(difference);
    }

    public void SpawnHouseUI()
    {
        _root.Player.SpawnBuild();
    }

    public void SpawnBowmanUI()
    {
        _root.Player.SpawnBowman();
    }

    public void SpawnSwordsmanUI()
    {
        _root.Player.SpawnSwordsman();
    }

    private void OnDestroy()
    {
        _root.Player.MoneyChangeUI -= MoneyChangeUI;
    }

    private void AnimationMoneyChangeUI(in int difference = 0)
    {

        if (_changesMoneyText != null)
        {
            _changesMoneyText.DOKill();
            _changesMoneyText.transform.DOKill();

            _changesMoneyText.text = (difference > 0 ? "+" : "") + difference.ToString();
            _changesMoneyText.color = difference >= 0 ? Color.green : Color.red;
            _changesMoneyText.alpha = 1f;
            _changesMoneyText.transform.localPosition = _changesTextInitialPosition;
            _changesMoneyText.transform.localScale = _changesTextInitialScale;

            _changesMoneyText.transform.DOLocalMoveY(_changesTextInitialPosition.y + 50f, 1f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _changesMoneyText.transform.localPosition = _changesTextInitialPosition;
                }).Play();

            _changesMoneyText.DOFade(0f, 1f)
                .SetEase(Ease.InQuad)
                .OnComplete(() => _changesMoneyText.alpha = 0f).Play();

            _changesMoneyText.transform.DOScale(_changesTextInitialScale * 0.5f, 1f)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    _changesMoneyText.transform.localScale = _changesTextInitialScale;
                }).Play();
        }
    }
}
