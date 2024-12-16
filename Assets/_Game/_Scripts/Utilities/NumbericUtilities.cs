using UnityEngine;

internal static class NumbericUtilities
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
}
