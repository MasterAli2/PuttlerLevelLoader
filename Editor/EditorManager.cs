using MelonLoader;
using puttlerLevelLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class EditorManager
{
    public static bool inEditor = false;

    public static GameObject? editorButtons1;
    public static GameObject? mainCanvas;
    public static GameObject? mainButtonHolder;
    public static GameObject? switchButtonObj;
    public static void loadEditor()
    {
        mainCanvas = GameObject.Find("Main Canvas");
        mainButtonHolder = mainCanvas.transform.GetChild(2).GetChild(2).gameObject;


        setupSwitchButton();
        setupEditorButtons();
    }
    static void setupSwitchButton()
    {
        MelonLogger.Msg(mainButtonHolder.transform.name);
        MelonLogger.Msg(mainButtonHolder.transform.parent.name);
        GameObject a = mainButtonHolder.transform.parent.GetChild(3)
        .GetChild(1)
        .GetChild(1)
        .GetChild(3)
        .gameObject;

        switchButtonObj = GameObject.Instantiate(a, a.transform.parent);
        switchButtonObj.SetActive(true);

        Button switchButton = switchButtonObj.GetComponent<Button>();
        switchButton.interactable = true;
        switchButton.onClick = new Button.ButtonClickedEvent();
        switchButton.onClick.AddListener((System.Action)(() => {toggleEditor();}));

    }
    static void toggleEditor()
    {
        if (inEditor)
        {
            mainButtonHolder.SetActive(true);
            editorButtons1.SetActive(false);
            inEditor = !inEditor;
        }
        else
        {
            mainButtonHolder.SetActive(false);
            editorButtons1.SetActive(true);
            inEditor = !inEditor;

        }
    }
    static void setupEditorButtons()
    {

        editorButtons1 = GameObject.Instantiate(mainButtonHolder, mainButtonHolder.transform.parent);
    
        editorButtons1.transform.name = "Editor Buttons";
        editorButtons1.SetActive(false);
    }
    public static void onUpdateScene()
    {
        
    }

}