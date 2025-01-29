public interface ISaveLoader
{
    void SaveGame(IGameRepository gameRepository);
    void LoadGame(IGameRepository gameRepository);
}
