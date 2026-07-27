using System;
using System.Text.Json;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[RegisterLevelObject("block")]
public class VanillaSurfaceBlockLevelObjectDefinition : LevelObjectDefinition
{
    public static GameObject prefab;

    public override GameObject[] Place(SerialLevelObject serialLevelObject)
    {
        GameObject obj = Utils.spawnPrefabWithValues(prefab,
         Vec3D.fromJson(serialLevelObject.data["pos"]),
          Vec3D.fromJson(serialLevelObject.data["size"]),
           serialLevelObject.data["rot"].GetSingle());
           
        obj.GetComponent<SpriteRenderer>().sprite = Utils.RuntimeSprite.square;
        obj.GetComponent<SpriteRenderer>().color = Color.black;
        
        obj.GetComponent<BoxCollider2D>().size = Vector2.one;

        obj.AddComponent<VanillaSurfaceBlockLevelObject>();
        return new GameObject[] { obj };
        
    }

    public override void ApplyEditorPlaceButtons(GameObject gameObject)
    {
        Image image = gameObject.transform.GetChild(0).GetComponent<Image>();

        image.sprite = Utils.RuntimeSprite.square;
        image.color = Color.black;
    }

    public override void CleanScene()
    {
        Transform rootHoleComps = GameObject.Find("Hole Components").transform;
        prefab = rootHoleComps.Find("Surface Block (2)").gameObject;

        Utils.HideAndDisable(prefab);
    }

    public override GameObject[] PlaceDefault()
    {
        SerialLevelObject serialLevelObject = new SerialLevelObject();

        serialLevelObject.data["pos"] = Vec3D.toJson(Vector3.zero);
        serialLevelObject.data["size"] = Vec3D.toJson(Vector3.one);
        serialLevelObject.data["rot"] = JsonDocument.Parse("0").RootElement;

        return Place(serialLevelObject);
    }
}