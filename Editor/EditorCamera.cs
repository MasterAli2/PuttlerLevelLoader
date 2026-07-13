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
        cameraScroll.leftDistance = 24f;
        cameraScroll.rightDistance = 24f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        
        if (Input.GetMouseButton(1))
        {
            Vector2 current = cam.ScreenToWorldPoint(Input.mousePosition);

            Vector2 difference = dragOrigin - current;

            Vector3 newPosition = cam.transform.position + (Vector3)difference;
            newPosition.y = Mathf.Clamp(newPosition.y, -12f, 12f);
            cam.transform.position = newPosition;
            cameraScroll.targetPosition = cam.transform.position.x;
        }
    }
    
}
