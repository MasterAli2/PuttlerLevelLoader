using Il2Cpp;
using Il2CppTMPro;
using MelonLoader;
using puttlerLevelLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EditorUI : MonoBehaviour
{
    public EditorUI(IntPtr ptr) : base(ptr) {}

    public static bool exitedEditor = false;

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
        setupExitButton();


        doLeftBar();
        doTopBar();
        setupEditorButtons();
        doSaveButton();

    }
    
    void setupExitButton()
    {
        Image ExitButtonImage = GameObject.Find("World Toggle").transform.GetChild(0).GetComponent<Image>();
        ExitButtonImage.sprite = BundleManager.ExitButtonSprite;

        Transform world_popup = GameObject.Find("World Components").transform.GetChild(0).GetChild(1).GetChild(2);

        var exitLabel = world_popup.GetChild(0).GetComponent<TextMeshProUGUI>();
        exitLabel.text = "Exit";

        var confirmationText = world_popup.GetChild(2).GetComponent<TextMeshProUGUI>();
        confirmationText.text = "Are you sure you would like to exit? Unsaved data may be lost.";
        confirmationText.enableAutoSizing = true;

        var confirmButton = world_popup.GetChild(3).GetChild(1).GetComponent<Button>();
        confirmButton.onClick = new Button.ButtonClickedEvent();
        confirmButton.onClick.AddListener((System.Action)(() => {
            EditorUI.exitedEditor = true;
            FindObjectOfType<LevelChanger>().FadeToLevel("Menu");
        }));
    }

    void doLeftBar()
    {
        if (BundleManager.leftBarPrefab == null)
        {
            return;
        }
        GameObject leftBar = GameObject.Instantiate(BundleManager.leftBarPrefab, GameObject.Find("Main Button Toggle ").transform).transform.GetChild(0).gameObject;;
        
        leftBar.transform.GetChild(0).GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        leftBar.transform.GetChild(0).GetComponent<Button>().onClick.AddListener((System.Action)(() => {EditorToolController.Instance.setMode(1);}));
        
        leftBar.transform.GetChild(1).GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        leftBar.transform.GetChild(1).GetComponent<Button>().onClick.AddListener((System.Action)(() => {EditorToolController.Instance.setMode(2);}));
        
        leftBar.transform.GetChild(2).GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        leftBar.transform.GetChild(2).GetComponent<Button>().onClick.AddListener((System.Action)(() => {EditorToolController.Instance.setMode(3);}));
    }

    void doTopBar()
    {
        if (BundleManager.topBarPrefab == null)
        {
            return;
        }
        GameObject topBar = GameObject.Instantiate(BundleManager.topBarPrefab, GameObject.Find("Main Button Toggle ").transform).transform.GetChild(0).gameObject;;
        
        GameObject snapGridToggleObj = topBar.transform.GetChild(1).gameObject;
        Button snapGridToggleButton = snapGridToggleObj.GetComponent<Button>();
        snapGridToggleButton.onClick = new Button.ButtonClickedEvent();
        snapGridToggleButton.onClick.AddListener((System.Action)(() => {Utils.flipObjectAndBool(snapGridToggleObj, ref EditorManager.Instance.snapToGrid);}));
        
        GameObject refFrameToggleObj = topBar.transform.GetChild(3).gameObject;
        Button refFrameToggleButton = refFrameToggleObj.GetComponent<Button>();
        refFrameToggleButton.onClick = new Button.ButtonClickedEvent();
        refFrameToggleButton.onClick.AddListener((System.Action)(() => {
            Utils.flipObjectAndBool(refFrameToggleObj, ref EditorManager.Instance.refFrameGlobal);
            
            EditorToolController.Instance.OnDeSelect();
            EditorToolController.Instance.OnSelect();
            }));
        

    }
    void doSaveButton()
    {
        if (BundleManager.saveBarPrefab == null)
        {
            return;
        }

        GameObject saveButtonObj = GameObject.Instantiate(BundleManager.saveBarPrefab, GameObject.Find("Main Button Toggle ").transform);

        Button saveButton = saveButtonObj.GetComponent<Button>();
        saveButton.onClick = new Button.ButtonClickedEvent();
        saveButton.onClick.AddListener((System.Action)(() =>
        {
            MelonLogger.Msg("Saved Clicked");
            EditorManager.SaveEditorAsIs();
        }));
        
    }

    void removeDefaultCameraMovement()
    {
        Camera.main.GetComponent<CameraScroll>().speed = 0f;

        mainButtonToggle.transform.GetChild(0).gameObject.SetActive(false);
        mainButtonToggle.transform.GetChild(1).gameObject.SetActive(false);
    }
    void setupSwitchButton()
    {
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

    // TODO: move this to EditorManager
    void toggleEditor()
    {
        if (EditorManager.Instance.isActive)
        {
            mainButtonHolder.SetActive(true);
            editorButtons1.SetActive(false);
        }
        else
        {
            mainButtonHolder.SetActive(false);
            editorButtons1.SetActive(true);
        }
        EditorManager.Instance.isActive = !EditorManager.Instance.isActive;
        if (EditorManager.Instance.isActive)
        {
            EditorToolController.Instance.OnSelect();
        }
        else
        {
            EditorToolController.Instance.OnDeSelect();
        }
    }
    void setupEditorButtons()
    {
        if (BundleManager.bottomBarPrefab == null)
        {
            throw new NullReferenceException("bottomBarPrefab is null");
        }

        editorButtons1 = GameObject.Instantiate(BundleManager.bottomBarPrefab, GameObject.Find("Main Button Toggle ").transform);
    
        editorButtons1.SetActive(false);


        var bagHolder = editorButtons1.transform.GetChild(0);

        var objects = new[] { "block", "portal" , "blackhole", "", "", "moving platform", "", "", ""};

        for (int index = 0; index < objects.Length; index++)
        {

            string obj_name = objects[index];

            if (obj_name == "")
            {
                continue;
            }

            Transform obj = bagHolder.GetChild(index);

            var button = obj.GetComponent<Button>();

            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener((System.Action)(() =>
            {
                if (EditorToolController.Instance && EditorManager.Instance && !GameManager.Instance.IsStarted)
                {

                    EditorPlacer.Instance.Place(LevelObjectRegistry.Registry[obj_name]);
    
                }
            }));
        }

    }


}