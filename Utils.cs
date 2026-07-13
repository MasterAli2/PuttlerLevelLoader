using Il2Cpp;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine.UI;
using UnityEngine.Events;
using Il2CppTMPro;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;

public static class Utils
{
    public static JsonElement ToJsonElement<T>(T value)
    {
        var json = JsonSerializer.Serialize(value);
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.Clone(); // keep it alive
    }
    public static void ResizeX(Transform t, float newWidth, bool anchorLeft, float meshMultiplier = 1f)
    {
        float oldWidth = t.localScale.x;
        float delta = newWidth - oldWidth;
        t.localScale = new Vector3(newWidth, t.localScale.y, t.localScale.z);

        float offset = (delta * meshMultiplier) * 0.5f;
        t.localPosition += new Vector3(anchorLeft ? offset : -offset, 0f, 0f);
    }

    public static void ResizeY(Transform t, float newHeight, bool anchorBottom, float meshMultiplier = 1f)
    {
        float oldHeight = t.localScale.y;
        float delta = newHeight - oldHeight;
        t.localScale = new Vector3(t.localScale.x, newHeight, t.localScale.z);

        float offset = (delta * meshMultiplier) * 0.5f;
        t.localPosition += new Vector3(0f, anchorBottom ? offset : -offset, 0f);
    }
    public static void HideAndDisable(GameObject obj)
    {
        if (obj == null) return;

        obj.SetActive(false);
        obj.hideFlags = HideFlags.HideAndDontSave;
    }
    public static GameObject spawnPrefabWithValues(GameObject prefab, Vector3 position = default(Vector3), Vector3 size = default(Vector3), float rotation = 0f)
    {
        if (size == default(Vector3))
        {
            size = Vector3.one;
        }
        GameObject obj = Utils.spawnPrefab(prefab);

        obj.transform.position = position;

        obj.transform.eulerAngles = new Vector3(0f, 0f, rotation);
        obj.transform.localScale = size;

        return obj;
    }
    public static GameObject spawnPrefab(GameObject prefab)
    {
        GameObject obj = GameObject.Instantiate(prefab);
        obj.hideFlags = HideFlags.None;
        obj.SetActive(true);

        return obj;
    }

    public static Vector3 pointerWorldPos()
    {
        Vector3 mouseScreenPos = Input.mousePosition;

        Vector3 mouseWorldPos = Camera.current.ScreenToWorldPoint(mouseScreenPos);

        mouseWorldPos.z = 0f;

        return mouseWorldPos;
    }

    public static void disableCollision(Collider2D collider)
    {
        collider.excludeLayers = ~0;
        collider.includeLayers = 0;
        collider.contactCaptureLayers = 0;
        collider.callbackLayers = 0;
    }
    
}
