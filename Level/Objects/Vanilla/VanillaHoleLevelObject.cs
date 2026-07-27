using System;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaHoleLevelObject : BaseLevelObject
{
    public static GameObject prefab;
    public static Sprite sprite;

    public VanillaHoleLevelObject(IntPtr ptr) : base(ptr) {}

    // this is kept here cuz of how complicated it is, i have no idea how and why this works


    public override void OnEditorPickup()
    {
        // nothing
    }
    public override void OnEditorDrop()
    {
        // nothing
    }
}