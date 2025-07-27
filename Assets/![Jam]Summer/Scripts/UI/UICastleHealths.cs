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
        _health = _cardCastle.Health;

        OnHealthChanged();
    }

    private void Update()
    {
        if (_health != _cardCastle.Health)
        {
            OnHealthChanged();
        }
    }

    private void OnHealthChanged()
    {
        _healthText.text = $"HP: {_cardCastle.Health}";
    }
}
