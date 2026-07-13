using Harmony;
using MelonLoader;
using MelonLoader.TinyJSON;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

[MelonLoader.RegisterTypeInIl2Cpp]
public class EditorManager : MonoBehaviour
{
    public static EditorManager Instance {get; private set;}

    public bool inEditor = false;
    public bool isActive = false;

    public EditorManager(IntPtr ptr) : base(ptr) {}

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        gameObject.AddComponent<EditorUI>();
        gameObject.AddComponent<EditorToolController>();
        gameObject.AddComponent<EditorCamera>();

    }
    
    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}