using Harmony;
using MelonLoader;
using MelonLoader.TinyJSON;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSceneManager
{
    public const int gameSceneIndex = 5;

    public static int nextGameSceneLoadTarget = 0;
    public static int gameUpdateTarget = 0;
    public static int lastGameUpdateTarget = 0;



    public static void onSceneLoaded(string sceneName)
    {

        if (sceneName != "Game")
        {
            return;
        }

        if (nextGameSceneLoadTarget == 1)
        {
            LevelManager.loadCustomLevel();

            nextGameSceneLoadTarget = 0;
            return;
        }
        else //if (nextGameSceneLoadTarget == 2)
        {
            //EditorUI.loadEditor();
        }


    }

    public static void onUpdateScene(string sceneName)
    {
        if (sceneName != "Game")
        {
            if (gameUpdateTarget == 1)
            {
                //LevelManager.reset();
            }

            return;
        }

        if (gameUpdateTarget == 1)
        {

        }
        else
        {
            //EditorUI.onUpdateScene();
        }


        
    }
}