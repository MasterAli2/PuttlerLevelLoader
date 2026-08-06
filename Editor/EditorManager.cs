using Harmony;
using MelonLoader;
using MelonLoader.TinyJSON;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;


// NOT TODO: disable barrier toggle when editor is active to allow placing OOB
/*

anyways first we add a side bar thing that collapses to the side/outof view on the right
then we add a tool thing like how in unity you can choose beetween move rotate scale etc
were just gonna copy unity as much as possible
also select multiple object with left drag
right drag for camera move
unselected scroll for camera zoom

*/
[MelonLoader.RegisterTypeInIl2Cpp]
public class EditorManager : MonoBehaviour
{
    public static EditorManager Instance {get; private set;}

    public bool inEditor = false;
    public bool isActive = false;


    // Called main to allow selecting multiple objects in the future
    public BaseLevelObject? mainSelectedObject;

    public EditorManager(IntPtr ptr) : base(ptr) {}

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        gameObject.AddComponent<EditorUI>();
        gameObject.AddComponent<EditorToolController>();
        gameObject.AddComponent<EditorCamera>();

        

    }
    
    void Update()
    {
        bool down = Input.GetMouseButtonDown(0);
        if (down && EditorManager.Instance.isActive)
        {

            Select();
        }
    }

    void Select()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 mousePos = Utils.pointerWorldPos();
        mousePos.z = Mathf.Abs(Camera.current.transform.position.z);

        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);
        var tool = EditorToolController.Instance;
        if (tool != null && hits.Any(tool.isEditorToolCollider))
            return;
        
        bool flag = false;
        foreach (Collider2D collider in hits)
        {
            if (!collider.transform.root.TryGetComponent<BaseLevelObject>(out var obj)) 
            {
                continue;
            }
            if (mainSelectedObject == obj)
            {
                flag = true;
                continue;
            }

            UnSelect();    
            

            obj.OnEditorSelectMain();

            mainSelectedObject = obj;
            EditorOutline.addOutline(mainSelectedObject.gameObject);

            EditorToolController.Instance.OnSelect();
            return;
        }
        if (!flag)
            UnSelect();   
    }

    private void UnSelect()
    {
        if (mainSelectedObject == null)
            return;
        EditorOutline.removeOutline(mainSelectedObject.gameObject);
        mainSelectedObject.OnEditorUnSelectMain();
        mainSelectedObject = null;

        EditorToolController.Instance.OnDeSelect();
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}