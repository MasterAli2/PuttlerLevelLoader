using System;
using System.Drawing;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

[MelonLoader.RegisterTypeInIl2Cpp]
class EditorToolController : MonoBehaviour
{
    public static EditorToolController Instance {get; private set;}

    public EditorToolController(IntPtr ptr) : base(ptr) {}

    public event Action onDrop;

    public bool movingObject = false;
    public Vector3 startPos;
    public Vector3 startRot;

    public Vector3 offset;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        GameManager.Instance.OnGameStart.AddListener((System.Action)(() =>
        {
            Cancel();
        }));
    }
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
        if (down && EditorManager.Instance.isActive && !GameManager.Instance.IsStarted)
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

    public void Pickup()
    {
        if (EditorManager.Instance.mainSelectedObject == null) return;

        startPos = EditorManager.Instance.mainSelectedObject.transform.position;
        startRot = EditorManager.Instance.mainSelectedObject.transform.eulerAngles;

        movingObject = true;
    }
    void Drop()
    {
        if (EditorManager.Instance.mainSelectedObject == null) return;

        EditorManager.Instance.mainSelectedObject.OnEditorDrop();
        movingObject = false;

        onDrop?.Invoke();
    }

    public void Cancel()
    {
        if (!movingObject || EditorManager.Instance.mainSelectedObject == null) return;

        EditorManager.Instance.mainSelectedObject.transform.position = startPos;
        EditorManager.Instance.mainSelectedObject.transform.eulerAngles = startRot;

        Drop();
    }
}