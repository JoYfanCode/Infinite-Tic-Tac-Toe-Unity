public abstract class SaveLoader<TData, TService> : ISaveLoader where TService : class
{
    public void SaveGame(IGameRepository gameRepository)
    {
        var service = ServiceLocator.GetService<TService>();

        var data = ConvertToData(service);
        gameRepository.SetData(data);
        gameRepository.SaveData();
    }

    public void LoadGame(IGameRepository gameRepository)
    {
        var service = ServiceLocator.GetService<TService>();
        gameRepository.LoadData();

        if (gameRepository.TryGetData(out TData data))
        {
            SetupData(data, service);
        }
        else
        {
            SetupDefaultData(service);
        }
    }

    protected abstract void SetupDefaultData(TService service);
    protected abstract TData ConvertToData(TService service);
    protected abstract void SetupData(TData data, TService service);
}