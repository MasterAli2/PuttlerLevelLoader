using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


// this is mostly AI generated but it works so whatever
public static class LevelObjectRegistry
{
    public static Dictionary<string, Func<SerialLevelObject, GameObject[]>> Registry;
    public static Dictionary<string, Func<GameObject[]>> PlaceDefaultRegistry;
    public static Dictionary<string, Action> CleanupRegistry;
    public static Dictionary<string, Action<GameObject>> EditorPlaceButtonRegistry;

    public static void Build()
    {
        Registry = new Dictionary<string, Func<SerialLevelObject, GameObject[]>>();
        PlaceDefaultRegistry = new Dictionary<string, Func<GameObject[]>>();
        CleanupRegistry = new Dictionary<string, Action>();
        EditorPlaceButtonRegistry = new Dictionary<string, Action<GameObject>>();

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

            var placeDefaultMethod = type.GetMethod(
                nameof(IBaseLevelObject.PlaceDefault),
                BindingFlags.Public | BindingFlags.Static);

            var cleanMethod = type.GetMethod(
                nameof(IBaseLevelObject.CleanScene),
                BindingFlags.Public | BindingFlags.Static);

            var applyEditorPlaceButtonsMethod = type.GetMethod(
                nameof(IBaseLevelObject.ApplyEditorPlaceButtons),
                BindingFlags.Public | BindingFlags.Static);

            Registry[attribute.Name] = serial =>
                (GameObject[])placeMethod!.Invoke(null, new object[] { serial })!;
                
            PlaceDefaultRegistry[attribute.Name] = () =>
                (GameObject[])placeDefaultMethod!.Invoke(null, null)!;

            CleanupRegistry[attribute.Name] = () =>
                cleanMethod!.Invoke(null, null);

            EditorPlaceButtonRegistry[attribute.Name] = gameObject =>
                applyEditorPlaceButtonsMethod?.Invoke(null, new object[] { gameObject });
            
        }
    }
}