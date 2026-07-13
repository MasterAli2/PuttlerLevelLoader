using Harmony;
using MelonLoader;
using MelonLoader.TinyJSON;
using UnityEngine;
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
            if (mainSelectedObject != null)
            {
                UnSelect();    
            }
            Select();
        }
    }

    void Select()
    {
        Vector3 mousePos = Utils.pointerWorldPos();
        mousePos.z = Mathf.Abs(Camera.current.transform.position.z);

        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);
        foreach (Collider2D collider in hits)
        {
            if (!collider.transform.root.TryGetComponent<BaseLevelObject>(out var obj)) 
            {
                continue;
            }

            obj.OnEditorSelectMain();

            mainSelectedObject = obj;
            EditorOutline.addOutline(mainSelectedObject.gameObject);
            break;
        }
    }

    private void UnSelect()
    {
        EditorOutline.removeOutline(mainSelectedObject.gameObject);
        mainSelectedObject.OnEditorUnSelectMain();
        mainSelectedObject = null;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}