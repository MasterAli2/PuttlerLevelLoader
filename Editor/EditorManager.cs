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
public class EditorManager : MonoBehaviour
{
    public static EditorManager Instance {get; private set;}

    public bool inEditor = false;
    public bool isActive = false;

    public float selectCoolDown = 0f;


    // Called main to allow selecting multiple objects in the future
    public BaseSelectableObject? mainSelectedObject;

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
        gameObject.AddComponent<EditorPlacer>();
        gameObject.AddComponent<EditorOutline>();

        var a = GameObject.Instantiate(BundleManager.testPrefab);
    }
    
    void Update()
    {
        selectCoolDown = Mathf.Max(selectCoolDown - Time.deltaTime, -1f);

        bool down = Input.GetMouseButtonDown(0);
        if (down && EditorManager.Instance.isActive && selectCoolDown <= 0f && !EditorPlacer.Instance.isPlacing)
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
            var obj = collider.GetComponentInParent<BaseSelectableObject>();
            if (obj == null) 
            {
                continue;
            }
            if (mainSelectedObject == obj)
            {
                flag = true;
                continue;
            }

            UnSelect();    
            
            SelectObj(obj);

            obj.OnEditorUpdate();
            
            return;
        }
        if (!flag)
            UnSelect();   
    }

    public void SelectObj(BaseSelectableObject levelObject)
    {
        levelObject.OnEditorSelectMain();

        mainSelectedObject = levelObject;
        EditorOutline.addOutline(mainSelectedObject.gameObject);

        EditorToolController.Instance.OnSelect();
    }

    public void UnSelect()
    {
        if (mainSelectedObject == null)
            return;
        EditorOutline.clearOutline();
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