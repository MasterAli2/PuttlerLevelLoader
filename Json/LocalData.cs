using MelonLoader;
using MelonLoader.Utils;
using System;
using System.IO;
using System.Text.Json;

using UnityEngine;

static class LocalData
{
    public static string dataPath = Path.Combine(MelonEnvironment.MelonBaseDirectory, "PeakLevelLoader/");
    public static string localLevelsPath = Path.Combine(dataPath, "LocalLevels/");
    //public static List<CustomLevel> customLevels = new List<CustomLevel>();
    public static Dictionary<string, CustomLevel> localLevels = new Dictionary<string, CustomLevel>();

    public static void init()
    {
        reloadLocalLevels();
    }

    public static void reloadLocalLevels()
    {
        Directory.CreateDirectory(localLevelsPath);

        localLevels = new Dictionary<string, CustomLevel>();
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
                    localLevels.Add(file, level);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"Error loading local level {file}: {ex}");
            }
        }

    }

    public static void createNewEmptyLocalLevel()
    {
        LocalData.reloadLocalLevels();

        CustomLevel customLevel = new CustomLevel();

        string baseName = "New Level";
        string name = baseName;
        int suffix = 1;

        while (localLevels.Values.Any(level => level.name == name))
        {
            name = $"{baseName} ({suffix})";
            suffix++;
        }

        customLevel.name = name;
        customLevel.par = 3;

        customLevel.ballX = 0;
        customLevel.ballY = 5;

        SerialLevelObject goal = new SerialLevelObject();
        goal.type = "hole";

        goal.data["pos"] = Vec3D.toJson(new Vector3(0, -3, 0));
        goal.data["rot"] = JsonSerializer.SerializeToElement(0f);
        goal.data["size"] = Vec3D.toJson(new Vector3(1, 1, 5));

        customLevel.levelObjects.Add(goal);

        // generate random file name
        string fileName = $"{Guid.NewGuid()}.json";

        File.WriteAllText(Path.Combine(localLevelsPath, fileName), customLevel.ToJson());
    }

    public static void saveLocalLevel(string levelId, CustomLevel customLevel)
    {
        File.WriteAllText(Path.Combine(localLevelsPath, levelId), customLevel.ToJson());
    }
}