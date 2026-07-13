using MelonLoader;
using UnityEngine;

[MelonLoader.RegisterTypeInIl2Cpp]
class EditorOutline : MonoBehaviour
{
    const float outlineWidth = 0.05f;

    public List<SpriteRenderer> outlineRenderers = new List<SpriteRenderer>();

    public EditorOutline(IntPtr ptr) : base(ptr) {}
    
    void Awake()
    {
        SpriteRenderer[] spriteRenderers = transform.parent.GetComponentsInChildren<SpriteRenderer>().ToArray();

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

            var scale = outline.transform.localScale;
            var parentScale = outline.transform.parent.localScale;

            scale.x += outlineWidth * 2f / parentScale.x;
            scale.y += outlineWidth * 2f / parentScale.y;
            outline.transform.localScale = scale;

            outlineRenderers.Add(newSpriteRenderer);
        }
    }

    void OnDestroy(){
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

        removeOutline(gameObject);

        GameObject outline = new GameObject("Editor Outline");
        outline.transform.SetParent(gameObject.transform, false);
        outline.AddComponent<EditorOutline>();
    }

    public static void removeOutline(GameObject gameObject)
    {
        if (gameObject == null)
        {
            return;
        }

        EditorOutline[] outlines = gameObject.GetComponentsInChildren<EditorOutline>().ToArray();
        foreach (EditorOutline outline in outlines)
        {
            if (outline == null)
            {
                continue;
            }

            foreach (SpriteRenderer spriteRenderer in outline.outlineRenderers)
            {
                if (spriteRenderer != null)
                {
                    Destroy(spriteRenderer.gameObject);
                }
            }

            if (outline.gameObject != null)
            {
                Destroy(outline.gameObject);
            }
        }
    }
}