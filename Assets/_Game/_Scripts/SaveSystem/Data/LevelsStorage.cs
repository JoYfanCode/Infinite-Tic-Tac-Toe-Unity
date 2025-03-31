public class LevelsData
{
    public int CompletedLevels;
}

public class LevelsStorage
{
    public int CompletedLevels { get; private set; }

    public void SetupCompletedLevels(int amount)
    {
        CompletedLevels = amount;
    }

    public void LevelCompleted()
    {
        CompletedLevels++;
    }
}