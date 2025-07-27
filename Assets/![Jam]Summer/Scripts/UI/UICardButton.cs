using BitterCMS.UnityIntegration;
using UnityEngine;
using UnityEngine.EventSystems; // Добавляем это пространство имен

[DisallowMultipleComponent]
public class UICardButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UIRoot _uIRoot;
    [SerializeField] private TypeCard _cardType;

    private void Start()
    {
        _uIRoot = GlobalState.GetRoot<Root>().UIRoot;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _uIRoot.UiHoverToolkit.StartHover(_cardType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _uIRoot.UiHoverToolkit.EndHover();
    }
}
