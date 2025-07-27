using System.Collections.Generic;
using System.Linq;
using BitterCMS.UnityIntegration;
using BitterCMS.Utility.Interfaces;
using TMPro;
using UnityEngine;

public class UIManagementPanel : MonoBehaviour, IInitializable
{
    [field: SerializeField] public List<UICardButton> Buttons { get; private set; }
    [field: SerializeField] public UIMoneyPanel MoneyPanel { get; private set; }

    private Root _root;
    private IReadOnlyCollection<Card> _cards;


    public void Init()
    {
        _root = GlobalState.GetRoot<Root>();
        _cards = _root.Player.Cards.GetAll();

        foreach (var button in Buttons)
        {
            button.Init();
        }

        MoneyPanel.SetCurrentMoney(_root.Player.Money);
        MoneyPanel.SetChangeText("");
        SetPrice();
    }

    private void SetPrice()
    {
        foreach (var button in Buttons)
        {
            var card = _cards.FirstOrDefault(b => b.Type == button.Type);
            if (card != null)
            {
                button.Price.SetPrice(card.Price);
            }
            else
            {
                Debug.Log($"Card {button.Type} not found");
            }
        }
    }
}
