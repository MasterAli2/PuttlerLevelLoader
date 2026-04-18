using Harmony;
using MelonLoader;
using MelonLoader.TinyJSON;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSceneManager
{
    public static CustomLevel? loadLevel;
    public static CustomLevel? activeLevel;
    public static int levelIndex;
    public static List<GameObject>? currentLevelObjs;

    public static bool inCustomLevel;
    public static void openCustomLevel(int levelIndex)
    {
        loadLevel = LocalData.customLevels[levelIndex];
        GameSceneManager.levelIndex = levelIndex;
        inCustomLevel = true;
        //MelonLogger.Msg(loadLevel == null);
        SceneManager.LoadScene(5);
    }


    public static void onSceneLoaded(string sceneName)
    {

        if (sceneName != "Game")
        {
            return;
        }

        if (loadLevel == null)
        {
            MelonLogger.Warning("Game scene loaded but no level queued");
            return;
        }

        handleGameLoad();
    }

    static void handleGameLoad()
    {
        if (loadLevel == null){
            MelonLogger.Error("Failed to load custom level");
            return;
        }

        LevelBuilder.getPrefabsAndEmpty();

        currentLevelObjs = LevelBuilder.buildFromObj(loadLevel);
        LevelBuilder.setMetadata(loadLevel);
        activeLevel = loadLevel;
        loadLevel = null;
        inCustomLevel = true;
        
    }
    public static void reset()
    {
        loadLevel = null;
        activeLevel = null;
        levelIndex = -1;
        inCustomLevel = false;
    }

    public static void onUpdateScene(string sceneName)
    {
        if (sceneName != "Game")
        {
            return;
        }
        if (sceneName != "Game" && inCustomLevel && loadLevel == null)
        {
            reset();
            return;
        }
        

        if (Input.GetKeyDown("tab"))
        {
            LocalData.init();
            CustomLevel? level;

            if (levelIndex < 0 || levelIndex >= LocalData.customLevels.Count)
            {
                return;
            }

            level = LocalData.customLevels[levelIndex];
            

            if (currentLevelObjs != null)
            {
                foreach (var obj1 in currentLevelObjs)
                {
                    GameObject.Destroy(obj1);
                }
            }

            if (level == null) return;
            activeLevel = level;
            currentLevelObjs = LevelBuilder.buildFromObj(activeLevel);

        }
    }
}