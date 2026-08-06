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
            if (_bundle == null)
            {
                return null;
            }
            return _bundle.LoadAsset<GameObject>("Assets/LeftBar.prefab");
        }
    }
    public static GameObject? moveToolPrefab
    {
        get
        {
            if (_bundle == null)
            {
                return null;
            }
            return _bundle.LoadAsset<GameObject>("Assets/MoveTool.prefab");
        }
    }
    public static GameObject? rotateToolPrefab
    {
        get
        {
            if (_bundle == null)
            {
                return null;
            }
            return _bundle.LoadAsset<GameObject>("Assets/RotateTool.prefab");
        }
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