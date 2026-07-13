using System.Drawing;
using MelonLoader;
using UnityEngine;

[MelonLoader.RegisterTypeInIl2Cpp]
class EditorToolController : MonoBehaviour
{
    public EditorToolController(IntPtr ptr) : base(ptr) {}

    public bool movingObject = false;

    public Vector3 offset;

    void Update()
    {
        
        Move();
        Rotate();
    }
    void Rotate()
    {
        if (!movingObject || EditorManager.Instance.mainSelectedObject == null) return;

        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) >= 1f)
        {
            EditorManager.Instance.mainSelectedObject.transform.eulerAngles += new Vector3(0f, 0f, 15 * Mathf.Sign(scroll));
        }
    }

    void Move()
    {
        if (movingObject && EditorManager.Instance.mainSelectedObject != null)
        {
            EditorManager.Instance.mainSelectedObject.transform.position = Utils.pointerWorldPos() + offset;
        }

        bool down = Input.GetKeyDown(KeyCode.M);
        if (down && EditorManager.Instance.isActive)
        {
            if (!movingObject)
            {
                Pickup();
            }
            else 
            {
                Drop();
            }
        }
    }

    void Pickup()
    {
        movingObject = EditorManager.Instance.mainSelectedObject ? true : false;
    }
    void Drop()
    {
        if (EditorManager.Instance.mainSelectedObject == null) return;

        EditorManager.Instance.mainSelectedObject.OnEditorDrop();
        movingObject = false;
    }
}