using BitterCMS.UnityIntegration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIRoot : MonoBehaviour
{
    private Root _root = null;
    private Vector3 _changesTextInitialPosition;
    private Vector3 _changesTextInitialScale; // Сохраняем начальный масштаб

    [SerializeField]
    private TMP_Text _moneyText = null;
    [SerializeField]
    private TMP_Text _changesMoneyText = null;

    private void Start()
    {
        _root = GlobalState.GetRoot<Root>();
        _root.Player.MoneyChanged += MoneyChangeUI;
        _moneyText.text = _root.Player.Money.ToString();

        if (_changesMoneyText != null)
        {
            _changesMoneyText.alpha = 0f;
            _changesTextInitialPosition = _changesMoneyText.transform.localPosition;
            _changesTextInitialScale = _changesMoneyText.transform.localScale;
        }
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
    }

    public void SpawnBowmanUI()
    {
        _root.Player.SpawnBow();
    }

    public void SpawnSwordsmanUI()
    {
        _root.Player.SpawnSword();
    }

    private void OnDestroy()
    {
        _root.Player.MoneyChanged -= MoneyChangeUI;
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
