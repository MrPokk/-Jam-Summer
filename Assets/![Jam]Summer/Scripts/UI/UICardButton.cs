using BitterCMS.UnityIntegration;
using UnityEngine;

[DisallowMultipleComponent]
public class UICardButton : MonoBehaviour
{
    private UIRoot _uIRoot;
    [SerializeField] private TypeCard _cardType;

    private void Start()
    {
        _uIRoot = GlobalState.GetRoot<Root>().UIRoot;
    }

    public void OnPointerEnter()
    {
        Debug.Log("OnPointerEnter");
        _uIRoot.UiHoverToolkit.StartHover(_cardType);
    }

    public void OnPointerExit()
    {
         Debug.Log("OnPointerExit");
        _uIRoot.UiHoverToolkit.EndHover();
    }
}
