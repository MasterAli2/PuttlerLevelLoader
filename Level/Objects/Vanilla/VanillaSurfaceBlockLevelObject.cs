using System;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


[RegisterLevelObject("block")]
[MelonLoader.RegisterTypeInIl2Cpp]
public class VanillaSurfaceBlockLevelObject : BaseLevelObject, IBaseLevelObject
{
    public static GameObject prefab;

    public static Sprite sprite;
    
    public VanillaSurfaceBlockLevelObject(IntPtr ptr) : base(ptr) {}

    public static GameObject Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = Utils.spawnPrefabWithValues(prefab,
         Vec3D.fromJson(serialLevelObject.data["pos"]),
          Vec3D.fromJson(serialLevelObject.data["size"]),
           serialLevelObject.data["rot"].GetSingle());
           
        obj.GetComponent<SpriteRenderer>().sprite = sprite;
        obj.GetComponent<SpriteRenderer>().color = Color.black;
        
        obj.GetComponent<BoxCollider2D>().size = Vector2.one;

        obj.AddComponent<VanillaSurfaceBlockLevelObject>();
        return obj;
        
    }

    public static void CleanScene()
    {
        if (sprite == null)
        {
            Texture2D tex = Texture2D.whiteTexture;
            sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), tex.width);
        }

        Transform rootHoleComps = GameObject.Find("Hole Components").transform;
        prefab = rootHoleComps.Find("Surface Block (2)").gameObject;

        Utils.HideAndDisable(prefab);
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