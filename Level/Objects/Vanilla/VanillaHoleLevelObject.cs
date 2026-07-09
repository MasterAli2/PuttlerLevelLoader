using System;
using UnityEngine;

[RegisterLevelObject("hole")]
[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaHoleLevelObject : MonoBehaviour, IBaseLevelObject
{
    public static GameObject prefab;

    public VanillaHoleLevelObject(IntPtr ptr) : base(ptr) {}

    public static GameObject Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = LevelPlacer.placeLevelHole(Vec3D.fromJson(serialLevelObject.data["pos"]), serialLevelObject.data["rot"].GetSingle(), Vec3D.fromJson(serialLevelObject.data["size"]));
        obj.AddComponent<VanillaHoleLevelObject>();
        return obj;
        
    }

    public static void CleanScene()
    {
        prefab = GameObject.Find("Level Hole");
        Utils.HideAndDisable(prefab);
    }
}