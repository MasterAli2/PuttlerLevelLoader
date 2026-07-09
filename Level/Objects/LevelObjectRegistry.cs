using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


// this is mostly AI generated but it works so whatever
public static class LevelObjectRegistry
{
    public static Dictionary<string, Func<SerialLevelObject, GameObject>> Registry;
    public static Dictionary<string, Action> CleanupRegistry;

    public static void Build()
    {
        Registry = new Dictionary<string, Func<SerialLevelObject, GameObject>>();
        CleanupRegistry = new Dictionary<string, Action>();

        foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                typeof(IBaseLevelObject).IsAssignableFrom(t)))
        {
            var attribute = type.GetCustomAttribute<RegisterLevelObject>();
            if (attribute == null)
                continue;

            var placeMethod = type.GetMethod(
                nameof(IBaseLevelObject.Place),
                BindingFlags.Public | BindingFlags.Static);

            var cleanMethod = type.GetMethod(
                nameof(IBaseLevelObject.CleanScene),
                BindingFlags.Public | BindingFlags.Static);

            Registry[attribute.Name] = serial =>
                (GameObject)placeMethod!.Invoke(null, new object[] { serial })!;

            CleanupRegistry[attribute.Name] = () =>
                cleanMethod!.Invoke(null, null);
        }
    }
}