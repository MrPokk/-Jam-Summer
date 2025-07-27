using BitterCMS.UnityIntegration;
using BitterCMS.Utility.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class UICardButton : MonoBehaviour, IInitializable, IPointerEnterHandler, IPointerExitHandler
{
    private UIRoot _uIRoot;
    public UIPrice Price { get; private set; }
    [field: SerializeField] public TypeCard Type { get; private set; }

    public void Init()
    {
        _uIRoot = GlobalState.GetRoot<Root>().UIRoot;

        Price = GetComponentInChildren<UIPrice>();

        if (Price == null)
            throw new System.NotImplementedException($"Not implemented {Price} in {gameObject.name}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _uIRoot.ToolkitHover(Type);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _uIRoot.ToolkitHoverEnd();
    }
}
