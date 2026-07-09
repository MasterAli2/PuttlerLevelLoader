using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class CustomLevel
{
    public string name;
    public int difficulty;
    public int par;
    public float ballX;
    public float ballY;
    public List<SerialLevelObject> levelObjects;

    public string ToJson()
    {
        return JsonSerializer.Serialize<CustomLevel>(this, Constants.jsonSerializerOptions);
    }

    public static CustomLevel FromJson(string json)
    {
        return JsonSerializer.Deserialize<CustomLevel>(json, Constants.jsonSerializerOptions) ?? new CustomLevel();
    }

}

public class SerialLevelObject
{
    public string type;
    public Dictionary<string, JsonElement> data = new();

    public string ToJson()
    {
        return JsonSerializer.Serialize<SerialLevelObject>(this, Constants.jsonSerializerOptions);
    }

    public static SerialLevelObject FromJson(string json)
    {
        return JsonSerializer.Deserialize<SerialLevelObject>(json, Constants.jsonSerializerOptions) ?? new SerialLevelObject();
    }
}


[JsonConverter(typeof(Vec3DConverter))]
public struct Vec3D
{
    public float x, y, z;
    public Vec3D(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
 
    public static Vector3 fromJson(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Array)
        {
            float x = element.GetArrayLength() > 0 ? element[0].GetSingle() : 0f;
            float y = element.GetArrayLength() > 1 ? element[1].GetSingle() : 0f;
            float z = element.GetArrayLength() > 2 ? element[2].GetSingle() : 0f;

            return new Vector3(x, y, z);
        }

        return Vector3.zero;
    }
}
