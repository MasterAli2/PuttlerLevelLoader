using System;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


[RegisterLevelObject("block")]
[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaSurfaceBlockLevelObject : BaseLevelObject, IBaseLevelObject
{
    public static GameObject prefab;

    public VanillaSurfaceBlockLevelObject(IntPtr ptr) : base(ptr) {}

    public static GameObject Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = Utils.spawnPrefabWithValues(prefab,
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