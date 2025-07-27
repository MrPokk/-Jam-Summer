using TMPro;
using UnityEngine;

public class UIPrice : MonoBehaviour
{
    public void SetPrice(int price) => GetComponent<TextMeshProUGUI>().text = price.ToString();
}
