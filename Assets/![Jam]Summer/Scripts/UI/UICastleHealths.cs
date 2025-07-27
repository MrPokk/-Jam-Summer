using TMPro;
using UnityEngine;

public class UICastleHealths : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _healthText;
    private int _health;

    private CardCastle _cardCastle;


    private void Start()
    {
        _cardCastle = GetComponentInParent<CardCastle>();

        OnHealthChanged();
    }

    private void Update()
    {
        OnHealthChanged();
    }

    private void OnHealthChanged()
    {
        if (_health != _cardCastle.Health || _health == 0)
        {
            _health = _cardCastle.Health;
            _healthText.text = $"HP: {_health}";
        }
    }
}
