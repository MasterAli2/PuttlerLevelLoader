using System;
using UnityEngine;

[RegisterLevelObject("portal")]
[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaPortalLevelObject : MonoBehaviour, IBaseLevelObject
{
    public static GameObject prefab;

    public VanillaPortalLevelObject(IntPtr ptr) : base(ptr) {}

    public static GameObject Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = LevelPlacer.placePortal(
            Vec3D.fromJson(serialLevelObject.data["entry"]),
            Vec3D.fromJson(serialLevelObject.data["exit"]));
        obj.AddComponent<VanillaPortalLevelObject>();
        return obj;
    }

    public static void CleanScene()
    {
        prefab = GameObject.Find("Portal");
        Utils.HideAndDisable(prefab);
    }
}
