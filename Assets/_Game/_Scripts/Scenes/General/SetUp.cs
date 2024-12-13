using System.Collections.Generic;

internal static class SetUp
{
    public static Modes Mode { get; set; }

    public static Dictionary<string, bool> DifficultiesComplited = new()
    {
        { "Normal", false },
        { "Hard", false },
        { "VeryHard", false },
    };
}
