using MelonLoader;
using UnityEngine;

class EditorOutline : MonoBehaviour
{
    const float outlineWidth = 0.05f;

    public List<SpriteRenderer> outlineRenderers = new List<SpriteRenderer>();

    public Transform target;
    public static EditorOutline Instance;

    public EditorOutline(IntPtr ptr) : base(ptr) {}

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    void Refresh()
    {
        Clear();

        outlineRenderers = new List<SpriteRenderer>();

        SpriteRenderer[] spriteRenderers = target.GetComponentsInChildren<SpriteRenderer>().ToArray();

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            GameObject outline = new GameObject("Outline");

            SpriteRenderer newSpriteRenderer = outline.AddComponent<SpriteRenderer>();
            outline.transform.SetParent(spriteRenderer.transform, false);

            outline.transform.localPosition = Vector3.zero;
            outline.transform.localScale = Vector3.one;
            outline.transform.localEulerAngles = Vector3.zero;

            newSpriteRenderer.sprite = spriteRenderer.sprite;
            newSpriteRenderer.color = Color.yellow;
            newSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder-1;

            ApplyOutlineScale(outline.transform);

            outlineRenderers.Add(newSpriteRenderer);
        }
    }

    void FixedUpdate()
    {
        if (this == null)
        {
            return;
        }

        foreach (SpriteRenderer spriteRenderer in outlineRenderers)
        {

            // bandaid fix
            if (spriteRenderer == null)
            {
                continue;
            }

            ApplyOutlineScale(spriteRenderer.transform);
        }
    }

    void ApplyOutlineScale(Transform outlineTransform)
    {
        var parentScale = outlineTransform.parent.localScale;
        var scale = Vector3.one;

        scale.x += outlineWidth * 2f / parentScale.x;
        scale.y += outlineWidth * 2f / parentScale.y;
        outlineTransform.localScale = scale;
    }

    void Clear(){
        foreach (SpriteRenderer spriteRenderer in outlineRenderers){
            if (spriteRenderer == null) 
                continue;

            Destroy(spriteRenderer.gameObject);
        }
    }

    public static void addOutline(GameObject gameObject)
    {
        if (gameObject == null)
        {
            return;
        }

        clearOutline();

        EditorOutline.Instance.target = gameObject.transform;
        EditorOutline.Instance.Refresh();
    }

    public static void clearOutline()
    {
        EditorOutline.Instance.Clear();


    }
}