using System;
using System.Text.Json;
using Il2Cpp;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[RegisterLevelObject("moving platform")]
[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaMovingPlatformLevelObject : LinkedLevelObject, IBaseLevelObject
{
    public bool isShadow = false;
    public Transform pointA;
    public Transform pointB;
    public Transform shadow;
    public MovingPlatform movingPlatform;
    public MovingShadowPlatform movingShadowPlatform;

    public static GameObject prefab;

    public VanillaMovingPlatformLevelObject(IntPtr ptr) : base(ptr) {}


    public static GameObject[] Place(SerialLevelObject serialLevelObject)
    {
        (Transform obj1, Transform obj2, Transform pointB, Transform pointA, Transform myShadowObject) = placeMovingPlatform(
            Vec3D.fromJson(serialLevelObject.data["start"]),
            Vec3D.fromJson(serialLevelObject.data["end"]),
            serialLevelObject.data["rot"].GetSingle());

        VanillaMovingPlatformLevelObject platform = obj1.gameObject.AddComponent<VanillaMovingPlatformLevelObject>();
        platform.isShadow = false;

        
        //VanillaMovingPlatformLevelObject shadow = obj2.gameObject.AddComponent<VanillaMovingPlatformLevelObject>();
        //VanillaMovingPlatformLevelObject shadow = pointB.gameObject.AddComponent<VanillaMovingPlatformLevelObject>();
        VanillaMovingPlatformLevelObject shadow = myShadowObject.gameObject.AddComponent<VanillaMovingPlatformLevelObject>();
        shadow.isShadow = true;
        
        platform.movingPlatform = obj1.gameObject.GetComponent<MovingPlatform>();
        platform.movingShadowPlatform = obj1.gameObject.GetComponent<MovingShadowPlatform>();
        shadow.movingPlatform = obj1.gameObject.GetComponent<MovingPlatform>();
        shadow.movingShadowPlatform = obj1.gameObject.GetComponent<MovingShadowPlatform>();

        platform.links.Add(shadow);
        shadow.links.Add(platform);
        platform.links.Add(pointA.gameObject);
        shadow.links.Add(pointA.gameObject);
        platform.links.Add(pointB.gameObject);
        shadow.links.Add(pointB.gameObject);
        platform.links.Add(obj1.GetComponent<MovingShadowPlatform>().platform.gameObject);
        shadow.links.Add(obj1.GetComponent<MovingShadowPlatform>().platform.gameObject);

        platform.pointA = pointA;
        platform.pointB = pointB;
        platform.shadow = obj2.transform;

        shadow.pointA = pointA;
        shadow.pointB = pointB;
        shadow.shadow = obj2.transform;



        return new GameObject[] { platform.gameObject, shadow.gameObject };
    }

    public static void ApplyEditorPlaceButtons(GameObject gameObject)
    {
        Image image = gameObject.transform.GetChild(0).GetComponent<Image>();

        image.sprite = Utils.RuntimeSprite.square;
        image.color = new Color(255, 0, 155, 255);

        RectTransform rectTransform = image.rectTransform;


        rectTransform.localScale = new Vector3(rectTransform.localScale.x*2f, rectTransform.localScale.y*0.2f, rectTransform.localScale.z);
    }

    public static void CleanScene()
    {
        prefab = GameObject.Find("Moving Platform");
        Utils.HideAndDisable(prefab);
    }

    public static (Transform, Transform, Transform, Transform, Transform) placeMovingPlatform(Vector3 start, Vector3 end, float rotation)
    {
        Transform obj = Utils.spawnPrefab(prefab).transform;



        Transform movingPlatform = obj.GetChild(0);
        MovingPlatform originalMovingPlatform = obj.GetComponent<MovingPlatform>();
        MovingPlatform newMovingPlatform = movingPlatform.gameObject.AddComponent<MovingPlatform>();

        newMovingPlatform.platform = movingPlatform;

        newMovingPlatform.pointA = originalMovingPlatform.pointA;
        newMovingPlatform.pointB = originalMovingPlatform.pointB;

        newMovingPlatform.speed = originalMovingPlatform.speed;
        newMovingPlatform.startGoingPointA = originalMovingPlatform.startGoingPointA;

        Destroy(originalMovingPlatform);


        Transform shadowMovingPlatform = obj.GetChild(1);
        MovingShadowPlatform originalShadowMovingPlatform = obj.GetComponent<MovingShadowPlatform>();
        MovingShadowPlatform newShadowMovingPlatform = movingPlatform.gameObject.AddComponent<MovingShadowPlatform>();

        newShadowMovingPlatform.platform = shadowMovingPlatform;
        newShadowMovingPlatform.Awake();


        

        Destroy(originalShadowMovingPlatform);

        Transform pointB = obj.transform.GetChild(3);
        Transform pointA = obj.transform.GetChild(2);

        pointB.position = end;
        pointA.position = start;


        SpriteRenderer shadowObjectRenderer = (new GameObject("Shadow Level Object")).AddComponent<SpriteRenderer>();
        shadowObjectRenderer.sprite = shadowMovingPlatform.gameObject.GetComponent<SpriteRenderer>().sprite;
        Color shadowColor = shadowMovingPlatform.gameObject.GetComponent<SpriteRenderer>().color;
        shadowColor.a = 157.5f / 255f;
        shadowObjectRenderer.color = shadowColor;

        BoxCollider2D shadowCollider = shadowObjectRenderer.gameObject.AddComponent<BoxCollider2D>();
        shadowCollider.isTrigger = true;
        shadowCollider.sharedMaterial = movingPlatform.gameObject.GetComponent<BoxCollider2D>().sharedMaterial;

        shadowCollider.transform.position = pointB.transform.position;
        shadowCollider.transform.localScale = new Vector3(2f, 0.2f, 1f);


        obj.DetachChildren();
        Destroy(obj.gameObject);






        movingPlatform.position = start;
        movingPlatform.eulerAngles = new Vector3(0f, 0f, rotation);

        shadowMovingPlatform.position = start;
        shadowMovingPlatform.eulerAngles = new Vector3(0f, 0f, rotation);



        return (movingPlatform, shadowMovingPlatform, pointB, pointA, shadowCollider.transform);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (isShadow)
        {
            //Destroy(pointB.gameObject);
        }
        else
        {   
            //Destroy(GetComponent<MovingShadowPlatform>().platform.gameObject);
            //Destroy(pointA.gameObject);
        }
    }

    public override void OnEditorPickup()
    {
        // nothing
    }

    public override void OnEditorDrop()
    {

        var a = movingShadowPlatform.targetPosition;
        
        if (isShadow)
        {
            if (a == pointB.position)
            {
                a = transform.position;
            }

            pointB.position = transform.position; 
        }
        else
        {
            if (a == pointA.position)
            {
                a = transform.position;
            }
            pointA.position = transform.position;
        }


        // TODO: fix startGoingA field thing
        //movingPlatform.Start();
        movingPlatform.startPosition = pointA.position;
        movingPlatform.targetPosition = pointB.position;

        //movingShadowPlatform.Start();
        movingShadowPlatform.startPosition = pointA.position;
        movingShadowPlatform.targetPosition = a;

    }

    public static GameObject[] PlaceDefault()
    {
        SerialLevelObject serialLevelObject = new SerialLevelObject();

        serialLevelObject.data["start"] = Vec3D.toJson(Vector3.zero);
        serialLevelObject.data["end"] = Vec3D.toJson(Vector3.zero);
        serialLevelObject.data["rot"] = JsonDocument.Parse("0").RootElement;

        return VanillaMovingPlatformLevelObject.Place(serialLevelObject);


    }
}

