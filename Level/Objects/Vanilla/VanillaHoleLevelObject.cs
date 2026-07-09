using System;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[RegisterLevelObject("hole")]
[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaHoleLevelObject : BaseLevelObject, IBaseLevelObject
{
    public static GameObject prefab;

    public VanillaHoleLevelObject(IntPtr ptr) : base(ptr) {}

    public static GameObject Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = placeLevelHole(Vec3D.fromJson(serialLevelObject.data["pos"]), serialLevelObject.data["rot"].GetSingle(), Vec3D.fromJson(serialLevelObject.data["size"]));
        obj.AddComponent<VanillaHoleLevelObject>();
        return obj;
        
    }

    public static void CleanScene()
    {
        prefab = GameObject.Find("Level Hole");
        Utils.HideAndDisable(prefab);
    }

    // this is kept here cuz of how complicated it is, i have no idea how and why this works
    public static GameObject placeLevelHole(Vector3 position, float rotation, Vector3 size)
    {
        GameObject obj = Utils.spawnPrefab(prefab);

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
}