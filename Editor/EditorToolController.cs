using System.Drawing;
using MelonLoader;
using UnityEngine;

[MelonLoader.RegisterTypeInIl2Cpp]
class EditorToolController : MonoBehaviour
{
    public EditorToolController(IntPtr ptr) : base(ptr) {}

    public GameObject? activeHeldGameObject;

    public Vector3 offset;

    void Update()
    {
        if (activeHeldGameObject != null)
        {
            activeHeldGameObject.transform.position = pointerWorldPos() + offset;
        }

        bool down = Input.GetMouseButtonDown(0);
        if (down && EditorUI.inEditor)
        {
            MelonLogger.Msg("Down");
            if (activeHeldGameObject == null)
            {
                Pickup();
            MelonLogger.Msg("pickup");
                
            }
            else {
            MelonLogger.Msg("drop");

                Drop();
            }
        }
    }

    void Pickup()
    {
        Vector3 mousePos = Utils.pointerWorldPos();
        mousePos.z = Mathf.Abs(Camera.current.transform.position.z);


        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);
        Collider2D? hit = null;
        foreach (Collider2D collider in hits)
        {
            MelonLogger.Msg($"name: {collider.gameObject.name}");

            if (!collider.transform.root.TryGetComponent<BaseLevelObject>(out var ut)) {
                MelonLogger.Msg($"nope");

                return;
            }
            ut.OnEditorPickup();
            hit = collider;
        }

        if (hit == null) {
            MelonLogger.Msg("null");

            return;
        }

        activeHeldGameObject = hit.transform.root.gameObject;
        offset = activeHeldGameObject.transform.position - mousePos;
        offset.z = 0;

    }
    void Drop()
    {
        activeHeldGameObject.transform.root.GetComponent<BaseLevelObject>().OnEditorDrop();
        activeHeldGameObject = null;
    }


    
}