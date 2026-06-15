using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

static class LevelsMenu
{
    public static void onSceneLoaded(string sceneName)
    {
        if (sceneName != "Online") return;

        initLocalLevelsMenu();
        
    }


    
    static void initLocalLevelsMenu()
    {
        LocalData.init();

        GameObject.Find("back").GetComponent<Button>().onClick.AddListener((System.Action)(() =>
            {
                SceneManager.LoadScene("Menu");
            }));
        

        GameObject localLevelsContentRoot = GameObject.Find("Canvas").transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        GameObject baseLLButton = localLevelsContentRoot.transform.GetChild(0).gameObject;
        baseLLButton.SetActive(false);
        if (LocalData.customLevels == null)
        {
            baseLLButton.SetActive(true);
            baseLLButton.GetComponentInChildren<Text>().text = "An error occured while loading the levels";
            return;
        }
        for (int i = 0; i < LocalData.customLevels.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(baseLLButton, baseLLButton.transform.parent);
            obj.SetActive(true);

            obj.GetComponentInChildren<Text>().text = LocalData.customLevels[i].name;

            int index = i; // important fix

            obj.GetComponent<Button>().onClick.AddListener((System.Action)(() =>
            {
                LevelManager.openCustomLevel(index);
            }));
        }
    }
}
