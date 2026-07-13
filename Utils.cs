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

    public static class RuntimeSprite
    {
        public const int BASE_TEX_SIZE = 256;
        public static Sprite _square;
        public static Sprite square {get
            {
                if (_square != null) return _square;

                Texture2D tex = Texture2D.whiteTexture;
                _square = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), tex.width);

                return _square;
            }
        }
        public static Sprite _circle;
        public static Sprite circle {get
            {
                if (_circle != null) return _circle;

                _circle = getCircleSprite();
                return _circle;
            }
        }

        public static Sprite getCircleSprite()
        {
            Texture2D texture = new Texture2D(BASE_TEX_SIZE, BASE_TEX_SIZE, TextureFormat.RGBA32, false);

            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            float center = BASE_TEX_SIZE / 2f;
            float radius = BASE_TEX_SIZE / 2f;
            
            float radiusSquared = radius * radius; 

            for (int y = 0; y < BASE_TEX_SIZE; y++)
            {
                for (int x = 0; x < BASE_TEX_SIZE; x++)
                {
                    float dx = (x + 0.5f) - center;
                    float dy = (y + 0.5f) - center;

                    float distanceSquared = (dx * dx) + (dy * dy);

                    if (distanceSquared <= radiusSquared)
                    {
                        texture.SetPixel(x, y, Color.white);
                    }
                    else
                    {
                        texture.SetPixel(x, y, Color.clear);
                    }
                }
            }

            texture.Apply();

            Sprite circleSprite = Sprite.Create(
                texture, 
                new Rect(0, 0, texture.width, texture.height), 
                new Vector2(0.5f, 0.5f), 
                texture.width                     
            );

            return circleSprite;
        }


    }
    
}
