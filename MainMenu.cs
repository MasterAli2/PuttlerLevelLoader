using MelonLoader;
using puttlerLevelLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

static class MainMenu
{
    public static void onSceneLoaded(string sceneName)
    {
        if (sceneName != "Menu") return;

        Canvas canvas = GameObject.Find("Menu Canvas").GetComponent<Canvas>();
        GameObject baseGameButton = canvas.transform.Find("Menu Components (Default Off)").Find("Game Buttons").Find("Shop Button").gameObject;
    
        GameObject onlineButtonObj = GameObject.Instantiate(baseGameButton, baseGameButton.transform.parent);
        onlineButtonObj.transform.localPosition = new Vector3(760, 1500, -16200);

        Button onlineButton = onlineButtonObj.GetComponent<Button>();
        onlineButton.interactable = true;
        onlineButton.onClick = new Button.ButtonClickedEvent();
        onlineButton.onClick.AddListener((System.Action)(() => {MainMenu.onClickOnlineButton();}));
    
    }
    public static void onClickOnlineButton()
    {
        SceneManager.LoadScene(BundleManager.levelsMenuSceneName);
    }

}