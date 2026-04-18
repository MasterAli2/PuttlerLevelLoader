using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Vec3DConverter : JsonConverter<Vec3D>
{
    public override Vec3D Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();

        reader.Read();
        float x = reader.GetSingle();

        reader.Read();
        float y = reader.GetSingle();

        reader.Read();
        float z = reader.GetSingle();

        reader.Read(); // EndArray

        return new Vec3D(x, y, z);
    }

    public override void Write(Utf8JsonWriter writer, Vec3D value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.x);
        writer.WriteNumberValue(value.y);
        writer.WriteNumberValue(value.z);
        writer.WriteEndArray();
    }
}