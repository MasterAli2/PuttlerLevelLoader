namespace puttlerLevelLoader;

using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PuttlerLevelLoader : MelonMod
{

    public override void OnInitializeMelon()
    {
        Physics2D.autoSyncTransforms = true;

        RegisterInIl2Cpp();

        LevelObjectRegistry.Build();
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

    private void RegisterInIl2Cpp(){

        ClassInjector.RegisterTypeInIl2Cpp<BaseLevelObject>();
        ClassInjector.RegisterTypeInIl2Cpp<LinkedLevelObject>();

        ClassInjector.RegisterTypeInIl2Cpp<EditorCamera>();
        ClassInjector.RegisterTypeInIl2Cpp<EditorManager>();
        ClassInjector.RegisterTypeInIl2Cpp<EditorOutline>();
        ClassInjector.RegisterTypeInIl2Cpp<EditorToolController>();
        ClassInjector.RegisterTypeInIl2Cpp<EditorUI>();

        ClassInjector.RegisterTypeInIl2Cpp<LevelManager>();

        ClassInjector.RegisterTypeInIl2Cpp<VanillaBlackholeLevelObject>();
        ClassInjector.RegisterTypeInIl2Cpp<VanillaHoleLevelObject>();
        ClassInjector.RegisterTypeInIl2Cpp<VanillaMovingPlatformLevelObject>();
        ClassInjector.RegisterTypeInIl2Cpp<VanillaPortalLevelObject>();
        ClassInjector.RegisterTypeInIl2Cpp<VanillaSurfaceBlockLevelObject>();
    }
}