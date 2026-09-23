using System;
using MelonLoader;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


public class BaseSelectableObject : MonoBehaviour
{
    public BaseSelectableObject(IntPtr ptr) : base(ptr) {}

    public BaseLevelObject baseLevelObject;

    public const string TAG = "Selectable Level Object";

    public void Awake()
    {
        gameObject.tag = TAG;

        baseLevelObject = GetComponentInParent<BaseLevelObject>();

        baseLevelObject.baseSelectableObjects.Add(this);
    }

    public virtual void Trash() {
        OnEditorTrash();

        baseLevelObject.Trash();
    }



    public virtual void OnEditorTrash()
    { 
        if (EditorManager.Instance.mainSelectedObject == this)
        {
            EditorManager.Instance.UnSelect();
        }

        baseLevelObject.OnEditorTrash(); 
    }
    public virtual void OnEditorPickup(){ baseLevelObject.OnEditorPickup(); }
    public virtual void OnEditorDrop(){ baseLevelObject.OnEditorDrop(); }
    public virtual void OnEditorSelectMain(){ baseLevelObject.OnEditorSelectMain(); }
    public virtual void OnEditorUnSelectMain(){ baseLevelObject.OnEditorUnSelectMain(); }
    public virtual void OnEditorUpdated(){ /*baseLevelObject.OnEditorUpdated()*/; }
    
}

