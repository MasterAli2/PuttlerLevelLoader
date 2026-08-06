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
    public bool movingObject;
    public ToolDirection moveDirection = ToolDirection.All;
    public enum ToolDirection
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
    public bool rotatingObject = false;
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

        OnDeSelect();
        OnSelect();
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
        if (!EditorManager.Instance.isActive ||EditorManager.Instance.mainSelectedObject == null)
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
        movingObject = false;

        rotateToolObj.SetActive(false);
        rotatingObject = false;
    }
    void Update()
    {
        
        Move();
        Rotate();
    }

    void Rotate()
    {
        Vector3 mousePos = (Vector2)Utils.pointerWorldPos();

        if (rotatingObject && EditorManager.Instance.mainSelectedObject != null)
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

        rotatingObject = true;
    }
    private void StopRotate()
    {
        if (EditorManager.Instance.mainSelectedObject == null) return;

        rotatingObject = false;
    }

    void Move()
    {
        Vector2 mousePos = (Vector2)Utils.pointerWorldPos();

        if (movingObject && EditorManager.Instance.mainSelectedObject != null)
        {
            // compute tool position from mouse and offsets, then constrain by dragState
            Vector2 toolPos = mousePos + moveToolOffset;

            if (moveDirection == ToolDirection.Up)
            {
                toolPos.x = startMoveToolPos.x;
            }
            else if (moveDirection == ToolDirection.Right)
            {
                toolPos.y = startMoveToolPos.y;
            }

            moveToolObj.transform.position = toolPos;

            Vector3 desiredObjPos = (Vector3)(toolPos + moveObjOffset);

            if (moveDirection == ToolDirection.Up)
            {
                desiredObjPos.x = startPos.x;
            }
            else if (moveDirection == ToolDirection.Right)
            {
                desiredObjPos.y = startPos.y;
            }

            EditorManager.Instance.mainSelectedObject.transform.position = desiredObjPos;
        }

        ToolDirection? detectedDragState = null;
        var overlaps = Physics2D.OverlapPointAll(mousePos);
        if (overlaps != null)
        {
            foreach (var col in overlaps)
            {
                if (col == moveToolBoundsCenter)
                {
                    detectedDragState = ToolDirection.All;
                    break;
                }
                if (col == moveToolBoundsUp)
                {
                    detectedDragState = ToolDirection.Up;
                    break;
                }
                if (col == moveToolBoundsDown)
                {
                    detectedDragState = ToolDirection.Right;
                    break;
                }
            }
        }

        bool isOverlapping = detectedDragState != null;

        bool down = Input.GetMouseButtonDown(0);

        if (down && isOverlapping && EditorManager.Instance.isActive && !GameManager.Instance.IsStarted)
        {
            if (detectedDragState != null)
                moveDirection = detectedDragState.Value;

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

        movingObject = true;
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