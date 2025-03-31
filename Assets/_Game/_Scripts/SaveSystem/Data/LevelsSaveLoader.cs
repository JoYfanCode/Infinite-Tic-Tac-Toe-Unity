public class LevelsSaveLoader : SaveLoader<LevelsData, LevelsStorage>
{
    const int DEFAULT_COMPLETED_LEVELS = 0;

    protected override LevelsData ConvertToData(LevelsStorage service)
    {
        return new LevelsData() { CompletedLevels = service.CompletedLevels };
    }

    protected override void SetupData(LevelsData data, LevelsStorage service)
    {
        service.SetupCompletedLevels(data.CompletedLevels);
    }

    protected override void SetupDefaultData(LevelsStorage service)
    {
        service.SetupCompletedLevels(DEFAULT_COMPLETED_LEVELS);
    }
}