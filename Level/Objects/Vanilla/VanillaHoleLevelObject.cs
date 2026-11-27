using System;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class VanillaHoleLevelObject : BaseLevelObject
{
    public static GameObject prefab;
    public static Sprite sprite;

    public VanillaHoleLevelObject(IntPtr ptr) : base(ptr) {}



    public override BaseSelectableObject.Abilities GetAbilities()
    {
        return BaseSelectableObject.Abilities.All;
    }
}