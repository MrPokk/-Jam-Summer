using BitterCMS.UnityIntegration;
using UnityEngine;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour
{

    private Root _root = null;

    private void Start()
    {
        _root = GlobalState.GetRoot<Root>();
    }

    public void SpawnHouseUI()
    {
    }

    public void SpawnBowmanUI()
    {
        _root.Player.SpawnBow();
    }

    public void SpawnSwordsmanUI()
    {
        _root.Player.SpawnSword();
    }
}
