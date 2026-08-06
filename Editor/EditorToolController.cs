using System.Drawing;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

[MelonLoader.RegisterTypeInIl2Cpp]
class EditorToolController : MonoBehaviour
{
    public static EditorToolController Instance {get; private set;}

    public EditorToolController(IntPtr ptr) : base(ptr) {}

    // temp fix
    public event Action onDrop;

    public Vector3 startPos;
    public Vector3 startRot;

    public Vector3 offset;

    // TODO: replace with enum
    public int mode = 0;

    // Move
    public bool draggingObject;
    public DragState dragState = DragState.All;
    public enum DragState
    {
        All = 0,
        Up = 1,
        Right = 2,
    }

    public GameObject moveToolObj;
    public BoxCollider2D moveToolBoundsCenter;
    public BoxCollider2D moveToolBoundsUp;
    public BoxCollider2D moveToolBoundsDown;

    public Vector2 moveObjOffset = Vector2.zero;
    public Vector2 moveToolOffset = Vector2.zero;
    public Vector2 startMoveToolPos = Vector2.zero;

    // Rotate
    public bool rotatingObj = false;
    public GameObject rotateToolObj;
    public CircleCollider2D rotateToolButton;
    public Transform rotateToolPivot;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void setMode(int mode)
    {
        this.mode = mode;

        if (mode != 1)
        {
            moveToolObj.SetActive(false);
        }
        if (mode == 1)
        {
            OnSelect();
        }
    }

    void Start()
    {
        GameManager.Instance.OnGameStart.AddListener((System.Action)(() =>
        {
            Cancel();
        }));

        if (BundleManager.moveToolPrefab != null)
        {
            moveToolObj = GameObject.Instantiate(BundleManager.moveToolPrefab);
            moveToolObj.SetActive(false);

            moveToolBoundsCenter = moveToolObj.transform.GetChild(0).GetComponent<BoxCollider2D>();
            moveToolBoundsUp = moveToolObj.transform.GetChild(1).GetComponent<BoxCollider2D>();
            moveToolBoundsDown = moveToolObj.transform.GetChild(2).GetComponent<BoxCollider2D>();
        }

        if (BundleManager.rotateToolPrefab != null)
        {
            rotateToolObj = GameObject.Instantiate(BundleManager.rotateToolPrefab);
            rotateToolObj.SetActive(false);

            rotateToolPivot = rotateToolObj.transform.GetChild(0);    
            rotateToolButton = rotateToolObj.transform.GetChild(0).GetChild(0).GetComponent<CircleCollider2D>();    
        }


    }
    public void OnSelect()
    {
        if (EditorManager.Instance.mainSelectedObject == null)
            return;

        if (mode == 1)
        {
            moveToolObj.SetActive(true);
            moveToolObj.transform.position = EditorManager.Instance.mainSelectedObject.transform.position;
        }
        else if (mode == 2)
        {
            rotateToolObj.SetActive(true);
            rotateToolObj.transform.position = EditorManager.Instance.mainSelectedObject.transform.position;
            rotateToolPivot.rotation = EditorManager.Instance.mainSelectedObject.transform.rotation;
        }
    }
    public void OnDeSelect()
    {
        moveToolObj.SetActive(false);
        draggingObject = false;

        rotateToolObj.SetActive(false);
        rotatingObj = false;
    }
    void Update()
    {
        
        Move();
        Rotate();
    }

    void Rotate()
    {
        Vector3 mousePos = (Vector2)Utils.pointerWorldPos();

        if (rotatingObj && EditorManager.Instance.mainSelectedObject != null)
        {

            //rotateToolPivot.LookAt(mousePos);

            
            Vector2 direction = mousePos - rotateToolPivot.position;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            rotateToolPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            EditorManager.Instance.mainSelectedObject.transform.rotation = rotateToolPivot.rotation;
        }

        bool isOverlapping = Physics2D.OverlapPointAll(mousePos).Contains(rotateToolButton);

        bool down = Input.GetMouseButtonDown(0);

        if (down && isOverlapping && EditorManager.Instance.isActive && !GameManager.Instance.IsStarted)
        {
            StartRotate();
        }

        bool up = Input.GetMouseButtonUp(0);
        if (up)
        {
            StopRotate();
        }

    }
    private void StartRotate()
    {
        if (EditorManager.Instance.mainSelectedObject == null) return;

        startRot = EditorManager.Instance.mainSelectedObject.transform.eulerAngles;

        rotatingObj = true;
    }
    private void StopRotate()
    {
        if (EditorManager.Instance.mainSelectedObject == null) return;

        rotatingObj = false;
    }

    void Move()
    {
        Vector2 mousePos = (Vector2)Utils.pointerWorldPos();

        if (draggingObject && EditorManager.Instance.mainSelectedObject != null)
        {
            // compute tool position from mouse and offsets, then constrain by dragState
            Vector2 toolPos = mousePos + moveToolOffset;

            if (dragState == DragState.Up)
            {
                toolPos.x = startMoveToolPos.x;
            }
            else if (dragState == DragState.Right)
            {
                toolPos.y = startMoveToolPos.y;
            }

            moveToolObj.transform.position = toolPos;

            Vector3 desiredObjPos = (Vector3)(toolPos + moveObjOffset);

            if (dragState == DragState.Up)
            {
                desiredObjPos.x = startPos.x;
            }
            else if (dragState == DragState.Right)
            {
                desiredObjPos.y = startPos.y;
            }

            EditorManager.Instance.mainSelectedObject.transform.position = desiredObjPos;
        }

        DragState? detectedDragState = null;
        var overlaps = Physics2D.OverlapPointAll(mousePos);
        if (overlaps != null)
        {
            foreach (var col in overlaps)
            {
                if (col == moveToolBoundsCenter)
                {
                    detectedDragState = DragState.All;
                    break;
                }
                if (col == moveToolBoundsUp)
                {
                    detectedDragState = DragState.Up;
                    break;
                }
                if (col == moveToolBoundsDown)
                {
                    detectedDragState = DragState.Right;
                    break;
                }
            }
        }

        bool isOverlapping = detectedDragState != null;

        bool down = Input.GetMouseButtonDown(0);

        if (down && isOverlapping && EditorManager.Instance.isActive && !GameManager.Instance.IsStarted)
        {
            if (detectedDragState != null)
                dragState = detectedDragState.Value;

            Vector2 toolPos = (Vector2)moveToolObj.transform.position;

            moveObjOffset = (Vector2)EditorManager.Instance.mainSelectedObject.transform.position - toolPos;
            moveToolOffset = toolPos - mousePos;
            StartMove();
        }

        bool up = Input.GetMouseButtonUp(0);
        if (up)
        {
            StopMove();
        }

    }

    public void StartMove()
    {
        if (EditorManager.Instance.mainSelectedObject == null) return;

        startPos = EditorManager.Instance.mainSelectedObject.transform.position;
        startRot = EditorManager.Instance.mainSelectedObject.transform.eulerAngles;

        if (moveToolObj != null)
            startMoveToolPos = (Vector2)moveToolObj.transform.position;

        draggingObject = true;
    }
    private void StopMove()
    {
        if (EditorManager.Instance.mainSelectedObject == null) return;

        EditorManager.Instance.mainSelectedObject.OnEditorDrop();
        draggingObject = false;

        onDrop?.Invoke();
    }

    public void Cancel()
    {
        if (!draggingObject || EditorManager.Instance.mainSelectedObject == null) return;

        //EditorManager.Instance.mainSelectedObject.transform.position = startPos;
        //EditorManager.Instance.mainSelectedObject.transform.eulerAngles = startRot;

        StopMove();
    }

    public bool isEditorToolCollider(Collider2D c)
    {
        return (c == moveToolBoundsCenter && moveToolBoundsCenter.isActiveAndEnabled) ||
            (c == moveToolBoundsUp && moveToolBoundsUp.isActiveAndEnabled) ||
            (c == moveToolBoundsDown && moveToolBoundsDown.isActiveAndEnabled) ||
            (c == rotateToolButton && rotateToolButton.isActiveAndEnabled);
    }
}