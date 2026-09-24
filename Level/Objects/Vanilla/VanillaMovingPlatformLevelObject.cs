
using System;
using Il2Cpp;
using MelonLoader;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#pragma warning disable CS8618

public class VanillaMovingPlatformLevelObject : BaseLevelObject
{
    public MovingPlatform movingPlatform;
    public MovingShadowPlatform movingShadowPlatform;

    public Transform pointA;
    public Transform pointB;

    public VanillaMovingPlatformLevelObject(IntPtr ptr) : base(ptr) { }

    void Start()
    {
        movingPlatform = GetComponentInChildren<MovingPlatform>();
        movingShadowPlatform = GetComponentInChildren<MovingShadowPlatform>();

        pointA = transform.GetChild(0);
        pointB = transform.GetChild(1);
    }

    public override void OnEditorUpdate()
    {
        base.OnEditorUpdate();

        if (movingPlatform == null || movingShadowPlatform == null || pointA == null) 
                return;
                
        movingPlatform.startPosition = pointA.transform.position;
        movingShadowPlatform.startPosition = pointA.transform.position;

        if (movingPlatform.startGoingPointA)
        {
            movingPlatform.targetPosition = pointA.position;
            //movingShadowPlatform.targetPosition = pointA.position;
        }
        else
        {
            movingPlatform.targetPosition = pointB.position;
            //movingShadowPlatform.targetPosition = pointB.position;
        }
    }
}

