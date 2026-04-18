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


public static class LevelBuilder
{
    public static GameObject prefabFountain;
    public static GameObject prefabBlackhole;

    public static GameObject prefabSwitch;
    public static GameObject prefabPortal;
    public static GameObject prefabMovingPlatform;

    public static Transform rootHoleComps;
    public static GameObject prefabSurfaceBlock;
    public static GameObject prefabPassageBlock;
    public static GameObject prefabSandSurface;

    public static GameObject prefabLevelHole;
    public static GameObject objectObjectiveBall;

    public static void getPrefabsAndEmpty()
    {
        prefabFountain = GameObject.Find("Fountain");
        prefabBlackhole = GameObject.Find("Blackhole");

        prefabSwitch = GameObject.Find("Switch");
        prefabPortal = GameObject.Find("Portal");
        prefabMovingPlatform = GameObject.Find("Moving Platform");

        rootHoleComps = GameObject.Find("Hole Components").transform;
        prefabSurfaceBlock = rootHoleComps.Find("Surface Block (2)").gameObject;
        prefabPassageBlock = rootHoleComps.Find("Passage Block (3)").gameObject;
        prefabSandSurface = rootHoleComps.Find("Sand Surface").gameObject;

        prefabLevelHole = GameObject.Find("Level Hole");
        objectObjectiveBall = GameObject.Find("Objective Ball");

        HideAndDisable(prefabFountain);
        HideAndDisable(prefabBlackhole);
        HideAndDisable(prefabSwitch);
        HideAndDisable(prefabPortal);
        HideAndDisable(prefabMovingPlatform);
        HideAndDisable(prefabSurfaceBlock);
        HideAndDisable(prefabPassageBlock);
        HideAndDisable(prefabSandSurface);
        HideAndDisable(prefabLevelHole);

        // dont delete ball cuz i dont feel like it
        // HideAndDisable(prefabObjectiveBall);

        HideAndDisable(rootHoleComps.Find("Surface Block (1)").gameObject);
    }

    static void HideAndDisable(GameObject obj)
    {
        if (obj == null) return;

        obj.SetActive(false);
        obj.hideFlags = HideFlags.HideAndDontSave;
    }

    

    public static List<GameObject> buildFromObj(CustomLevel level)
    {

        LevelPlacer.setObjectiveBall(new Vector3(level.ballX, level.ballY, 0f));
        List<GameObject> result = new List<GameObject>();

        foreach (LevelObject obj in level.levelObjects)
        {
            GameObject? newObj = null;
            switch (obj.type)
            {
                case "hole":
                    newObj = LevelPlacer.placeLevelHole(Vec3D.fromJson(obj.data["pos"]), obj.data["rot"].GetSingle(), Vec3D.fromJson(obj.data["size"]));
                    break;
                case "blackhole":
                    newObj = LevelPlacer.placeBlackhole(Vec3D.fromJson(obj.data["pos"]));
                    break;
                case "portal":
                    newObj = LevelPlacer.placePortal(Vec3D.fromJson(obj.data["entry"]), Vec3D.fromJson(obj.data["exit"]));
                    break;
                case "moving platform":
                    newObj = LevelPlacer.placeMovingPlatform(Vec3D.fromJson(obj.data["start"]), Vec3D.fromJson(obj.data["end"]), obj.data["rot"].GetSingle());
                    break;
                case "block":
                    newObj = LevelPlacer.spawnPrefabWithValues(prefabSurfaceBlock, Vec3D.fromJson(obj.data["pos"]), Vec3D.fromJson(obj.data["size"]), obj.data["rot"].GetSingle());
                    break;
                default:
                    MelonLogger.Warning("Unknown level object type encountered");
                    break;
            }
            if (newObj != null)
            {
                result.Add(newObj); 
            }


        }
        return result;


    }

    public static void setMetadata(CustomLevel level)
    {
        GameObject.Find("Par Number Text (2)").GetComponent<TextMeshProUGUI>().text = level.par.ToString();
        GameObject.Find("Hole Number Text").GetComponent<TextMeshProUGUI>().text = level.name.ToString();
    }
}