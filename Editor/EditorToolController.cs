using System.ComponentModel.DataAnnotations;
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

    public float moveGridsize = 0.2f;
    public float rotateGridsize = 18f;
    public float scaleGridsize = 0.2f;
    public bool gridActive = true;

    public Vector3 startPos;
    public Vector3 startRot;
    public Vector3 startScale;

    public Vector3 offset;
    public Vector2 startMousePos;

    // TODO: replace with enum
    public int mode = 0;
    public enum ToolDirection
    {
        All = 0,
        Up = 1,
        Right = 2,
    }
    // Move
    public bool movingObject;
    public ToolDirection moveDirection = ToolDirection.All;
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

    // Scale
    public bool scalingObject;
    public ToolDirection scaleDirection = ToolDirection.All;

    public GameObject scaleToolObj;
    public BoxCollider2D scaleToolBoundsCenter;
    public BoxCollider2D scaleToolBoundsUp;
    public BoxCollider2D scaleToolBoundsDown;

    public Transform scaleToolUpHead;
    public Transform scaleToolRightHead;
    public Vector2 startScaleToolScale;
    public Vector2 startScaleToolPosition;


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

        if (BundleManager.scaleToolPrefab != null)
        {
            scaleToolObj = GameObject.Instantiate(BundleManager.scaleToolPrefab);
            scaleToolObj.SetActive(false);

            scaleToolBoundsCenter = scaleToolObj.transform.GetChild(0).GetComponent<BoxCollider2D>();
            scaleToolBoundsUp = scaleToolObj.transform.GetChild(1).GetComponent<BoxCollider2D>();
            scaleToolBoundsDown = scaleToolObj.transform.GetChild(2).GetComponent<BoxCollider2D>();
        
            scaleToolUpHead = scaleToolBoundsUp.transform.GetChild(0);
            scaleToolRightHead = scaleToolBoundsDown.transform.GetChild(0);
        
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
        else if (mode == 3)
        {
            scaleToolObj.SetActive(true);
            scaleToolObj.transform.position = EditorManager.Instance.mainSelectedObject.transform.position;
        }
    }
    public void OnDeSelect()
    {
        moveToolObj.SetActive(false);
        movingObject = false;

        rotateToolObj.SetActive(false);
        rotatingObject = false;

        scaleToolObj.SetActive(false);
        scalingObject = false;
    }
    void Update()
    {
        
        Move();
        Rotate();
        Scale();
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
        startMousePos = Utils.pointerWorldPos();

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
        startMousePos = Utils.pointerWorldPos();

        if (moveToolObj != null)
            startMoveToolPos = (Vector2)moveToolObj.transform.position;

        movingObject = true;
    }
    private void StopMove()
    {
        if (EditorManager.Instance.mainSelectedObject == null) return;

        EditorManager.Instance.mainSelectedObject.OnEditorDrop();
        movingObject = false;

        onDrop?.Invoke();
    }
    void Scale()
    {
        Vector2 mousePos = (Vector2)Utils.pointerWorldPos();

        if (scalingObject && EditorManager.Instance.mainSelectedObject != null)
        {
            Transform mainSelectedObject = EditorManager.Instance.mainSelectedObject.transform;

            Vector2 mousePosDifference = mousePos - startMousePos;

            bool all = scaleDirection == ToolDirection.All;
            if (all)
            {
                mousePosDifference.x = Mathf.Max(mousePosDifference.x, mousePosDifference.y);
                mousePosDifference.y = Mathf.Max(mousePosDifference.x, mousePosDifference.y);
            }

            if (scaleDirection == ToolDirection.Right || all)
            {
                scaleToolBoundsDown.transform.localScale = 
                new Vector3(scaleToolBoundsDown.transform.localScale.x, startScaleToolScale.x + mousePosDifference.x, scaleToolBoundsDown.transform.localScale.z);
            
                scaleToolBoundsDown.transform.localPosition = 
                new Vector3(startScaleToolPosition.x + mousePosDifference.x / 2, scaleToolBoundsDown.transform.localPosition.y, scaleToolBoundsDown.transform.localPosition.z);

                var headScale = scaleToolRightHead.localScale;
                headScale.x = .2f * (startScaleToolScale.x / (startScaleToolScale.x + mousePosDifference.x));
                scaleToolRightHead.localScale = headScale;


                mainSelectedObject.localScale = 
                new Vector3(startScale.x + mousePosDifference.x, mainSelectedObject.localScale.y, mainSelectedObject.localScale.z);
            
            }
            if (scaleDirection == ToolDirection.Up || all)
            {
                scaleToolBoundsUp.transform.localScale = 
                new Vector3(scaleToolBoundsUp.transform.localScale.x, startScaleToolScale.y + mousePosDifference.y, scaleToolBoundsUp.transform.localScale.z);
            
                scaleToolBoundsUp.transform.localPosition = 
                new Vector3(scaleToolBoundsUp.transform.localPosition.x, startScaleToolPosition.y + mousePosDifference.y / 2, scaleToolBoundsUp.transform.localPosition.z);

                var headScale = scaleToolUpHead.localScale;
                headScale.y = .2f * (startScaleToolScale.y / (startScaleToolScale.y + mousePosDifference.y));
                scaleToolUpHead.localScale = headScale;

                mainSelectedObject.localScale = 
                new Vector3(mainSelectedObject.localScale.x, startScale.y + mousePosDifference.y, mainSelectedObject.localScale.z);
            }

        }

        ToolDirection? detectedDragState = null;
        var overlaps = Physics2D.OverlapPointAll(mousePos);
        if (overlaps != null)
        {
            foreach (var col in overlaps)
            {
                if (col == scaleToolBoundsCenter)
                {
                    detectedDragState = ToolDirection.All;
                    break;
                }
                if (col == scaleToolBoundsUp)
                {
                    detectedDragState = ToolDirection.Up;
                    break;
                }
                if (col == scaleToolBoundsDown)
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
                scaleDirection = detectedDragState.Value;

            Vector2 toolPos = (Vector2)scaleToolObj.transform.position;


            StartScale();
        }

        bool up = Input.GetMouseButtonUp(0);
        if (up)
        {
            StopScale();
        }

    }


    public void StartScale()
    {
        if (EditorManager.Instance.mainSelectedObject == null) return;

        ResetScaleTool();


        startPos = EditorManager.Instance.mainSelectedObject.transform.position;
        startRot = EditorManager.Instance.mainSelectedObject.transform.eulerAngles;
        startScale = EditorManager.Instance.mainSelectedObject.transform.localScale;

        startMousePos = Utils.pointerWorldPos();

        startScaleToolScale = new Vector2(scaleToolBoundsDown.transform.localScale.y, scaleToolBoundsUp.transform.localScale.y);
        startScaleToolPosition = new Vector2(scaleToolBoundsDown.transform.localPosition.x, scaleToolBoundsUp.transform.localPosition.y);


        //if (scaleToolObj != null)
            //startScaleToolPos = (Vector2)scaleToolObj.transform.position;

        scalingObject = true;
    }
    private void StopScale()
    {
        if (EditorManager.Instance.mainSelectedObject == null) return;

        ResetScaleTool();

        scalingObject = false;
    }

    void ResetScaleTool()
    {
        scaleToolBoundsUp.transform.localPosition = new Vector3(0f, 1.5f, 0f);
        scaleToolBoundsDown.transform.localPosition = new Vector3(1.5f, 0f, 0f);

        scaleToolBoundsUp.transform.localScale = new Vector3(.2f, 3f, 1f);
        scaleToolBoundsDown.transform.localScale = new Vector3(.2f, 3f, 1f);

        var upHeadScale = scaleToolUpHead.localScale;
        upHeadScale.y = 0.2f;
        scaleToolUpHead.localScale = upHeadScale;

        var rightHeadScale = scaleToolRightHead.localScale;
        rightHeadScale.x = 0.2f;
        scaleToolRightHead.localScale = rightHeadScale;
    }

    public void Cancel()
    {
        if (!movingObject || EditorManager.Instance.mainSelectedObject == null) return;

        //EditorManager.Instance.mainSelectedObject.transform.position = startPos;
        //EditorManager.Instance.mainSelectedObject.transform.eulerAngles = startRot;

        StopMove();
    }

    public bool isEditorToolCollider(Collider2D c)
    {
        return (c == moveToolBoundsCenter && moveToolBoundsCenter.isActiveAndEnabled) ||
            (c == moveToolBoundsUp && moveToolBoundsUp.isActiveAndEnabled) ||
            (c == moveToolBoundsDown && moveToolBoundsDown.isActiveAndEnabled) ||
            (c == rotateToolButton && rotateToolButton.isActiveAndEnabled) ||
            (c == scaleToolBoundsCenter && scaleToolBoundsCenter.isActiveAndEnabled) ||
            (c == scaleToolBoundsUp && scaleToolBoundsUp.isActiveAndEnabled) ||
            (c == scaleToolBoundsDown && scaleToolBoundsDown.isActiveAndEnabled);
    }
}