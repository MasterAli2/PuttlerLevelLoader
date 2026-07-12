
using UnityEngine;


[MelonLoader.RegisterTypeInIl2Cpp]
public abstract class LinkedLevelObject : BaseLevelObject
{
    public List<UnityEngine.Object> links = new List<UnityEngine.Object>();

    public bool destroyed = false;

    public LinkedLevelObject(IntPtr ptr) : base(ptr) {}

    public override void OnDestroy()
    {
        if (destroyed)
            return;

        destroyed = true;

        foreach (UnityEngine.Object obj in links.ToArray())
        {
            if (obj == null)
                continue;
            
            if (obj is LinkedLevelObject linkedLevelObject)
            {
                if (linkedLevelObject.destroyed) continue;

                Destroy(linkedLevelObject.gameObject);

                continue;
            }

            Destroy(obj);
        }
    }
    
}