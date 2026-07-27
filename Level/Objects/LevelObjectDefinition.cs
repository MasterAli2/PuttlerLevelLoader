using UnityEngine;

public abstract class LevelObjectDefinition
{
    public abstract void CleanScene();
    public abstract GameObject[] Place(SerialLevelObject serialLevelObject);
    public abstract GameObject[] PlaceDefault();
    public abstract void ApplyEditorPlaceButtons(GameObject gameObject);

}