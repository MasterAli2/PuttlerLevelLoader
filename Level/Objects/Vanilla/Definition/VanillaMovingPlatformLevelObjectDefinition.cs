
using System.Text.Json;
using Il2Cpp;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[RegisterLevelObject("moving platform")]
public class VanillaMovingPlatformLevelObjectDefinition : LevelObjectDefinition
{

    public static GameObject prefab;

    public override BaseLevelObject Place(SerialLevelObject serialLevelObject)
    {


        GameObject obj = placeMovingPlatform(
            Vec3D.fromJson(serialLevelObject.data["start"]),
            Vec3D.fromJson(serialLevelObject.data["end"]),
            serialLevelObject.data["rot"].GetSingle());

        BaseLevelObject baseLevelObject = obj.AddComponent<VanillaMovingPlatformLevelObject>();

        GameObject pointA = obj.transform.GetChild(2).gameObject;
        GameObject pointB = obj.transform.GetChild(3).gameObject;

        GameObject pointASPO = new GameObject("Sprite Renderer");
        pointASPO.transform.parent = pointA.transform;
        GameObject pointBSPO = new GameObject("Sprite Renderer");
        pointBSPO.transform.parent = pointB.transform;

        SpriteRenderer pointASP = pointASPO.AddComponent<SpriteRenderer>();
        SpriteRenderer pointBSP = pointBSPO.AddComponent<SpriteRenderer>();

        //pointASP.sortingOrder = 0;
        //pointBSP.sortingOrder = 0;

        pointASP.sprite = Utils.RuntimeSprite.circle;
        pointBSP.sprite = Utils.RuntimeSprite.circle;

        Color color = new Color(255, 0, 155);

        pointASP.color = color;
        pointBSP.color = color;

        pointASP.transform.localScale = Vector3.one*.1f;
        pointBSP.transform.localScale = Vector3.one*.1f;

        pointA.transform.localScale = Vector3.one;
        pointB.transform.localScale = Vector3.one;

        BoxCollider2D pointABX = pointA.AddComponent<BoxCollider2D>();
        BoxCollider2D pointBBX = pointB.AddComponent<BoxCollider2D>();

        pointABX.isTrigger = true;
        pointBBX.isTrigger = true;

        pointA.AddComponent<BaseSelectableObject>();
        pointB.AddComponent<BaseSelectableObject>();

        Transform platform = obj.transform.GetChild(0);
        Transform shadow = obj.transform.GetChild(1);

        platform.SetParent(pointA.transform);
        shadow.SetParent(pointA.transform);

        platform.position = pointA.transform.position;
        shadow.position = pointA.transform.position;

        platform.rotation = pointA.transform.rotation;
        shadow.rotation = pointA.transform.rotation;

        //platform.localScale = platform.localScale * 10f;
        //shadow.localScale = pointA.transform.localScale * 10f;


        MovingPlatform movingPlatform = obj.transform.GetComponent<MovingPlatform>();
        MovingShadowPlatform shadowMovingPlatform = obj.transform.GetComponent<MovingShadowPlatform>();

        movingPlatform.startPosition = pointA.transform.position;
        shadowMovingPlatform.startPosition = pointA.transform.position;
        


        return baseLevelObject;
    }

    public override void ApplyEditorPlaceButtons(GameObject gameObject)
    {
        Image image = gameObject.transform.GetChild(0).GetComponent<Image>();

        image.sprite = Utils.RuntimeSprite.square;
        image.color = new Color(255, 0, 155, 255);

        RectTransform rectTransform = image.rectTransform;


        rectTransform.localScale = new Vector3(rectTransform.localScale.x*2f, rectTransform.localScale.y*0.2f, rectTransform.localScale.z);
    }

    public override void CleanScene()
    {
        prefab = GameObject.Find("Moving Platform");
        Utils.HideAndDisable(prefab);
    }

    public static GameObject placeMovingPlatform(Vector3 start, Vector3 end, float rotation)
    {
        Transform obj = Utils.spawnPrefab(prefab).transform;



        Transform movingPlatform = obj.GetChild(0);
        MovingPlatform originalMovingPlatform = obj.GetComponent<MovingPlatform>();

        Transform shadowMovingPlatform = obj.GetChild(1);
        MovingShadowPlatform originalShadowMovingPlatform = obj.GetComponent<MovingShadowPlatform>();


        Transform pointA = obj.transform.GetChild(2);
        Transform pointB = obj.transform.GetChild(3);

        pointB.position = end;
        pointA.position = start;



        /*
        GameObject dotfish = GameObject.Instantiate(movingPlatform.gameObject, movingPlatform.parent);

        var c = dotfish.GetComponent<BoxCollider2D>();
        c.isTrigger = true;
        var dotfishRenderer = dotfish.GetComponent<SpriteRenderer>();
        var dotfishColor = dotfishRenderer.color;
        dotfishColor.a = 0.5f;
        dotfishRenderer.color = dotfishColor;

        GameObject adotfish = GameObject.Instantiate(movingPlatform.gameObject, movingPlatform.parent);

        var c1 = adotfish.GetComponent<BoxCollider2D>();
        c1.isTrigger = true;
        var adotfishRenderer = adotfish.GetComponent<SpriteRenderer>();
        var adotfishColor = adotfishRenderer.color;
        adotfishColor.a = 0.5f;
        adotfishRenderer.color = adotfishColor;
        */
        /*

        SpriteRenderer shadowObjectRenderer = (new GameObject("Shadow Level Object")).AddComponent<SpriteRenderer>();
        shadowObjectRenderer.transform.SetParent(obj);
        shadowObjectRenderer.sprite = shadowMovingPlatform.gameObject.GetComponent<SpriteRenderer>().sprite;
        Color shadowColor = shadowMovingPlatform.gameObject.GetComponent<SpriteRenderer>().color;
        shadowColor.a = 157.5f / 255f;
        shadowObjectRenderer.color = shadowColor;

        BoxCollider2D shadowCollider = shadowObjectRenderer.gameObject.AddComponent<BoxCollider2D>();
        shadowCollider.isTrigger = true;
        shadowCollider.sharedMaterial = movingPlatform.gameObject.GetComponent<BoxCollider2D>().sharedMaterial;

        shadowCollider.transform.position = pointB.transform.position;
        shadowCollider.transform.localScale = new Vector3(2f, 0.2f, 1f);


        */




        movingPlatform.position = start;
        movingPlatform.eulerAngles = new Vector3(0f, 0f, rotation);

        shadowMovingPlatform.position = start;
        shadowMovingPlatform.eulerAngles = new Vector3(0f, 0f, rotation);

        /*
        Utils.InvokeMethod(originalMovingPlatform, "Start");
        Utils.InvokeMethod(originalShadowMovingPlatform, "Start");
        */


        return obj.gameObject;
    }

    public override BaseLevelObject PlaceDefault()
    {

        SerialLevelObject serialLevelObject = new SerialLevelObject();

        serialLevelObject.data["start"] = Vec3D.toJson(Vector3.zero);
        serialLevelObject.data["end"] = Vec3D.toJson(Vector3.zero);
        serialLevelObject.data["rot"] = JsonDocument.Parse("0").RootElement;

        return Place(serialLevelObject);


    }
}

