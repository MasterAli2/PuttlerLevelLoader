using System;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class VanillaBlackholeLevelObject : BaseLevelObject
{
    public static GameObject prefab;

    public VanillaBlackholeLevelObject(IntPtr ptr) : base(ptr) {}

    public override BaseSelectableObject.Abilities GetAbilities()
    {
        return BaseSelectableObject.Abilities.canMove;
    }

    public override SerialLevelObject SerializeObject()
    {
        SerialLevelObject serialLevelObject = new SerialLevelObject("blackhole");

        serialLevelObject.data["pos"] = Vec3D.toJson(transform.position);

        return serialLevelObject;
    }
}
