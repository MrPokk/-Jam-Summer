using BitterCMS.UnityIntegration;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

public class UIRoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas _canvasRoot;    
    [SerializeField] private HUD _hudRoot;
    [SerializeField] private TMP_Text _priceBuildText;
    [SerializeField] private TMP_Text _priceBowmanText;
    [SerializeField] private TMP_Text _priceSwordsmanText;
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private TextPixelOutline _changesMoneyContainer;

    private Root _root;
    private Vector3 _changesTextInitialPosition;
    private Vector3 _changesTextInitialScale;

    public Canvas CanvasRoot => _canvasRoot;
    public HUD HudRoot => _hudRoot;

    public void InitializeUI()
    {
        _canvasRoot.gameObject.SetActive(false);
        _hudRoot.gameObject.SetActive(true);

        _root = GlobalState.GetRoot<Root>();
        _moneyText.text = _root.Player.Money.ToString();

        _changesTextInitialPosition = _changesMoneyContainer.GetPosition();
        _changesTextInitialScale = _changesMoneyContainer.GetScale();

        SetupChangeMoneyText();
        UpdatePrices();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _root.Player.MoneyChangeUI += OnMoneyChanged;
    }

    private void UnsubscribeFromEvents()
    {
        if (_root != null && _root.Player != null)
        {
            _root.Player.MoneyChangeUI -= OnMoneyChanged;
        }
    }

    private void SetupChangeMoneyText()
    {
        _changesMoneyContainer.SetAlpha(0f);
    }

    private void UpdatePrices()
    {
        _priceBuildText.text = _root.Player.Cards.Builds.FirstOrDefault(e => e is CardBuild).Price.ToString();
        _priceBowmanText.text = _root.Player.Cards.Entities.FirstOrDefault(e => e is CardBowman).Price.ToString();
        _priceSwordsmanText.text = _root.Player.Cards.Entities.FirstOrDefault(e => e is CardSwordsman).Price.ToString();
    }

    public void ToggleCanvas()
    {
        _canvasRoot.gameObject.SetActive(!_canvasRoot.gameObject.activeSelf);
    }

    private void OnMoneyChanged(int newMoney)
    {
        var previousMoney = int.Parse(_moneyText.text);
        var difference = newMoney - previousMoney;


        _moneyText.text = newMoney.ToString();

        PlayMoneyChangeAnimation(difference);
    }

    private void PlayMoneyChangeAnimation(int difference)
    {
        if (difference == 0) return;

        var transform = _changesMoneyContainer.GetTransform();

        transform.DOKill();

        _changesMoneyContainer.SetAll(
            textColor: difference > 0 ? Color.green : Color.red,
            outlineColor: Color.black,
            alpha: 1f,
            text: $"{(difference > 0 ? "+" : "")}{difference}",
            scale: Vector3.zero,
            position: _changesTextInitialPosition
        );

        var sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(_changesTextInitialScale * 1.2f, 0.2f).SetEase(Ease.OutBack));
        sequence.Append(transform.DOScale(_changesTextInitialScale, 0.1f));

        sequence.Append(transform.DOLocalMoveY(_changesTextInitialPosition.y + 50f, 0.5f)).OnUpdate(() =>
        {
            _changesMoneyContainer.SetAlpha(1f - (transform.localPosition.y - _changesTextInitialPosition.y) / 50f);
        });

        sequence.OnComplete(() =>
        {
            transform.localPosition = _changesTextInitialPosition;
            _changesMoneyContainer.SetAlpha(0f);
        });

        sequence.Play();
    }

    public void SpawnHouseUI(int index) => _root.Player.SpawnBuild(index);
    public void SpawnEntityUI(int index) => _root.Player.SpawnEntity(index);

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }
}
