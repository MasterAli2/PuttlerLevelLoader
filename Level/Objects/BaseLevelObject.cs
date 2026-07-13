using System;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


[MelonLoader.RegisterTypeInIl2Cpp]
public class BaseLevelObject : MonoBehaviour
{
    public BaseLevelObject(IntPtr ptr) : base(ptr) {}

    public void Awake()
    {
        gameObject.tag = "Level Object";
    }

    public virtual void OnDestroy(){}



    public virtual void OnEditorPickup(){}
    public virtual void OnEditorDrop(){}
    public virtual void OnEditorSelectMain(){}
    public virtual void OnEditorUnSelectMain(){}
    
}