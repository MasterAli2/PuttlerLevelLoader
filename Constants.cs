using System.Text.Json;

public static class Constants
{
    public static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        IncludeFields = true
        #if DEBUG
        ,
        WriteIndented = true
        #endif
    };

}