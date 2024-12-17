using System.Collections.Generic;

internal static class SetUp
{
    public static GameModes GameMode;
    public static AIDifficulties AIDifficulty;

    public static Dictionary<AIDifficulties, bool> AIDifficultiesCompleted = new Dictionary<AIDifficulties, bool>()
    {
        { AIDifficulties.NORMAL, false },
        { AIDifficulties.HARD, false },
        { AIDifficulties.VERY_HARD, false },
    };

    public static bool isOpenedNewDifficulty = false;
}
