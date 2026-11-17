using Harmony;
using Il2Cpp;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

static class MainMenu
{
    public const float EXTRA_MENU_Y_LEVEL = -2140f;

    static Transform gameButtons;
    static StartAnimation mainStartAnimation;

    static GameObject extras_menu_obj;

    static int chosenEditLevelIndex = -1;
    public static void onSceneLoaded(string sceneName)
    {
        if (sceneName != "Menu") return;

        Canvas canvas = GameObject.Find("Menu Canvas").GetComponent<Canvas>();
        gameButtons = canvas.transform.Find("Menu Components (Default Off)").Find("Game Buttons");

        setupExtraButton();
        setupExtrasMenu();
        setupLocalMenu();
        setupEditMenu();

        doEditorExit();
    }
    static void doEditorExit()
    {
        if (EditorUI.exitedEditor)
        {
            EditorUI.exitedEditor = false;

            Utils.jumpToEndOfAnim(mainStartAnimation._animation, BundleManager.ExtraMenuGoEditor.name);
        }
    }
    static void setupExtrasMenu()
    {
        GameObject extras_menu_prefab = BundleManager.ExtrasMenuPrefab;
        extras_menu_obj = GameObject.Instantiate(extras_menu_prefab, gameButtons);

        RectTransform extras_menu_rect = extras_menu_obj.GetComponent<RectTransform>();
        Vector2 vector2 = extras_menu_rect.anchoredPosition;
        vector2.y = EXTRA_MENU_Y_LEVEL;
        extras_menu_rect.anchoredPosition = vector2;

        AnimationClip extra_menu2_anim = BundleManager.ExtraMenu2Anim;
        mainStartAnimation._animation.AddClip(extra_menu2_anim, extra_menu2_anim.name);

        GameObject mainMenu = extras_menu_obj.transform.GetChild(0).gameObject;

        GameObject home_button = mainMenu.transform.Find("Home Button (PLL_extra_home_button)").gameObject;

        Utils.expandOnHoverObj(home_button, 1.05f, 5f);

        StartAnimation startAnimation = home_button.AddComponent<StartAnimation>();
        startAnimation._animation = mainStartAnimation._animation;
        startAnimation.animationName = extra_menu2_anim.name;

        Button homeButton = home_button.GetComponent<Button>();
        homeButton.interactable = true;
        homeButton.onClick = new Button.ButtonClickedEvent();
        homeButton.onClick.AddListener((System.Action)(() => {Utils.TryPlayAnim(startAnimation);}));

        Transform mainGrid = mainMenu.transform.Find("Main grid (PLL)");

        GameObject editorButton = mainGrid.Find("Editor Button (PLL)").gameObject;

        Utils.expandOnHoverObj(editorButton, 1.05f, 5f);

        GameObject levelButton = mainGrid.Find("Levels Button (PLL)").gameObject;

        Utils.expandOnHoverObj(levelButton, 1.05f, 5f);

        GameObject onlineButton = mainGrid.Find("Online Button (PLL)").gameObject;

        Utils.expandOnHoverObj(onlineButton, 1.05f, 5f);


        //Animation animation = extras_menu_obj.GetComponent<Animation>();

        mainStartAnimation._animation.AddClip(BundleManager.ExtraMenuGoEditor, BundleManager.ExtraMenuGoEditor.name);

        StartAnimation startAnimation1 = editorButton.AddComponent<StartAnimation>();
        startAnimation1.animationName = BundleManager.ExtraMenuGoEditor.name;
        startAnimation1._animation = mainStartAnimation._animation;

        Button _editorButton = editorButton.GetComponent<Button>();
        _editorButton.interactable = true;
        _editorButton.onClick = new Button.ButtonClickedEvent();
        _editorButton.onClick.AddListener((System.Action)(() => {
            Utils.TryPlayAnim(startAnimation1);
        }));
        
    }
    public static void setupEditMenu()
    {
        Transform root = extras_menu_obj.transform.GetChild(2).GetChild(2).GetChild(0);

        

        Button backButton = extras_menu_obj.transform.GetChild(2).GetChild(1).GetComponent<Button>();

        mainStartAnimation._animation.AddClip(BundleManager.ExtraMenuEditGoLocal, BundleManager.ExtraMenuEditGoLocal.name);
        StartAnimation startAnimation = backButton.gameObject.AddComponent<StartAnimation>();
        startAnimation.animationName = BundleManager.ExtraMenuEditGoLocal.name;
        startAnimation._animation = mainStartAnimation._animation;

        backButton.onClick = new Button.ButtonClickedEvent();
        backButton.onClick.AddListener((System.Action)(() => {
            Utils.TryPlayAnim(startAnimation);
        }));
        
        Button openEditorButton = root.GetChild(3).GetComponent<Button>();
        openEditorButton.onClick = new Button.ButtonClickedEvent();
        openEditorButton.onClick.AddListener((System.Action)(() => {
            // open current level as editor here
            LevelManager.openCustomLevel(chosenEditLevelIndex, true);
        }));

        Button saveCurrentMetaStuffButton = root.GetChild(4).GetComponent<Button>();
        saveCurrentMetaStuffButton.onClick = new Button.ButtonClickedEvent();
        saveCurrentMetaStuffButton.onClick.AddListener((System.Action)(() => {
            // save stuff here idk
            MelonLogger.Msg("Save Stuff");
        }));


    }
    public static void setupLocalMenu()
    {

        GameObject back_button = extras_menu_obj.transform.GetChild(1).Find("Back Button (PLL)").gameObject;

        mainStartAnimation._animation.AddClip(BundleManager.ExtraMenuEditorGoMain, BundleManager.ExtraMenuEditorGoMain.name);

        Utils.expandOnHoverObj(back_button, 1.05f, 5f);

        StartAnimation startAnimation = back_button.AddComponent<StartAnimation>();
        startAnimation.animationName = BundleManager.ExtraMenuEditorGoMain.name;
        startAnimation._animation = mainStartAnimation._animation;

        Button _editorButton = back_button.GetComponent<Button>();
        _editorButton.interactable = true;
        _editorButton.onClick = new Button.ButtonClickedEvent();
        _editorButton.onClick.AddListener((System.Action)(() => {
            Utils.TryPlayAnim(startAnimation);
        }));



        
        refreshLocalMenu();

    }

    public static void refreshLocalMenu()
    {
        LocalData.init();


        GameObject localLevelsContentRoot = extras_menu_obj.transform.GetChild(1).GetChild(4).GetChild(0).GetChild(0).GetChild(0).gameObject;

        GameObject baseLLButton = localLevelsContentRoot.transform.GetChild(0).gameObject;

        for (int i = localLevelsContentRoot.transform.childCount; i > 1; i--)
        {
            GameObject.Destroy(localLevelsContentRoot.transform.GetChild(i - 1).gameObject);
        }

        baseLLButton.SetActive(false);
        if (LocalData.customLevels == null)
        {
            baseLLButton.SetActive(true);
            baseLLButton.GetComponentInChildren<Text>().text = "An error occured while loading the levels";
            return;
        }

        mainStartAnimation._animation.AddClip(BundleManager.ExtraMenuLocalGoEdit, BundleManager.ExtraMenuLocalGoEdit.name);


        for (int i = 0; i < LocalData.customLevels.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(baseLLButton, baseLLButton.transform.parent);
            obj.SetActive(true);

            Utils.expandOnHoverObj(obj, 1.005f, 0.05f);

            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = LocalData.customLevels[i].name;

            int index = i; // important fix

            obj.transform.GetChild(0).GetComponent<Button>().onClick.AddListener((System.Action)(() =>
            {
                LevelManager.openCustomLevel(index);
            }));

            StartAnimation startAnimation1 = obj.transform.GetChild(3).gameObject.AddComponent<StartAnimation>();
            startAnimation1.animationName = BundleManager.ExtraMenuLocalGoEdit.name;
            startAnimation1._animation = mainStartAnimation._animation;

            obj.transform.GetChild(3).GetComponent<Button>().onClick.AddListener((System.Action)(() =>
            {
                chosenEditLevelIndex = index;
                Utils.TryPlayAnim(startAnimation1);
            }));
        }
    }

    public static void setupExtraButton()
    {
        Canvas canvas = GameObject.Find("Menu Canvas").GetComponent<Canvas>();
        Transform gameButtons = canvas.transform.Find("Menu Components (Default Off)").Find("Game Buttons");
        GameObject baseGameButton = gameButtons.Find("Bag Button").gameObject;

        GameObject extrasButtonObj = GameObject.Instantiate(baseGameButton, baseGameButton.transform.parent);
        extrasButtonObj.transform.localPosition = new Vector3(760, 1500, -16200);

        //AnimationMenu animationMenu = GameObject.Find("Animation").GetComponent<AnimationMenu>();


        AnimationClip enterAnim = BundleManager.ExtraMenu1Anim;        
        mainStartAnimation = extrasButtonObj.GetComponent<StartAnimation>();
        mainStartAnimation._animation.AddClip(enterAnim, enterAnim.name);
        mainStartAnimation.animationName = enterAnim.name;

        
        Button onlineButton = extrasButtonObj.GetComponent<Button>();
        onlineButton.interactable = true;
        onlineButton.onClick = new Button.ButtonClickedEvent();
        onlineButton.onClick.AddListener((System.Action)(() => {
            //TODO: cache the component
            if (baseGameButton.GetComponent<Button>().interactable)
                Utils.TryPlayAnim(mainStartAnimation);
            }));

        extrasButtonObj.transform.GetChild(3).GetComponent<Image>().sprite = BundleManager.ExtraButtonSprite;
        extrasButtonObj.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Extra Stuff";
    }
    public static void onClickOnlineButton()
    {
        SceneManager.LoadScene(BundleManager.levelsMenuSceneName);
    }
}