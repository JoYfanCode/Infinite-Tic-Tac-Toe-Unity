public abstract class SaveLoader<TData, TService> : ISaveLoader where TService : class
{
    public void SaveGame(IGameRepository gameRepository)
    {
        var service = ServiceLocator.GetService<TService>();
        var data = ConvertToData(service);
        gameRepository.SetData(data);
    }

    public void LoadGame(IGameRepository gameRepository)
    {
        var service = ServiceLocator.GetService<TService>();

        if (!gameRepository.TryGetData(out TData data))
        {
            SetupDefaultData(service);
        }
        else
        {
            SetupData(data, service);
        }
    }

    protected abstract void SetupDefaultData(TService service);
    protected abstract TData ConvertToData(TService service);
    protected abstract void SetupData(TData data, TService service);
}