using BitterCMS.UnityIntegration;
using BitterCMS.Utility.Interfaces;
using TMPro;
using UnityEngine;
using DG.Tweening; // Add DoTween namespace

[DisallowMultipleComponent]
public class UIHoverToolkit : MonoBehaviour, IInitializable
{
    private UIRoot _uIRoot;
    private DescriptionEntityComponent _currentEntity;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup; // Added for fade effects

    [Header("UI References")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _attackText;
    [SerializeField] private TMP_Text _descriptionText;
    
    [Header("Position Settings")]
    [SerializeField] private Vector2 _offset = new Vector2(20, -20);

    [Header("Animation Settings")]
    [SerializeField] private float _fadeDuration = 0.2f;
    [SerializeField] private float _scaleDuration = 0.15f;
    [SerializeField] private float _showScale = 1f;
    [SerializeField] private float _hideScale = 0.8f;

    public void Init()
    {
        _uIRoot = GlobalState.GetRoot<Root>().UIRoot;
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        // Initialize hidden state
        _canvasGroup.alpha = 0f;
        _rectTransform.localScale = Vector3.one * _hideScale;
        gameObject.SetActive(false);
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
    }

    private void HideTooltip()
    {
        // Kill any existing animations to prevent conflicts
        _canvasGroup.DOKill();
        _rectTransform.DOKill();
        
        // Fade out and scale down
        _canvasGroup.DOFade(0f, _fadeDuration)
            .OnComplete(() => gameObject.SetActive(false));
            
        _rectTransform.DOScale(_hideScale, _scaleDuration);
    }

    private void ShowTooltip()
    {
        gameObject.SetActive(true);
        
        // Kill any existing animations to prevent conflicts
        _canvasGroup.DOKill();
        _rectTransform.DOKill();
        
        // Reset values before animation
        _canvasGroup.alpha = 0f;
        _rectTransform.localScale = Vector3.one * _hideScale;
        
        // Fade in and scale up
        _canvasGroup.DOFade(1f, _fadeDuration);
        _rectTransform.DOScale(_showScale, _scaleDuration);
    }

    public void UpdateStats()
    {
        if (_currentEntity != null && _canvasGroup.alpha > 0.9f) // Check if tooltip is visible
        {
            _healthText.text = _currentEntity.Health.ToString();
            _attackText.text = _currentEntity.Attack.ToString();
        }
    }
}
