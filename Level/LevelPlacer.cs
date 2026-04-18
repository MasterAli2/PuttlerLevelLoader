using Il2Cpp;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine.UI;
using UnityEngine.Events;
using Il2CppTMPro;
using System.Drawing;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.



public static class LevelPlacer
{
    public static GameObject placeLevelHole(Vector3 position, float rotation, Vector3 size)
    {
        GameObject obj = spawnPrefab(LevelBuilder.prefabLevelHole);

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

        Utils.ResizeX(leftSide, leftSize, false, 2.56f);  
        Utils.ResizeY(leftSide, vertSize, false, 2.56f); 

        Utils.ResizeX(rightSide, rightSize, true, 2.56f);
        Utils.ResizeY(rightSide, vertSize, false, 2.56f);

        Utils.ResizeY(middleSide, vertSize-1*(diffA), false, 2.56f);

        return obj;

    }
    
    public static void setObjectiveBall(Vector3 position)
    {
        LevelBuilder.objectObjectiveBall.transform.position = position;
        LevelBuilder.objectObjectiveBall.GetComponent<ObjectiveBall>().Awake();
    }
    public static GameObject placePortal(Vector3 entryPos, Vector3 exitPos)
    {
        GameObject obj = spawnPrefab(LevelBuilder.prefabPortal);

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
    public static GameObject placeMovingPlatform(Vector3 start, Vector3 end, float rotation)
    {
        GameObject obj = spawnPrefab(LevelBuilder.prefabMovingPlatform);

        obj.transform.position = start;
        obj.transform.eulerAngles = new Vector3(0f, 0f, rotation);


        obj.transform.GetChild(3).position = end;

        return obj;
    }
    public static GameObject placeBlackhole(Vector3 position)
    {
        GameObject obj = spawnPrefab(LevelBuilder.prefabBlackhole);

        obj.transform.position = position;

        return obj;
    }
    public static GameObject spawnPrefabWithValues(GameObject prefab, Vector3 position, Vector3 size, float rotation)
    {
        GameObject obj = spawnPrefab(prefab);

        obj.transform.position = position;

        obj.transform.eulerAngles = new Vector3(0f, 0f, rotation);
        obj.transform.localScale = size;

        return obj;
    }
    public static GameObject spawnPrefab(GameObject prefab)
    {
        GameObject obj = GameObject.Instantiate(prefab);
        obj.hideFlags = HideFlags.None;
        obj.SetActive(true);

        return obj;
    }
}

    
