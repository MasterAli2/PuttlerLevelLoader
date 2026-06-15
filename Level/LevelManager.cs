using Harmony;
using MelonLoader;
using MelonLoader.TinyJSON;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    public static CustomLevel? currentLevel;
    public static int levelIndex;
    public static List<GameObject>? currentLevelObjs;

    public static bool inCustomLevel;
    public static void openCustomLevel(int levelIndex)
    {
        MelonLogger.Msg(levelIndex);
        MelonLogger.Msg(LocalData.customLevels.Count);
        currentLevel = LocalData.customLevels[levelIndex];

        LevelManager.levelIndex = levelIndex;
        inCustomLevel = true;
        //MelonLogger.Msg(loadLevel == null);

        GameSceneManager.nextGameSceneLoadTarget = 1;

        SceneManager.LoadScene(GameSceneManager.gameSceneIndex);
    }


    public static void loadCustomLevel()
    {
        if (currentLevel == null){
            MelonLogger.Error("Failed to load custom level");
            return;
        }

        LevelBuilder.getPrefabsAndEmpty();

        currentLevelObjs = LevelBuilder.buildFromObj(currentLevel);
        LevelBuilder.setMetadata(currentLevel);

        inCustomLevel = true;
        GameSceneManager.gameUpdateTarget = 1;

        
    }
    public static void reset()
    {
        currentLevel = null;
        levelIndex = -1;
        inCustomLevel = false;
    }

    public static void onUpdateScene()
    {
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
            currentLevel = level;
            currentLevelObjs = LevelBuilder.buildFromObj(currentLevel);

        }
    }
}