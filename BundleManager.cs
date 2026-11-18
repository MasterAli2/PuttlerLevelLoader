using UnityEngine;
using System.Collections;
using MelonLoader;
using MelonLoader.TinyJSON;

public static class BundleManager
{
    private static Il2CppAssetBundle? _legacy_bundle;
    private static Il2CppAssetBundle? _bundle;
    public static string legacyBundlePath = Path.Combine(LocalData.dataPath, "bundle");
    public static string bundlePath = Path.Combine(LocalData.dataPath, "pll_main");


    public static string levelsMenuSceneName
    {
        get
        {
            if (_legacy_bundle == null)
            {
                return "Menu";
            }
            return _legacy_bundle.GetAllScenePaths()[0];
        }
    }


    public static GameObject? leftBarPrefab
    {
        get
        {
            return getGameobjectFromMainBundleOrNull("LeftBar");
        }
    }
    public static GameObject? bottomBarPrefab
    {
        get
        {
            return getGameobjectFromMainBundleOrNull("bottombar");
        }
    }
    public static GameObject? saveBarPrefab
    {
        get
        {
            return getGameobjectFromMainBundleOrNull("savebar");
        }
    }
    public static GameObject? topBarPrefab
    {
        get
        {
            return getGameobjectFromMainBundleOrNull("TopBar");
        }
    }
    public static GameObject? moveToolPrefab
    {
        get
        {
            return getGameobjectFromMainBundleOrNull("MoveTool");
        }
    }
    public static GameObject? rotateToolPrefab
    {
        get
        {
            return getGameobjectFromMainBundleOrNull("RotateTool");
        }
    }
    public static GameObject? scaleToolPrefab
    {
        get
        {
            return getGameobjectFromMainBundleOrNull("ScaleTool");
        }
    }

    public static GameObject? testPrefab
    {
        get
        {
            return getGameobjectFromMainBundleOrNull("test_prefab");
        }
    }

    public static AnimationClip? ExtraMenu1Anim
    {
        get
        {
            return getAnimFromMainBundleOrNull("UI/ExtraMenu1");
        }
    }
    public static AnimationClip? ExtraMenu2Anim
    {
        get
        {
            return getAnimFromMainBundleOrNull("UI/ExtraMenu2");
        }
    }
    public static AnimationClip? ExtraMenuGoEditor
    {
        get
        {
            return getAnimFromMainBundleOrNull("UI/ExtraAnims/GoEditor");
        }
    }
    public static AnimationClip? ExtraMenuEditorGoMain
    {
        get
        {
            return getAnimFromMainBundleOrNull("UI/ExtraAnims/EditorGoMain");
        }
    }
    public static AnimationClip? ExtraMenuLocalGoEdit
    {
        get
        {
            return getAnimFromMainBundleOrNull("UI/ExtraAnims/LocalGoEdit");
        }
    }
    public static AnimationClip? ExtraMenuEditGoLocal
    {
        get
        {
            return getAnimFromMainBundleOrNull("UI/ExtraAnims/EditGoLocal");
        }
    }


    public static GameObject? ExtrasMenuPrefab
    {
        get
        {
            return getGameobjectFromMainBundleOrNull("UI/Extras Menu");
        }
    }

    public static Sprite? ExtraButtonSprite
    {
        get
        {
            return getSpriteFromMainBundleOrNull("UI/extra_sign");
        }
    }
    public static Sprite? ExitButtonSprite
    {
        get
        {
            return getSpriteFromMainBundleOrNull("Sprites/Exit Icon");
        }
    }

    private static GameObject? getGameobjectFromMainBundleOrNull(string name)
    {
        return getAssetFromMainBundleOrNull<GameObject>(name + ".prefab");
    }
    private static AnimationClip? getAnimFromMainBundleOrNull(string name)
    {
        return getAssetFromMainBundleOrNull<AnimationClip>(name + ".anim");
    }
    private static Sprite? getSpriteFromMainBundleOrNull(string name)
    {
        return getAssetFromMainBundleOrNull<Sprite>(name + ".png");
    }
    private static T? getAssetFromMainBundleOrNull<T>(string name) where T: UnityEngine.Object
    {
        if (_bundle == null)
        {
            return null;
        }
        return _bundle.LoadAsset<T>("assets/" + name);
    }

    

    public static void init()
    {

        _legacy_bundle = Il2CppAssetBundleManager.LoadFromFile(legacyBundlePath);
        if (_legacy_bundle == null)
        {
            MelonLogger.Error("Failed to load legacy bundle!");
        }

        _bundle = Il2CppAssetBundleManager.LoadFromFile(bundlePath);
        if (_bundle == null)
        {
            MelonLogger.Error("Failed to load main bundle!");
        }

    }
}