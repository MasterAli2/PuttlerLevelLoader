using System;
using UnityEngine;
using Il2Cpp;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[RegisterLevelObject("portal")]
[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaPortalLevelObject : LinkedLevelObject, IBaseLevelObject
{
    public static GameObject prefab;

    public VanillaPortalLevelObject(IntPtr ptr) : base(ptr) {}

    public static GameObject Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = placePortal(
            Vec3D.fromJson(serialLevelObject.data["entry"]),
            Vec3D.fromJson(serialLevelObject.data["exit"]));

        GameObject entryObj = obj.transform.GetChild(0).gameObject;
        GameObject exitObj = obj.transform.GetChild(1).gameObject;
        GameObject audio = obj.transform.GetChild(2).gameObject;

        obj.transform.DetachChildren();
        
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

        return obj;
    }

    public static void CleanScene()
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

    public override void OnEditorPickup()
    {
        // nothing
    }
    public override void OnEditorDrop()
    {
        // nothing
    }
}
