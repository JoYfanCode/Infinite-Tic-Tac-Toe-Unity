using Newtonsoft.Json;
using System.Collections.Generic;

public class GameRepository : IGameRepository
{
    private const string GAME_STATE_SAVE_KEY = "Save_Key";

    private Dictionary<string, string> _gameState = new();

    public void SetData<T>(T data)
    {
        string key = typeof(T).ToString();
        string jsonData = JsonConvert.SerializeObject(data);
        _gameState[key] = jsonData;
    }

    public bool TryGetData<T>(out T data)
    {
        string key = typeof(T).ToString();

        if (!_gameState.TryGetValue(key, out var jsonData))
        {
            data = default;
            return false;
        }

        data = JsonConvert.DeserializeObject<T>(jsonData);
        return false;
    }

    public void SaveState()
    {
        var jsonGameState = JsonConvert.SerializeObject(_gameState);
        _gameState = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonGameState);
    }
}