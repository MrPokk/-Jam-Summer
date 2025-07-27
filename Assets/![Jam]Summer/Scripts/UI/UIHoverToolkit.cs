using BitterCMS.UnityIntegration;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class UIHoverToolkit : MonoBehaviour
{
    private UIRoot _uIRoot;
    private DescriptionEntityComponent _currentEntity;

    [Header("UI References")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _attackText;
    [SerializeField] private TMP_Text _descriptionText;

    private void Start() => _uIRoot = GlobalState.GetRoot<Root>().UIRoot;

    public void StartHover(TypeCard cardType)
    {
        _currentEntity = _uIRoot.GetCardDescription(cardType);
        ShowTooltip();
    }

    public void EndHover()
    {
        HideTooltip();
        _currentEntity = null;
    }

    private void ShowTooltip()
    {
        if (_currentEntity == null) return;

        _nameText.text = _currentEntity.EntityName;
        _healthText.text = _currentEntity.Health.ToString();
        _attackText.text = _currentEntity.Attack.ToString();
        _descriptionText.text = _currentEntity.Description;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public void UpdateStats()
    {
        if (_currentEntity != null && _nameText.gameObject.activeSelf)
        {
            _healthText.text = _currentEntity.Health.ToString();
            _attackText.text = _currentEntity.Attack.ToString();
        }
    }
}
