using Harmony;
using MelonLoader;
using MelonLoader.TinyJSON;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

[MelonLoader.RegisterTypeInIl2Cpp]
public class EditorManager : MonoBehaviour
{
    public EditorManager(IntPtr ptr) : base(ptr) {}

    void Awake()
    {
        gameObject.AddComponent<EditorUI>();
        gameObject.AddComponent<EditorToolController>();

    }
    
}