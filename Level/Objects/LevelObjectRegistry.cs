using System.Collections.Generic;
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
            Type[] types;

            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(t => t != null).Cast<Type>().ToArray();

                foreach (Exception? loaderException in ex.LoaderExceptions)
                {
                    Debug.LogWarning(loaderException?.ToString());
                }
            }

            foreach (Type type in types)
            {
                var attribute = type.GetCustomAttribute<RegisterLevelObject>();
                if (attribute == null)
                    continue;

                if (!typeof(LevelObjectDefinition).IsAssignableFrom(type))
                {
                    Debug.LogError($"{type.FullName} has RegisterLevelObject but does not inherit LevelObjectDefinition.");
                    continue;
                }

                if (type.IsAbstract)
                    continue;

                if (Activator.CreateInstance(type) is not LevelObjectDefinition instance)
                {
                    Debug.LogError($"Failed to create instance of {type.FullName}.");
                    continue;
                }

                if (!Registry.TryAdd(attribute.Name, instance))
                {
                    Debug.LogError($"Duplicate LevelObject name '{attribute.Name}' ({type.FullName}).");
                }
            }
        }
    }
}