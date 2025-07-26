using BitterCMS.UnityIntegration;
using UnityEngine;

public class UIRoot : MonoBehaviour
{

    private Root _root = null;

    private void Start()
    {
        _root = GlobalState.GetRoot<Root>();
    }

    public void SpawnHouseUI()
    {
        _root.Player.SpawnBuild();
    }

    public void SpawnBowmanUI()
    {
        _root.Player.SpawnBowman();
    }

    public void SpawnSwordsmanUI()
    {
        _root.Player.SpawnSwordsman();
    }
}
