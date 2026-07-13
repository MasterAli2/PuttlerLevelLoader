using Il2Cpp;
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
    public GameObject? mainButtonToggle;
    public GameObject? switchButtonObj;
    public void Start()
    {
        mainCanvas = GameObject.Find("Main Canvas");
        mainButtonToggle = mainCanvas.transform.GetChild(2).gameObject;
        mainButtonHolder = mainButtonToggle.transform.GetChild(2).gameObject;

        removeDefaultCameraMovement();
        setupSwitchButton();
        //setupEditorButtons();
    }

    void removeDefaultCameraMovement()
    {
        Camera.main.GetComponent<CameraScroll>().speed = 0f;

        mainButtonToggle.transform.GetChild(0).gameObject.SetActive(false);
        mainButtonToggle.transform.GetChild(1).gameObject.SetActive(false);
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
        if (EditorManager.Instance.isActive)
        {
            mainButtonHolder.SetActive(true);
            //editorButtons1.SetActive(false);
        }
        else
        {
            mainButtonHolder.SetActive(false);
            //editorButtons1.SetActive(true);
        }
        EditorManager.Instance.isActive = !EditorManager.Instance.isActive;
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