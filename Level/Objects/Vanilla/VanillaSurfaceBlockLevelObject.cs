using System;
using System.Text.Json;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class VanillaSurfaceBlockLevelObject : BaseLevelObject
{
    public static GameObject prefab;
    
    public VanillaSurfaceBlockLevelObject(IntPtr ptr) : base(ptr) {}

    public override SerialLevelObject SerializeObject()
    {
        SerialLevelObject serialLevelObject = new SerialLevelObject("block");

        serialLevelObject.data["pos"] = Vec3D.toJson(transform.position);
        serialLevelObject.data["size"] = Vec3D.toJson(transform.localScale);
        serialLevelObject.data["rot"] = JsonSerializer.SerializeToElement(transform.eulerAngles.z);

        return serialLevelObject;
    }
}