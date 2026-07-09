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

    public static GameObject objectObjectiveBall;

    public static void getPrefabsAndEmpty()
    {
        objectObjectiveBall = GameObject.Find("Objective Ball");

        Transform rootHoleComps = GameObject.Find("Hole Components").transform;

        GameObject prefabPassageBlock = rootHoleComps.Find("Passage Block (3)").gameObject;
        GameObject prefabSandSurface = rootHoleComps.Find("Sand Surface").gameObject;

        GameObject prefabFountain = GameObject.Find("Fountain");
        GameObject prefabSwitch = GameObject.Find("Switch");


        foreach (var pair in LevelObjectRegistry.CleanupRegistry)
        {
            pair.Value.Invoke();
        }

        Utils.HideAndDisable(prefabPassageBlock);
        Utils.HideAndDisable(prefabSandSurface);
        Utils.HideAndDisable(prefabFountain);
        Utils.HideAndDisable(prefabSwitch);
        Utils.HideAndDisable(rootHoleComps.Find("Surface Block (1)").gameObject);
    }



    

    public static List<GameObject> buildFromObj(CustomLevel level)
    {

        placeObjectiveBall(new Vector3(level.ballX, level.ballY, 0f));
        List<GameObject> result = new List<GameObject>();

        foreach (SerialLevelObject obj in level.levelObjects)
        {
            GameObject? newObj = null;
            
            if (!LevelObjectRegistry.Registry.ContainsKey(obj.type))
            {
                MelonLogger.Warning("Unknown level object type found");
                continue;
            }

            newObj = LevelObjectRegistry.Registry[obj.type].Invoke(obj);

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

    public static void placeObjectiveBall(Vector3 position)
    {
        objectObjectiveBall.transform.position = position;

        // this is probably to fix starting behaviour
        objectObjectiveBall.GetComponent<ObjectiveBall>().Awake();
    }
}