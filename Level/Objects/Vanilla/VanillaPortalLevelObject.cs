using System;
using UnityEngine;
using Il2Cpp;

[RegisterLevelObject("portal")]
[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaPortalLevelObject : MonoBehaviour, IBaseLevelObject
{
    public static GameObject prefab;

    public VanillaPortalLevelObject(IntPtr ptr) : base(ptr) {}

    public static GameObject Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = placePortal(
            Vec3D.fromJson(serialLevelObject.data["entry"]),
            Vec3D.fromJson(serialLevelObject.data["exit"]));
        obj.AddComponent<VanillaPortalLevelObject>();
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
}
