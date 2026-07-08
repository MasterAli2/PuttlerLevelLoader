using Harmony;
using MelonLoader;
using MelonLoader.TinyJSON;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

[MelonLoader.RegisterTypeInIl2Cpp]
public class LevelManager : MonoBehaviour
{
    public LevelManager(IntPtr ptr) : base(ptr) {}
    public static CustomLevel? currentLevel;
    public static int levelIndex;
    public static List<GameObject>? currentLevelObjs;

    public static bool inCustomLevel;
    public static void openCustomLevel(int levelIndex)
    {
        currentLevel = LocalData.customLevels[levelIndex];

        LevelManager.levelIndex = levelIndex;
        inCustomLevel = true;

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

        GameObject managerObj = new GameObject("Level Manager");
        managerObj.AddComponent<LevelManager>();
    
    }
    public static void OnDestroy()
    {
        currentLevel = null;
        levelIndex = -1;
        inCustomLevel = false;

        MelonLogger.Msg("OnDestroy() LevelManager");
    }

    public static void Update()
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