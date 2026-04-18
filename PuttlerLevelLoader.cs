namespace puttlerLevelLoader;

using MelonLoader;
using UnityEngine.SceneManagement;


public class PuttlerLevelLoader : MelonMod
{

    public override void OnInitializeMelon()
    {
        BundleManager.init();
        LocalData.init();

    }

    public override void OnUpdate()
    {   
        string sceneName = SceneManager.GetActiveScene().name;

        GameSceneManager.onUpdateScene(sceneName);
        
    }
    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        GameSceneManager.onSceneLoaded(sceneName);
        
        MainMenu.onSceneLoaded(sceneName);
        
        LevelsMenu.onSceneLoaded(sceneName);
    }
}