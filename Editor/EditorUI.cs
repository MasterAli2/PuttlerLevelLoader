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
        setupEditorButtons();
        doLeftBar();
    }

    void doLeftBar()
    {
        if (BundleManager.leftBarPrefab == null)
        {
            return;
        }
        GameObject leftBar = GameObject.Instantiate(BundleManager.leftBarPrefab, GameObject.Find("Main Button Toggle ").transform);;
        
        leftBar.transform.GetChild(0).GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        leftBar.transform.GetChild(0).GetComponent<Button>().onClick.AddListener((System.Action)(() => {EditorToolController.Instance.setMode(1);}));
        
        leftBar.transform.GetChild(1).GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        leftBar.transform.GetChild(1).GetComponent<Button>().onClick.AddListener((System.Action)(() => {EditorToolController.Instance.setMode(2);}));
        
        leftBar.transform.GetChild(2).GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        leftBar.transform.GetChild(2).GetComponent<Button>().onClick.AddListener((System.Action)(() => {EditorToolController.Instance.setMode(3);}));

        MelonLogger.Msg(leftBar.transform.GetChild(0).GetComponent<Button>().name);
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
            editorButtons1.SetActive(false);
        }
        else
        {
            mainButtonHolder.SetActive(false);
            editorButtons1.SetActive(true);
        }
        EditorManager.Instance.isActive = !EditorManager.Instance.isActive;
    }
    void setupEditorButtons()
    {

        editorButtons1 = GameObject.Instantiate(mainButtonHolder, mainButtonHolder.transform.parent);
    
        editorButtons1.transform.name = "Editor Buttons";
        editorButtons1.SetActive(false);

        var bagHolder = editorButtons1.transform.GetChild(0).GetChild(2);

        for (int i = 1; i < 7; i++)
        {
            Destroy(bagHolder.transform.GetChild(i).gameObject);
        }

        GameObject baseUIThingFish = bagHolder.transform.GetChild(0).gameObject;
        Destroy(baseUIThingFish.transform.GetChild(1).gameObject);
        //Destroy(baseUIThingFish.transform.GetChild(0).GetComponent<Image>());

        Utils.HideAndDisable(baseUIThingFish);

        foreach (var pair in LevelObjectRegistry.Registry)
        {
            GameObject obj = GameObject.Instantiate(baseUIThingFish, baseUIThingFish.transform.parent);
            obj.hideFlags = HideFlags.None;
            obj.SetActive(true);

            pair.Value.ApplyEditorPlaceButtons(obj);

            var a = obj.GetComponent<Button>();

            a.onClick = new Button.ButtonClickedEvent();
            a.onClick.AddListener((System.Action)(() =>
            {
                if (EditorToolController.Instance && EditorManager.Instance && !GameManager.Instance.IsStarted)
                {
                    if (EditorToolController.Instance.movingObject)
                    {
                        EditorToolController.Instance.Cancel();
                    }

                    GameObject[] objs = LevelObjectRegistry.Registry[pair.Key].PlaceDefault();

                    if (EditorManager.Instance.mainSelectedObject != null)
                        EditorOutline.removeOutline(EditorManager.Instance.mainSelectedObject.gameObject);

                    EditorManager.Instance.mainSelectedObject = objs[0].GetComponent<BaseLevelObject>();
                    EditorOutline.addOutline(EditorManager.Instance.mainSelectedObject.gameObject);
                    EditorToolController.Instance.Pickup();

                    System.Action onDropHandler = null;
                    onDropHandler = () =>
                    {
                        foreach (GameObject obj in objs)
                        {
                            obj.transform.position = EditorManager.Instance.mainSelectedObject.transform.position;
                            obj.transform.localEulerAngles = EditorManager.Instance.mainSelectedObject.transform.localEulerAngles;
                            obj.transform.localScale = EditorManager.Instance.mainSelectedObject.transform.localScale;
                        }

                        // unsubscribe after handling
                        EditorToolController.Instance.onDrop -= onDropHandler;
                    };

                    EditorToolController.Instance.onDrop += onDropHandler;
                }
            }));

        }


    }
}