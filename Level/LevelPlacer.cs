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
    
    
    public static void setObjectiveBall(Vector3 position)
    {
        LevelBuilder.objectObjectiveBall.transform.position = position;
        LevelBuilder.objectObjectiveBall.GetComponent<ObjectiveBall>().Awake();
    }




    
}

    
