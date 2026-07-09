using System;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[RegisterLevelObject("moving platform")]
[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaMovingPlatformLevelObject : BaseLevelObject, IBaseLevelObject
{
    public static GameObject prefab;

    public VanillaMovingPlatformLevelObject(IntPtr ptr) : base(ptr) {}

    public static GameObject Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = placeMovingPlatform(
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

    public static GameObject placeMovingPlatform(Vector3 start, Vector3 end, float rotation)
    {
        GameObject obj = Utils.spawnPrefab(prefab);

        obj.transform.position = start;
        obj.transform.eulerAngles = new Vector3(0f, 0f, rotation);


        obj.transform.GetChild(3).position = end;

        return obj;
    }
}
