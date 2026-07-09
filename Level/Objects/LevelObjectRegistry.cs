using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class LevelObjectRegistry
{
    public static Dictionary<string, Func<SerialLevelObject, GameObject>> Registry;
    
    public static void Build()
    {
        Registry = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                typeof(IBaseLevelObject).IsAssignableFrom(t))
            .Select(t => new
            {
                Type = t,
                Attribute = t.GetCustomAttribute<RegisterLevelObject>()
            })
            .Where(x => x.Attribute != null)
            .ToDictionary(
                x => x.Attribute!.Name,
                x => (Func<SerialLevelObject, GameObject>)(serial =>
                {
                    var method = x.Type.GetMethod(
                        nameof(IBaseLevelObject.Place),
                        BindingFlags.Public |
                        BindingFlags.Static);

                    return (GameObject)method!.Invoke(null, new object[] { serial })!;
                }));
    }
}