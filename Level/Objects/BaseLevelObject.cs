using System;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


public class BaseLevelObject : MonoBehaviour
{
    public BaseLevelObject(IntPtr ptr) : base(ptr) {}

    public List<BaseSelectableObject> baseSelectableObjects = new List<BaseSelectableObject>();

    public void Awake()
    {
        gameObject.tag = "Level Object";
    }

    public virtual BaseSelectableObject[] GetSelectables()
    {
        return baseSelectableObjects.ToArray();
    }
    public virtual void OnDestroy(){}

    public virtual void Trash()
    {
        Destroy(gameObject);
    }

    public virtual void OnEditorPickup(){}
    public virtual void OnEditorTrash(){}
    public virtual void OnEditorDrop(){}
    public virtual void OnEditorSelectMain(){}
    public virtual void OnEditorUnSelectMain(){}
    public virtual void OnEditorUpdate(){}
    
}