using System;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[RegisterLevelObject("hole")]
public class VanillaHoleLevelObjectDefinition : LevelObjectDefinition
{
    public static GameObject prefab;
    public static Sprite sprite;

    public override GameObject[] Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = placeLevelHole(Vec3D.fromJson(serialLevelObject.data["pos"]), serialLevelObject.data["rot"].GetSingle(), Vec3D.fromJson(serialLevelObject.data["size"]));
        
        obj.AddComponent<VanillaHoleLevelObject>();
        return new GameObject[] { obj };
        
    }

    public override void ApplyEditorPlaceButtons(GameObject gameObject)
    {
        // nothing
    }

    public override void CleanScene()
    {
        if (sprite == null)
        {
            Texture2D tex = Texture2D.whiteTexture;
            sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), tex.width);
        }

        prefab = GameObject.Find("Level Hole");
        Utils.HideAndDisable(prefab);
    }

    // this is kept here cuz of how complicated it is, i have no idea how and why this works
    public static GameObject placeLevelHole(Vector3 position, float rotation, Vector3 size)
    {
        GameObject obj = Utils.spawnPrefab(prefab);

        obj.transform.position = position;
        obj.transform.eulerAngles = new Vector3(0f, 0f, rotation);

        Transform leftSide = obj.transform.GetChild(4);
        Transform rightSide = obj.transform.GetChild(5);
        Transform middleSide = obj.transform.GetChild(6);
        float diffA = leftSide.localPosition.y - middleSide.localPosition.y;

        float rightSize, leftSize, vertSize;
        rightSize = size.x;
        leftSize = size.y;
        vertSize = size.z;

        SpriteRenderer leftSpriteRenderer = leftSide.GetComponent<SpriteRenderer>();
        if (leftSpriteRenderer != null)
        {
            leftSpriteRenderer.sprite = sprite;
            leftSpriteRenderer.color = Color.black;
        }
        SpriteRenderer rightSpriteRenderer = rightSide.GetComponent<SpriteRenderer>();
        if (rightSpriteRenderer != null)
        {
            rightSpriteRenderer.sprite = sprite;
            rightSpriteRenderer.color = Color.black;
        }
        SpriteRenderer middleSpriteRenderer = middleSide.GetComponent<SpriteRenderer>();
        if (middleSpriteRenderer != null)
        {
            middleSpriteRenderer.sprite = sprite;
            middleSpriteRenderer.color = Color.black;
        }

        /*
        Utils.ResizeX(leftSide, leftSize, false, 2.56f);  
        Utils.ResizeY(leftSide, vertSize, false, 2.56f); 

        Utils.ResizeX(rightSide, rightSize, true, 2.56f);
        Utils.ResizeY(rightSide, vertSize, false, 2.56f);

        Utils.ResizeY(middleSide, vertSize-1*(diffA), false, 2.56f);
        */
        
        leftSide.localScale = new Vector3(leftSide.localScale.x*2.56f, leftSide.localScale.y*2.56f, 1f);
        rightSide.localScale = new Vector3(rightSide.localScale.x*2.56f, rightSide.localScale.y*2.56f, 1f);
        middleSide.localScale = new Vector3(middleSide.localScale.x*2.56f, middleSide.localScale.y*2.56f, 1f);

        Utils.ResizeX(leftSide, leftSize, false);  
        Utils.ResizeY(leftSide, vertSize, false); 

        Utils.ResizeX(rightSide, rightSize, true);
        Utils.ResizeY(rightSide, vertSize, false);
        
        Utils.ResizeY(middleSide, vertSize-1*diffA, false);

        leftSide.GetComponent<BoxCollider2D>().size = Vector2.one;
        rightSide.GetComponent<BoxCollider2D>().size = Vector2.one;
        middleSide.GetComponent<BoxCollider2D>().size = Vector2.one;


        return obj;

    }

    public override GameObject[] PlaceDefault()
    {
        throw new NotImplementedException();
    }
}