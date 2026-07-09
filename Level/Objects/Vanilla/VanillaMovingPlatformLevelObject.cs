using System;
using UnityEngine;

[RegisterLevelObject("moving platform")]
[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaMovingPlatformLevelObject : MonoBehaviour, IBaseLevelObject
{
    public static GameObject prefab;

    public VanillaMovingPlatformLevelObject(IntPtr ptr) : base(ptr) {}

    public static GameObject Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = LevelPlacer.placeMovingPlatform(
            Vec3D.fromJson(serialLevelObject.data["start"]),
            Vec3D.fromJson(serialLevelObject.data["end"]),
            serialLevelObject.data["rot"].GetSingle());
        obj.AddComponent<VanillaMovingPlatformLevelObject>();
        return obj;
    }

    public static void CleanScene()
    {
        prefab = GameObject.Find("Moving Platform");
        Utils.HideAndDisable(prefab);
    }
}
