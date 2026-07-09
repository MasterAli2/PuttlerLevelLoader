using UnityEngine;

public interface IBaseLevelObject
{
    public abstract static void CleanScene();
    public abstract static GameObject Place(SerialLevelObject serialLevelObject);

}