using System;
using System.Text.Json;
using Il2Cpp;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaMovingPlatformLevelObject : LinkedLevelObject
{
    public bool isShadow = false;
    public Transform pointA;
    public Transform pointB;
    public Transform shadow;
    public MovingPlatform movingPlatform;
    public MovingShadowPlatform movingShadowPlatform;

    public static GameObject prefab;

    public VanillaMovingPlatformLevelObject(IntPtr ptr) : base(ptr) {}

    void Start()
    {
        shadow.transform.position = transform.position;
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
}

