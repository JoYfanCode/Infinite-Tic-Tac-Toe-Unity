using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static bool RollChance(int value)
    {
        int next = Random.Range(0, 100);
        return next < value;
    }

    public static bool RollChance(float value)
    {
        float next = Random.Range(0f, 100f);
        return next <= value;
    }

    public static IReadOnlyList<GameObject> ConverToGameObjects<T>(IReadOnlyList<T> list) where T : MonoBehaviour
    {
        List<GameObject> gameObjectsList = new();

        foreach (T obj in list)
        {
            gameObjectsList.Add(obj.gameObject);
        }

        return gameObjectsList;
    }
}
