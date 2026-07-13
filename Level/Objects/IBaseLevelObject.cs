using UnityEngine;

public interface IBaseLevelObject
{
    public abstract static void CleanScene();
    public abstract static GameObject[] Place(SerialLevelObject serialLevelObject);
    public abstract static GameObject[] PlaceDefault();
    public abstract static void ApplyEditorPlaceButtons(GameObject gameObject);

}