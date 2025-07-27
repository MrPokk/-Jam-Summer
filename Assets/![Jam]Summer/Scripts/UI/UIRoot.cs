using BitterCMS.UnityIntegration;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class UIRoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas _canvasRoot;
    [SerializeField] private UIHoverToolkit _uIHoverToolkit;
    [SerializeField] private UIManagementPanel _uIManagementPanel;
    [SerializeField] private UILoseCanvas _uILoseCanvas;

    [SerializeField] private HUD _hudRoot;
    private Root _root;
    private Vector3 _changesTextInitialPosition;
    private Vector3 _changesTextInitialScale;

    public Canvas CanvasRoot => _canvasRoot;
    public HUD HudRoot => _hudRoot;
    public void InitializeUI()
    {
        _root = GlobalState.GetRoot<Root>();

        _uIHoverToolkit.Init();
        _uIManagementPanel.Init();

        _canvasRoot.gameObject.SetActive(false);
        _hudRoot.gameObject.SetActive(true);
        _uILoseCanvas.gameObject.SetActive(false);

        _changesTextInitialPosition = _uIManagementPanel.MoneyPanel.ChangesMoneyContainer.GetPosition();
        _changesTextInitialScale = _uIManagementPanel.MoneyPanel.ChangesMoneyContainer.GetScale();

        SubscribeToEvents();
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

    private void OnMoneyChanged(int newMoney)
    {
        var currentMoneyText = _uIManagementPanel.MoneyPanel.GetCurrentMoney();
        if (string.IsNullOrEmpty(currentMoneyText)) currentMoneyText = "0";

        var previousMoney = int.Parse(currentMoneyText);
        var difference = newMoney - previousMoney;

        _uIManagementPanel.MoneyPanel.SetCurrentMoney(newMoney);
        PlayMoneyChangeAnimation(difference);
    }


    private void PlayMoneyChangeAnimation(int difference)
    {
        if (difference == 0) return;

        var changesContainer = _uIManagementPanel.MoneyPanel.ChangesMoneyContainer;
        var transform = changesContainer.GetTransform();
        transform.DOKill();

        changesContainer.SetAll(
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

        sequence.Append(transform.DOLocalMoveY(_changesTextInitialPosition.y + 50f, 0.5f)
            .OnUpdate(() =>
            {
                float progress = (transform.localPosition.y - _changesTextInitialPosition.y) / 50f;
                changesContainer.SetAlpha(Mathf.Clamp01(1f - progress));
            })
            .OnComplete(() =>
            {
                changesContainer.SetAlpha(0f);
                transform.localPosition = _changesTextInitialPosition;

            }));

        sequence.Play();
    }

    public void ToggleManagementPanel() => _uIManagementPanel.gameObject.SetActive(!_uIManagementPanel.gameObject.activeSelf);
    public void ToggleCanvas() => _canvasRoot.gameObject.SetActive(!_canvasRoot.gameObject.activeSelf);
    public void ToggleHUD() => _hudRoot.gameObject.SetActive(!_hudRoot.gameObject.activeSelf);
    public void ShowLoseCanvas() => _uILoseCanvas.gameObject.SetActive(true);
    public void ToolkitHover(TypeCard cardType) => _uIHoverToolkit.StartHover(cardType);
    public void ToolkitHoverEnd() => _uIHoverToolkit.EndHover();
    public void RestartLevel() => _root.RestartBattle();
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
