
using BitterCMS.CMSSystem;
using BitterCMS.Utility.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugUtility : InteractionCore, IEnterInUpdate
{
    public void Update(float timeDelta)
    {
        if (Input.GetKeyUp(KeyCode.R))
            SceneManager.LoadScene(sceneBuildIndex: 0);
        
        if (!Input.GetKeyUp(KeyCode.Q))
            return;

        var allPresenter = CMSRuntimer.GetAllPresenters();

        foreach (var presenter in allPresenter)
        {
            var allEntity = presenter.GetAllEntities();
            foreach (var entity in allEntity)
            {
                entity.RefreshComponent();
            }
        }
    }
}
