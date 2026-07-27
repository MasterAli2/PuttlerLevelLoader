using System;
using UnityEngine;
using Il2Cpp;
using System.Text.Json;
using UnityEngine.UI;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[RegisterLevelObject("portal")]
public class VanillaPortalLevelObjectDefinition : LevelObjectDefinition
{
    public static GameObject prefab;

    public override GameObject[] Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = placePortal(
            Vec3D.fromJson(serialLevelObject.data["entry"]),
            Vec3D.fromJson(serialLevelObject.data["exit"]));

        GameObject entryObj = obj.transform.GetChild(0).gameObject;
        GameObject exitObj = obj.transform.GetChild(1).gameObject;
        GameObject audio = obj.transform.GetChild(2).gameObject;

        obj.transform.DetachChildren();
        GameObject.Destroy(obj);
        
        VanillaPortalLevelObject entry = entryObj.AddComponent<VanillaPortalLevelObject>();
        VanillaPortalLevelObject exit = exitObj.AddComponent<VanillaPortalLevelObject>();

        entry.links.Add(exit);
        exit.links.Add(entry);
        entry.links.Add(audio);
        exit.links.Add(audio);

        CircleCollider2D entryCollision = entry.gameObject.AddComponent<CircleCollider2D>();
        entryCollision.isTrigger = true;
        entryCollision.radius = 0.6f;

        Utils.disableCollision(entryCollision);

        CircleCollider2D exitCollision = exit.gameObject.AddComponent<CircleCollider2D>();
        exitCollision.isTrigger = true;
        exitCollision.radius = 0.6f;

        Utils.disableCollision(exitCollision);

        return new GameObject[] { entry.gameObject, exit.gameObject };
    }

    public override void ApplyEditorPlaceButtons(GameObject gameObject)
    {
        Image outerImage = gameObject.transform.GetChild(0).GetComponent<Image>();

        GameObject innerObj = GameObject.Instantiate(outerImage.gameObject, outerImage.transform.parent);
        Image innerImage = innerObj.GetComponent<Image>();

        outerImage.sprite = Utils.RuntimeSprite.circle;
        outerImage.color = new Color(0, 155, 255, 255);

        


        innerImage.sprite = Utils.RuntimeSprite.circle;
        innerImage.color = Color.black;

        RectTransform innerRectTransform = outerImage.rectTransform;
        innerRectTransform.localScale = new Vector3(innerRectTransform.localScale.x, innerRectTransform.localScale.y, innerRectTransform.localScale.z);
        RectTransform outerRectTransform = outerImage.rectTransform;
        outerRectTransform.localScale = new Vector3(outerRectTransform.localScale.x*1.25f, outerRectTransform.localScale.y*1.25f, outerRectTransform.localScale.z);
    

    }

    public override void CleanScene()
    {
        prefab = GameObject.Find("Portal");
        Utils.HideAndDisable(prefab);
    }

    public static GameObject placePortal(Vector3 entryPos, Vector3 exitPos)
    {
        GameObject obj = Utils.spawnPrefab(prefab);

        obj.transform.position = Vector3.zero;

        Transform entry = obj.transform.GetChild(0);
        Transform exit = obj.transform.GetChild(1);
        AudioSource audio = obj.transform.GetChild(2).GetComponent<AudioSource>();

        entry.position = entryPos;
        exit.position = exitPos;

        Portal portal = entry.GetComponent<Portal>();
        portal._destination = exit;
        portal.portalAudio = audio;

        return obj;
    }

    public override GameObject[] PlaceDefault()
    {
        SerialLevelObject serialLevelObject = new SerialLevelObject();

        serialLevelObject.data["entry"] = Vec3D.toJson(Vector3.zero);
        serialLevelObject.data["exit"] = Vec3D.toJson(Vector3.zero);

        return Place(serialLevelObject);


    }
}
