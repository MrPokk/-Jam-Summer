using BitterCMS.UnityIntegration;
using BitterCMS.Utility.Interfaces;
using DG.Tweening;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class UIHoverToolkit : MonoBehaviour, IInitializable
{
    private UIRoot _uIRoot;
    private DescriptionEntityComponent _currentEntity;
    private RectTransform _rectTransform;

    private Vector3 _targetScaleOriginal;

    [Header("UI References")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _attackText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _abilityText;


    [Header("Position Settings")]
    [SerializeField] private Vector2 _offset = new Vector2(20, -20);

    public void Init()
    {
        _uIRoot = GlobalState.GetRoot<Root>().UIRoot;
        _rectTransform = GetComponent<RectTransform>();
        _targetScaleOriginal = transform.localScale;
        HideTooltip();
    }

    private void Update()
    {
        if (gameObject.activeSelf && _currentEntity != null)
        {
            FollowMouse();
        }
    }

    public void StartHover(TypeCard cardType)
    {
        _currentEntity = _uIRoot.GetCardDescription(cardType);
        SetTooltip();
        ShowTooltip();
        FollowMouse();
    }

    public void EndHover()
    {
        HideTooltip();
        _currentEntity = null;
    }

    private void FollowMouse()
    {
        Vector2 mousePosition = Input.mousePosition;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform.parent as RectTransform,
            mousePosition,
            null,
            out Vector2 localPoint))
        {
            _rectTransform.localPosition = localPoint + _offset;
        }
    }

    private void SetTooltip()
    {
        if (_currentEntity == null) return;

        _nameText.text = _currentEntity.EntityName;
        _healthText.text = _currentEntity.Health.ToString();
        _attackText.text = _currentEntity.Attack.ToString();
        _descriptionText.text = _currentEntity.Description;
        _abilityText.text = _currentEntity.Ability.ToString();
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void ShowTooltip()
    {
        gameObject.SetActive(true);
        AnimationShow();

    }

    private void AnimationShow()
    {
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        transform.DOScale(_targetScaleOriginal, 0.3f)
            .SetEase(Ease.OutBack).Play();
    }
}
