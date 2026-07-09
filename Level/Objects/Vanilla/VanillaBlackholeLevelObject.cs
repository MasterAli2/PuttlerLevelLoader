using System;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[RegisterLevelObject("blackhole")]
[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaBlackholeLevelObject : MonoBehaviour, IBaseLevelObject
{
    public static GameObject prefab;

    public VanillaBlackholeLevelObject(IntPtr ptr) : base(ptr) {}

    public static GameObject Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = Utils.spawnPrefabWithValues(prefab, Vec3D.fromJson(serialLevelObject.data["pos"]));
        obj.AddComponent<VanillaBlackholeLevelObject>();
        return obj;
    }

    public static void CleanScene()
    {
        prefab = GameObject.Find("Blackhole");
        Utils.HideAndDisable(prefab);
    }
}
