using System;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[RegisterLevelObject("blackhole")]
public class VanillaBlackholeLevelObjectDefinition : LevelObjectDefinition
{
    public static GameObject prefab;

    public override GameObject[] Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = Utils.spawnPrefabWithValues(prefab, Vec3D.fromJson(serialLevelObject.data["pos"]));
        
        obj.AddComponent<VanillaBlackholeLevelObject>();

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

        return new GameObject[] { obj };
    }
    

    public override void ApplyEditorPlaceButtons(GameObject gameObject)
    {
        Image innerImager = gameObject.transform.GetChild(0).GetComponent<Image>();

        GameObject outerObj = GameObject.Instantiate(innerImager.gameObject, innerImager.transform.parent);
        Image outerImage = outerObj.GetComponent<Image>();

        innerImager.sprite = Utils.RuntimeSprite.circle;
        innerImager.color = Color.black;

        


        outerImage.sprite = Utils.RuntimeSprite.circle;
        outerImage.color = new Color(155, 0, 255, 255);;


        RectTransform outerRectTransform = outerImage.rectTransform;
        outerRectTransform.localScale = new Vector3(outerRectTransform.localScale.x * 0.75f, outerRectTransform.localScale.y * 0.75f, outerRectTransform.localScale.z);
        RectTransform innerRectTransform = innerImager.rectTransform;
        innerRectTransform.localScale = new Vector3(innerRectTransform.localScale.x, innerRectTransform.localScale.y, innerRectTransform.localScale.z);
    

    }

    public override void CleanScene()
    {
        prefab = GameObject.Find("Blackhole");
        Utils.HideAndDisable(prefab);
    }


    public override GameObject[] PlaceDefault()
    {
        SerialLevelObject serialLevelObject = new SerialLevelObject();

        serialLevelObject.data["pos"] = Vec3D.toJson(Vector3.zero);

        return Place(serialLevelObject);
    }
}
