using MelonLoader;
using puttlerLevelLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[MelonLoader.RegisterTypeInIl2Cpp]
public class EditorUI : MonoBehaviour
{
    public EditorUI(IntPtr ptr) : base(ptr) {}

    public GameObject? editorButtons1;
    public GameObject? mainCanvas;
    public GameObject? mainButtonHolder;
    public GameObject? switchButtonObj;
    public void Start()
    {
        mainCanvas = GameObject.Find("Main Canvas");
        mainButtonHolder = mainCanvas.transform.GetChild(2).GetChild(2).gameObject;


        setupSwitchButton();
        //setupEditorButtons();
    }
    void setupSwitchButton()
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
    void toggleEditor()
    {
        if (inEditor)
        {
            mainButtonHolder.SetActive(true);
            //editorButtons1.SetActive(false);
            inEditor = !inEditor;
        }
        else
        {
            mainButtonHolder.SetActive(false);
            //editorButtons1.SetActive(true);
            inEditor = !inEditor;

        }
    }
    void setupEditorButtons()
    {

        editorButtons1 = GameObject.Instantiate(mainButtonHolder, mainButtonHolder.transform.parent);
    
        editorButtons1.transform.name = "Editor Buttons";
        editorButtons1.SetActive(false);
    }
    public static void onUpdateScene()
    {
        
    }

}