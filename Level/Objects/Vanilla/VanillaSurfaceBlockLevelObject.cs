using System;
using System.Text.Json;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaSurfaceBlockLevelObject : BaseLevelObject
{
    public static GameObject prefab;
    
    public VanillaSurfaceBlockLevelObject(IntPtr ptr) : base(ptr) {}

    public override void OnEditorPickup()
    {
        // nothing
    }
    public override void OnEditorDrop()
    {
        // nothing
    }
}