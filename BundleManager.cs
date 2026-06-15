using UnityEngine;
using System.Collections;
using MelonLoader;
using MelonLoader.TinyJSON;

public static class BundleManager
{
    private static Il2CppAssetBundle? _bundle;
    public static string bundlePath = Path.Combine(LocalData.dataPath, "bundle");


    public static string levelsMenuSceneName
    {
        get
        {
            if (_bundle == null)
            {
                return "Menu";
            }
            return _bundle.GetAllScenePaths()[0];
        }
    }

    public static void init()
    {

        _bundle = Il2CppAssetBundleManager.LoadFromFile(bundlePath);
        if (_bundle == null)
        {
            MelonLogger.Error("Failed to load AssetBundle!");
        }

    }
}