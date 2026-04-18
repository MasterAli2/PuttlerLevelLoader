using MelonLoader;
using MelonLoader.Utils;
using System;
using System.IO;
using System.Text.Json;

static class LocalData
{
    public static string dataPath = Path.Combine(MelonEnvironment.MelonBaseDirectory, "PeakLevelLoader/");
    public static string localLevelsPath = Path.Combine(dataPath, "LocalLevels/");
    public static List<CustomLevel> customLevels = new List<CustomLevel>();

    public static void init()
    {
        reloadLocalLevels();
    }

    public static void reloadLocalLevels()
    {
        Directory.CreateDirectory(localLevelsPath);

        customLevels = new List<CustomLevel>();
        var files = Directory
            .GetFiles(localLevelsPath, "*.json")
            .OrderBy(f => File.GetLastWriteTime(f))
            .ToArray();

        foreach (string file in files)
        {
            try
            {
                string json = File.ReadAllText(file);

                CustomLevel level = CustomLevel.FromJson(json);

                // Prettyfy the json :3
                //File.WriteAllText(file, level.ToJson());

                if (level != null)
                {

                    customLevels.Add(level);
                }
            }
            catch
            {
                MelonLogger.Error($"Error loading local level {file}");
            }
        }

    }
}