using Il2Cpp;
using MelonLoader;
using UnityEngine;

[MelonLoader.RegisterTypeInIl2Cpp]
class EditorCamera : MonoBehaviour
{
    public EditorCamera(IntPtr ptr) : base(ptr) {}
    public Vector2 mousePositionLastFrame;

    public Vector2 dragOrigin;

    public CameraScroll cameraScroll;


    public float zoomSensitivity = 0.25f;
    public float minZoom = 2.5f;
    public float maxZoom = 10f;

    public readonly float maxHorizontalDistance = 24f;
    public readonly float maxVerticalDistance = 24f;

    public Camera cam
    {
        get
        {
            return Camera.main;
        }
    }

    void Awake()
    {
        cameraScroll = cam.GetComponent<CameraScroll>();
        cameraScroll.leftDistance = maxHorizontalDistance;
        cameraScroll.rightDistance = maxHorizontalDistance;
    }

    void Update()
    {
        float scroll = -Input.mouseScrollDelta.y;
        if (scroll != 0 && EditorToolController.Instance && !EditorToolController.Instance.movingObject)
        {
            var a = cam.ScreenToWorldPoint(Input.mousePosition);

            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + scroll*zoomSensitivity, minZoom, maxZoom);

            var b = cam.ScreenToWorldPoint(Input.mousePosition);

            MelonLogger.Msg(a==b);

            Vector2 difference = a - b;
            Vector3 newPosition = cam.transform.position + (Vector3)difference;
            newPosition.y = Mathf.Clamp(newPosition.y, -maxVerticalDistance, maxVerticalDistance);
            cam.transform.position = newPosition;
            cameraScroll.targetPosition = cam.transform.position.x;
        }


        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        
        if (Input.GetMouseButton(1))
        {
            Vector2 current = cam.ScreenToWorldPoint(Input.mousePosition);

            Vector2 difference = dragOrigin - current;

            Vector3 newPosition = cam.transform.position + (Vector3)difference;
            newPosition.y = Mathf.Clamp(newPosition.y, -maxVerticalDistance, maxVerticalDistance);
            cam.transform.position = newPosition;
            cameraScroll.targetPosition = cam.transform.position.x;
        }
    }
    
}
