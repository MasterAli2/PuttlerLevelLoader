using System;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[RegisterLevelObject("blackhole")]
public class VanillaBlackholeLevelObjectDefinition : LevelObjectDefinition
{
    public static GameObject prefab;

    public override BaseLevelObject Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = Utils.spawnPrefabWithValues(prefab, Vec3D.fromJson(serialLevelObject.data["pos"]));
        
        BaseLevelObject baseLevelObject = obj.AddComponent<VanillaBlackholeLevelObject>();
        obj.AddComponent<BaseSelectableObject>();

        GameObject.Destroy(obj.transform.GetChild(0).gameObject);
        GameObject spriteObj = new GameObject("Sprite");
        spriteObj.transform.SetParent(obj.transform, false); 

        spriteObj.transform.localScale = (Vector3.one * 0.45f) * 2.56f;

        SpriteRenderer spriteRenderer = spriteObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Utils.RuntimeSprite.circle;
        spriteRenderer.color = Color.black;

        CircleCollider2D entryCollision = obj.AddComponent<CircleCollider2D>();
        entryCollision.isTrigger = true;
        entryCollision.radius = 0.6f;

        Utils.disableCollision(entryCollision);

        return baseLevelObject;
    }

    public override void CleanScene()
    {
        prefab = GameObject.Find("Blackhole");
        Utils.HideAndDisable(prefab);
    }


    public override BaseLevelObject PlaceDefault()
    {
        SerialLevelObject serialLevelObject = new SerialLevelObject();

        serialLevelObject.data["pos"] = Vec3D.toJson(Vector3.zero);

        return Place(serialLevelObject);
    }
}
