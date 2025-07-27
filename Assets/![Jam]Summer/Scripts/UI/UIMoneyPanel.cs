using BitterCMS.Utility.Interfaces;
using TMPro;
using UnityEngine;

public class UIMoneyPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyCurrent;
    [field: SerializeField] public TextPixelOutline ChangesMoneyContainer { get; private set; }

    public void SetCurrentMoney(int money) => _moneyCurrent.text = money.ToString();

    public string GetCurrentMoney() => _moneyCurrent.text;

    public void SetChangeText(string text) => ChangesMoneyContainer.SetContent(text);

    public string GetChangeText() => ChangesMoneyContainer.GetText();
}
