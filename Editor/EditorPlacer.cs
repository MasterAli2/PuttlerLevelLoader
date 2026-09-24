using Harmony;
using MelonLoader;
using MelonLoader.TinyJSON;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class EditorPlacer : MonoBehaviour
{
    public static EditorPlacer Instance {get; private set;}
    public EditorPlacer(IntPtr ptr) : base(ptr) {}


    public bool isPlacing = false;
    public BaseLevelObject mainPlacingObject;
    public BaseSelectableObject firstPlacingObjects;
    public List<BaseSelectableObject> placingObjects;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Place(LevelObjectDefinition levelObjectDefinition)
    {
        if (EditorManager.Instance != null)
        {
            EditorManager.Instance.UnSelect();
        }

        mainPlacingObject = levelObjectDefinition.PlaceDefault();
        placingObjects = mainPlacingObject.GetSelectables().ToList();
        if (placingObjects.Count == 0)
        {
            return;
        }
        firstPlacingObjects = placingObjects[0];

        isPlacing = true;
        pressed_down =false;

        mainPlacingObject.OnEditorUpdate();
    }

    bool pressed_down = false;

    void Update()
    {
        if (!isPlacing)
            return;

        Vector2 mousePos = Utils.pointerWorldPos();
        
        placingObjects[0].transform.position = Utils.snapToGridVector(mousePos, EditorToolController.Instance.moveGridsize);

        if (!pressed_down)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pressed_down = true;
            }
        }
        if (!pressed_down) 
            return;
            
        bool up = Input.GetMouseButtonUp(0);
        if (up)
        {

            placingObjects[0].transform.position = Utils.snapToGridVector(mousePos, EditorToolController.Instance.moveGridsize);
            placingObjects[0].OnEditorUpdate();

            placingObjects.RemoveAt(0);

            
            if (placingObjects.Count == 0)
            {
                isPlacing = false;
                EditorManager.Instance.SelectObj(firstPlacingObjects);
                
            }


            EditorManager.Instance.selectCoolDown = 0.1f;

            pressed_down = false;

        }
    }
}