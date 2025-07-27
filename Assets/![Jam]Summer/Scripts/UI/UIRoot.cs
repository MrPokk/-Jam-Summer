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
    [field: SerializeField] public UIHoverToolkit UiHoverToolkit { get; private set; }

    [SerializeField] private HUD _hudRoot;
    [SerializeField] private TMP_Text _priceBuildText;
    [SerializeField] private TMP_Text _priceBowmanText;
    [SerializeField] private TMP_Text _priceSwordsmanText;
    [SerializeField] private TMP_Text _priceWizardText;
    [SerializeField] private TMP_Text _priceCavalryText;
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

        UiHoverToolkit.HideTooltip();

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
        _priceBuildText.text = _root.Player.Cards.Builds.Find(e => e.Type == TypeCard.Build).Price.ToString();
        _priceBowmanText.text = _root.Player.Cards.Entities.Find(e => e.Type == TypeCard.Bowman).Price.ToString();
        _priceSwordsmanText.text = _root.Player.Cards.Entities.Find(e => e.Type == TypeCard.Swordsman).Price.ToString();
        _priceWizardText.text = _root.Player.Cards.Entities.Find(e => e.Type == TypeCard.Wizard).Price.ToString();
        _priceCavalryText.text = _root.Player.Cards.Entities.Find(e => e.Type == TypeCard.Cavalry).Price.ToString();
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

    public DescriptionEntityComponent GetCardDescription(TypeCard cardType)
    {
        var entity = _root.Player.Cards.Entities.Find(e => e.Type == cardType);
        if (entity != null)
        {
            return entity.GetComponent<DescriptionEntityComponent>();
        }

        var build = _root.Player.Cards.Builds.Find(e => e.Type == cardType);
        if (build != null)
        {
            return build.GetComponent<DescriptionEntityComponent>();
        }

        return null;
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

    public void SpawnHouseUI() => _root.Player.SpawnBuild(TypeCard.Build);
    public void SpawnBowmanUI() => _root.Player.SpawnEntity(TypeCard.Bowman);
    public void SpawnSwordsmanUI() => _root.Player.SpawnEntity(TypeCard.Swordsman);
    public void SpawnWizardUI() => _root.Player.SpawnEntity(TypeCard.Wizard);
    public void SpawnCavalryUI() => _root.Player.SpawnEntity(TypeCard.Cavalry);

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }
}
