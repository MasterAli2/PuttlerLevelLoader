using System;
using MelonLoader;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


public class BaseSelectableObject : MonoBehaviour
{
    [Flags]
    public enum Abilities
    {
        None = 0,
        canMove = 1 << 0,
        canRotate = 1 << 1,
        canScale = 1 << 2,
        canRect = 1 << 3,


        // wheter or not editor tools such as move, rotate, scale and rect should be handled 
        // by the normal logic or it should passed to the object to handle
        // not currently implemented 
        relayEditorTools = 1 << 4,

        
        All = canMove | canRotate | canScale | canRect | relayEditorTools,
        Default = canMove | canRotate | canScale | canRect 
    }
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
    public virtual void OnEditorUpdate(){ baseLevelObject.OnEditorUpdate(); }

    public virtual BaseSelectableObject.Abilities GetAbilities() { return baseLevelObject.GetAbilities(); }
    
}

