using System;
using System.Text.Json;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[RegisterLevelObject("hole")]
public class VanillaHoleLevelObjectDefinition : LevelObjectDefinition
{
    public static GameObject prefab;
    public override BaseLevelObject Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = placeLevelHole(Vec3D.fromJson(serialLevelObject.data["pos"]), serialLevelObject.data["rot"].GetSingle(), Vec3D.fromJson(serialLevelObject.data["size"]));
        
        BaseLevelObject baseLevelObject = obj.AddComponent<VanillaHoleLevelObject>();
        obj.AddComponent<BaseSelectableObject>();
        return baseLevelObject;
        
    }

    public override void CleanScene()
    {
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
        diffA *= 2f;

        float rightSize, leftSize, vertSize;
        rightSize = size.x/2f-0.4096f/2f;
        leftSize = size.y/2f-0.4096f/2f;
        vertSize = size.z;

        SpriteRenderer leftSpriteRenderer = leftSide.GetComponent<SpriteRenderer>();
        if (leftSpriteRenderer != null)
        {
            leftSpriteRenderer.sprite = Utils.RuntimeSprite.square;
            leftSpriteRenderer.color = Color.black;
        }
        SpriteRenderer rightSpriteRenderer = rightSide.GetComponent<SpriteRenderer>();
        if (rightSpriteRenderer != null)
        {
            rightSpriteRenderer.sprite = Utils.RuntimeSprite.square;
            rightSpriteRenderer.color = Color.black;
        }
        SpriteRenderer middleSpriteRenderer = middleSide.GetComponent<SpriteRenderer>();
        if (middleSpriteRenderer != null)
        {
            middleSpriteRenderer.sprite = Utils.RuntimeSprite.square;
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

        middleSide.localPosition = new Vector3(middleSide.localPosition.x, leftSide.localPosition.y - diffA / 2f, middleSide.localPosition.z);
        middleSide.localScale = new Vector3(0.4096f, leftSide.localScale.y-diffA, middleSide.localScale.z);

        leftSide.GetComponent<BoxCollider2D>().size = Vector2.one;
        rightSide.GetComponent<BoxCollider2D>().size = Vector2.one;
        middleSide.GetComponent<BoxCollider2D>().size = Vector2.one;


        return obj;

    }

    public override BaseLevelObject PlaceDefault()
    {
        SerialLevelObject serialLevelObject = new SerialLevelObject();

        serialLevelObject.data["pos"] = Vec3D.toJson(Vector3.zero);
        serialLevelObject.data["size"] = Vec3D.toJson(new(2f, 3f, 1f));
        serialLevelObject.data["rot"] = JsonDocument.Parse("0").RootElement;

        return Place(serialLevelObject);
    }
}