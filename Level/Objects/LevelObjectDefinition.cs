using UnityEngine;

public abstract class LevelObjectDefinition
{
    public abstract void CleanScene();
    public abstract BaseLevelObject Place(SerialLevelObject serialLevelObject);
    public abstract BaseLevelObject PlaceDefault();
}