using System.Runtime.CompilerServices;
using Harmony;
using MelonLoader;
using MelonLoader.TinyJSON;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public LevelManager(IntPtr ptr) : base(ptr) {}
    public static CustomLevel? currentLevel;
    public static bool asEditor = false;
    public static string levelId;
    public static List<BaseLevelObject>? currentLevelObjs;

    public static bool inCustomLevel;
    public static void openCustomLevel(string levelId, bool asEditor = false)
    {
        currentLevel = LocalData.localLevels[levelId];

        LevelManager.levelId = levelId;
        inCustomLevel = true;

        LevelManager.asEditor = asEditor;

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

        if (asEditor)
        {
            (new GameObject("Editor Manager")).AddComponent<EditorManager>();
        }

        GameObject managerObj = new GameObject("Level Manager");
        managerObj.AddComponent<LevelManager>();
    
    }
    public static void OnDestroy()
    {
        currentLevel = null;
        levelId = "";
        inCustomLevel = false;

        MelonLogger.Msg("OnDestroy() LevelManager");
    }

    public static void Update()
    {
        if (Input.GetKeyDown("tab"))
        {
            LocalData.init();
            CustomLevel? level;

            if (levelId == "")
            {
                return;
            }

            level = LocalData.localLevels[levelId];
            

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