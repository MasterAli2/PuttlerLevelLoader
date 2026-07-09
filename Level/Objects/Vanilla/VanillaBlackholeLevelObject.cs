using System;
using UnityEngine;

[RegisterLevelObject("blackhole")]
[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaBlackholeLevelObject : MonoBehaviour, IBaseLevelObject
{
    public static GameObject prefab;

    public VanillaBlackholeLevelObject(IntPtr ptr) : base(ptr) {}

    public static GameObject Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = LevelPlacer.placeBlackhole(Vec3D.fromJson(serialLevelObject.data["pos"]));
        obj.AddComponent<VanillaBlackholeLevelObject>();
        return obj;
    }

    public static void CleanScene()
    {
        prefab = GameObject.Find("Blackhole");
        Utils.HideAndDisable(prefab);
    }
}
