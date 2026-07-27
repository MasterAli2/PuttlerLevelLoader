using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

#pragma warning disable CS8618

public static class LevelObjectRegistry
{
    public static Dictionary<string, LevelObjectDefinition> Registry;

    public static void Build()
    {
        Registry = new Dictionary<string, LevelObjectDefinition>();

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                var attribute = type.GetCustomAttribute<RegisterLevelObject>();
                if (attribute == null)
                    continue;

                if (!typeof(LevelObjectDefinition).IsAssignableFrom(type))
                {
                    Debug.LogError($"{type.FullName} has RegisterLevelObject but does not inherit LevelObjectDefinition.");
                    continue;
                }

                if (Activator.CreateInstance(type) is not LevelObjectDefinition instance)
                {
                    Debug.LogError($"Failed to create instance of {type.FullName}.");
                    continue;
                }

                Registry.Add(attribute.Name, instance);
            }
        }
    }
}