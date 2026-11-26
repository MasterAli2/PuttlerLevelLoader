using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

class EditorToolController : MonoBehaviour
{
    public static EditorToolController Instance {get; private set;}

    public EditorToolController(IntPtr ptr) : base(ptr) {}

    // temp fix
    public event Action onDrop;

    public float moveGridsize = 0.1f;
    public float rotateGridsize = 9f;
    public float scaleGridsize = 0.1f;
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
    public BoxCollider2D moveToolBoundsRight;

    public Vector2 moveObjOffset = Vector2.zero;
    public Vector2 moveToolOffset = Vector2.zero;
    public Vector2 startMoveToolPos = Vector2.zero;

    public GameObject moveRightLineObj;
    public GameObject moveUpLineObj;

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
    public BoxCollider2D scaleToolBoundsRight;

    public Transform scaleToolUpHead;
    public Transform scaleToolRightHead;
    public Vector2 startScaleToolScale;
    public Vector2 startScaleToolPosition;

    // Rect

    public bool rectingObject;

    public GameObject rectToolObj;

    public CircleCollider2D rectToolBallTR;
    public CircleCollider2D rectToolBallTL;
    public CircleCollider2D rectToolBallBR;
    public CircleCollider2D rectToolBallBL;

    public BoxCollider2D rectToolSideRight;
    public BoxCollider2D rectToolSideLeft;
    public BoxCollider2D rectToolSideTop;
    public BoxCollider2D rectToolSideBottom;


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
            moveToolBoundsRight = moveToolObj.transform.GetChild(2).GetComponent<BoxCollider2D>();

            moveUpLineObj = moveToolBoundsUp.transform.GetChild(1).gameObject;
            moveRightLineObj = moveToolBoundsRight.transform.GetChild(1).gameObject;
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
            scaleToolBoundsRight = scaleToolObj.transform.GetChild(2).GetComponent<BoxCollider2D>();
        
            scaleToolUpHead = scaleToolBoundsUp.transform.GetChild(0);
            scaleToolRightHead = scaleToolBoundsRight.transform.GetChild(0);
        
        }
        if (BundleManager.rectToolPrefab != null)
        {
            rectToolObj = GameObject.Instantiate(BundleManager.rectToolPrefab);
            rectToolObj.SetActive(false);        

            Transform ballsHolder = rectToolObj.transform.GetChild(0).GetChild(0);
            Transform sidesHolder = rectToolObj.transform.GetChild(0).GetChild(1);

            rectToolBallTR = ballsHolder.GetChild(0).GetComponent<CircleCollider2D>();
            rectToolBallTL = ballsHolder.GetChild(1).GetComponent<CircleCollider2D>();
            rectToolBallBR = ballsHolder.GetChild(2).GetComponent<CircleCollider2D>();
            rectToolBallBL = ballsHolder.GetChild(3).GetComponent<CircleCollider2D>();

            rectToolSideRight = sidesHolder.GetChild(0).GetComponent<BoxCollider2D>();
            rectToolSideLeft = sidesHolder.GetChild(1).GetComponent<BoxCollider2D>();
            rectToolSideTop = sidesHolder.GetChild(2).GetComponent<BoxCollider2D>();
            rectToolSideBottom = sidesHolder.GetChild(3).GetComponent<BoxCollider2D>();
        }
    }
    public void OnSelect()
    {
        if (!EditorManager.Instance.isActive ||EditorManager.Instance.mainSelectedObject == null)
            return;

        BaseSelectableObject.Abilities abilities = EditorManager.Instance.mainSelectedObject.GetAbilities();

        if ((mode == 1 && (abilities & BaseSelectableObject.Abilities.canMove) == 0) ||
            (mode == 2 && (abilities & BaseSelectableObject.Abilities.canRotate) == 0) ||
            (mode == 3 && (abilities & BaseSelectableObject.Abilities.canScale) == 0) ||
            (mode == 4 && (abilities & BaseSelectableObject.Abilities.canRect) == 0))
        {
            return;
        }

        if (mode == 1)
        {
            moveToolObj.SetActive(true);
            moveToolObj.transform.position = EditorManager.Instance.mainSelectedObject.transform.position;

            if (!EditorManager.Instance.refFrameGlobal)
            {
                moveToolObj.transform.rotation = EditorManager.Instance.mainSelectedObject.transform.rotation;
            }
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
            scaleToolObj.transform.rotation = EditorManager.Instance.mainSelectedObject.transform.rotation;
        }
        else if (mode == 4)
        {
            rectToolObj.SetActive(true);
            rectToolObj.transform.position = EditorManager.Instance.mainSelectedObject.transform.position;
            rectToolObj.transform.rotation = EditorManager.Instance.mainSelectedObject.transform.rotation;

            FitRectToolToBasic(EditorManager.Instance.mainSelectedObject.transform);
        }
    }
    public void OnDeSelect()
    {
        moveToolObj.SetActive(false);
        moveToolObj.transform.rotation = Quaternion.identity;
        movingObject = false;

        rotateToolObj.SetActive(false);
        rotateToolObj.transform.rotation = Quaternion.identity;
        rotatingObject = false;

        scaleToolObj.SetActive(false);
        scaleToolObj.transform.rotation = Quaternion.identity;
        scalingObject = false;

        rectToolObj.SetActive(false);
        rectingObject = false;
    }
    void Update()
    {
        
        Move();
        Rotate();
        Scale();

        Rect();

        Trash();
    }
    void Rect()
    {
        
    }

    void Trash()
    {
        bool down = Input.GetKeyDown(KeyCode.T);
        if (down && EditorManager.Instance.mainSelectedObject != null)
        {
            if (LevelManager.currentLevelObjs.Contains(EditorManager.Instance.mainSelectedObject.baseLevelObject))
            {
                LevelManager.currentLevelObjs.Remove(EditorManager.Instance.mainSelectedObject.baseLevelObject);
            }
            EditorManager.Instance.mainSelectedObject.Trash();
        }
    }

    void Rotate()
    {
        Vector3 mousePos = (Vector2)Utils.pointerWorldPos();

        if (rotatingObject && EditorManager.Instance.mainSelectedObject != null)
        {

            //rotateToolPivot.LookAt(mousePos);

            
            Vector2 direction = mousePos - rotateToolPivot.position;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (EditorManager.Instance.snapToGrid)
                angle = Utils.snapToGridValue(angle, rotateGridsize);
            
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

        EditorManager.Instance.mainSelectedObject.OnEditorUpdate();

    }
    private void StopRotate()
    {
        if (EditorManager.Instance.mainSelectedObject == null) return;

        rotatingObject = false;

        EditorManager.Instance.mainSelectedObject.OnEditorUpdate();
    }

    void Move()
    {
        Vector2 mousePos = (Vector2)Utils.pointerWorldPos();

        if (movingObject && EditorManager.Instance.mainSelectedObject != null)
        {   
            // had copilot rewrite this part for local movement cuz im too stupid to understand the math
            Vector3 toolPos;
            Vector3 desiredObjPos;

            if (moveDirection == ToolDirection.All)
            {
                toolPos = (Vector3)(mousePos + moveToolOffset);
                desiredObjPos = toolPos + (Vector3)moveObjOffset;
            }
            else
            {
                Vector3 axis = moveDirection == ToolDirection.Right
                    ? moveToolBoundsRight.transform.up
                    : moveToolBoundsUp.transform.up;
                axis.Normalize();

                Vector3 mouseOffset = mousePos - startMousePos;
                float distance = Vector3.Dot(mouseOffset, axis);

                Vector3 movement = distance * axis;
                toolPos = (Vector3)startMoveToolPos + movement;
                desiredObjPos = startPos + movement;
            }

            // Apply grid
            if (EditorManager.Instance.snapToGrid)
            {
                toolPos = Utils.snapToGridVector(toolPos, moveGridsize);
                desiredObjPos = Utils.snapToGridVector(desiredObjPos, moveGridsize);
            }

            moveToolObj.transform.position = toolPos;
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
                if (col == moveToolBoundsRight)
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
        moveRightLineObj.SetActive(true);
        moveUpLineObj.SetActive(true);

        if (moveDirection == ToolDirection.Up)
        {
            moveRightLineObj.SetActive(false);   
        }
        else if (moveDirection == ToolDirection.Right)
        {
            moveUpLineObj.SetActive(false);
        }

        if (EditorManager.Instance.mainSelectedObject == null) return;

        startPos = EditorManager.Instance.mainSelectedObject.transform.position;
        startRot = EditorManager.Instance.mainSelectedObject.transform.eulerAngles;
        startMousePos = Utils.pointerWorldPos();

        if (moveToolObj != null)
            startMoveToolPos = (Vector2)moveToolObj.transform.position;

        movingObject = true;

        EditorManager.Instance.mainSelectedObject.OnEditorUpdate();

    }
    private void StopMove()
    {
        moveRightLineObj.SetActive(false);
        moveUpLineObj.SetActive(false);

        if (EditorManager.Instance.mainSelectedObject == null) return;

        EditorManager.Instance.mainSelectedObject.OnEditorDrop();
        movingObject = false;

        onDrop?.Invoke();

        EditorManager.Instance.mainSelectedObject.OnEditorUpdate();

    }
    void Scale()
    {
        Vector2 mousePos = (Vector2)Utils.pointerWorldPos();

        if (scalingObject && EditorManager.Instance.mainSelectedObject != null)
        {
            Transform mainSelectedObject = EditorManager.Instance.mainSelectedObject.transform;

            Vector2 mousePosDifference = mousePos - startMousePos;
            if (EditorManager.Instance.snapToGrid)
                mousePosDifference = Utils.snapToGridVector(mousePosDifference, scaleGridsize);

            bool all = scaleDirection == ToolDirection.All;
            if (all)
            {
                mousePosDifference.x = Mathf.Max(mousePosDifference.x, mousePosDifference.y);
                mousePosDifference.y = Mathf.Max(mousePosDifference.x, mousePosDifference.y);
            }

            if (scaleDirection == ToolDirection.Right || all)
            {
                scaleToolBoundsRight.transform.localScale = 
                new Vector3(scaleToolBoundsRight.transform.localScale.x, startScaleToolScale.x + mousePosDifference.x, scaleToolBoundsRight.transform.localScale.z);
            
                scaleToolBoundsRight.transform.localPosition = 
                new Vector3(startScaleToolPosition.x + mousePosDifference.x / 2, scaleToolBoundsRight.transform.localPosition.y, scaleToolBoundsRight.transform.localPosition.z);

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
                if (col == scaleToolBoundsRight)
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

        startScaleToolScale = new Vector2(scaleToolBoundsRight.transform.localScale.y, scaleToolBoundsUp.transform.localScale.y);
        startScaleToolPosition = new Vector2(scaleToolBoundsRight.transform.localPosition.x, scaleToolBoundsUp.transform.localPosition.y);


        //if (scaleToolObj != null)
            //startScaleToolPos = (Vector2)scaleToolObj.transform.position;

        scalingObject = true;

        EditorManager.Instance.mainSelectedObject.OnEditorUpdate();

    }
    private void StopScale()
    {
        if (EditorManager.Instance.mainSelectedObject == null) return;

        ResetScaleTool();

        scalingObject = false;

        EditorManager.Instance.mainSelectedObject.OnEditorUpdate();

    }

    void ResetScaleTool()
    {
        scaleToolBoundsUp.transform.localPosition = new Vector3(0f, 1.5f, 0f);
        scaleToolBoundsRight.transform.localPosition = new Vector3(1.5f, 0f, 0f);

        scaleToolBoundsUp.transform.localScale = new Vector3(.2f, 3f, 1f);
        scaleToolBoundsRight.transform.localScale = new Vector3(.2f, 3f, 1f);

        var upHeadScale = scaleToolUpHead.localScale;
        upHeadScale.y = 0.2f;
        scaleToolUpHead.localScale = upHeadScale;

        var rightHeadScale = scaleToolRightHead.localScale;
        rightHeadScale.x = 0.2f;
        scaleToolRightHead.localScale = rightHeadScale;
    }

    void FitRectToolToBasic(Transform target)
    {
        rectToolObj.transform.localScale = target.localScale*0.55f;

        Transform balls = rectToolObj.transform.GetChild(0).GetChild(0);
        Transform sides = rectToolObj.transform.GetChild(0).GetChild(1);
        for (int i = 0; i < balls.childCount; i++)
        {
            Transform ball = balls.GetChild(i);
            ball.localScale = new Vector3(
                0.35f/rectToolObj.transform.localScale.x,
                0.35f/rectToolObj.transform.localScale.y,
                ball.localScale.z
            );
        }

        Transform leftSide = sides.GetChild(0);
        var leftSideScale = leftSide.localScale;
        leftSideScale.x = 0.05f / rectToolObj.transform.localScale.x;
        leftSide.localScale = leftSideScale;

        Transform rightSide = sides.GetChild(1);
        var rightSideScale = rightSide.localScale;
        rightSideScale.x = 0.05f / rectToolObj.transform.localScale.x;
        rightSide.localScale = rightSideScale;

        Transform bottomSide = sides.GetChild(2);
        var bottomSideScale = bottomSide.localScale;
        bottomSideScale.x = 0.05f / rectToolObj.transform.localScale.y;
        bottomSide.localScale = bottomSideScale;

        Transform topSide = sides.GetChild(3);
        var topSideScale = topSide.localScale;
        topSideScale.x = 0.05f / rectToolObj.transform.localScale.y;
        topSide.localScale = topSideScale;

    }

    public void Cancel()
    {
        if (!movingObject || EditorManager.Instance.mainSelectedObject == null) return;

        StopMove();
        StopRotate();
        StopScale();
    }

    public bool isEditorToolCollider(Collider2D c)
    {
        return (c == moveToolBoundsCenter && moveToolBoundsCenter.isActiveAndEnabled) ||
            (c == moveToolBoundsUp && moveToolBoundsUp.isActiveAndEnabled) ||
            (c == moveToolBoundsRight && moveToolBoundsRight.isActiveAndEnabled) ||
            (c == rotateToolButton && rotateToolButton.isActiveAndEnabled) ||
            (c == scaleToolBoundsCenter && scaleToolBoundsCenter.isActiveAndEnabled) ||
            (c == scaleToolBoundsUp && scaleToolBoundsUp.isActiveAndEnabled) ||
            (c == scaleToolBoundsRight && scaleToolBoundsRight.isActiveAndEnabled) ||

            (c == rectToolBallTR && rectToolBallTR.isActiveAndEnabled) ||
            (c == rectToolBallTL && rectToolBallTL.isActiveAndEnabled) ||
            (c == rectToolBallBR && rectToolBallBR.isActiveAndEnabled) ||
            (c == rectToolBallBL && rectToolBallBL.isActiveAndEnabled) ||

            (c == rectToolSideRight && rectToolSideRight.isActiveAndEnabled) ||
            (c == rectToolSideLeft && rectToolSideLeft.isActiveAndEnabled) ||
            (c == rectToolSideTop && rectToolSideTop.isActiveAndEnabled) ||
            (c == rectToolSideBottom && rectToolSideBottom.isActiveAndEnabled);
    }
}