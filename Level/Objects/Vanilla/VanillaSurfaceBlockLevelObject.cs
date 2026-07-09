using System;
using UnityEngine;

[RegisterLevelObject("block")]
[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaSurfaceBlockLevelObject : MonoBehaviour, IBaseLevelObject
{
    public static GameObject prefab;

    public VanillaSurfaceBlockLevelObject(IntPtr ptr) : base(ptr) {}

    public static GameObject Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = LevelPlacer.spawnPrefabWithValues(prefab,
         Vec3D.fromJson(serialLevelObject.data["pos"]),
          Vec3D.fromJson(serialLevelObject.data["size"]),
           serialLevelObject.data["rot"].GetSingle());
           
        obj.AddComponent<VanillaSurfaceBlockLevelObject>();
        return obj;
        
    }

    public static void CleanScene()
    {
        Transform rootHoleComps = GameObject.Find("Hole Components").transform;
        prefab = rootHoleComps.Find("Surface Block (2)").gameObject;

        Utils.HideAndDisable(prefab);
    }
}