using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

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

        if (_gameState.TryGetValue(key, out var jsonData))
        {
            data = JsonConvert.DeserializeObject<T>(jsonData);
            return true;
        }
        else
        {
            data = default;
            return false;
        }
    }

    public void SaveData()
    {
        string jsonGameState = JsonConvert.SerializeObject(_gameState);
        PlayerPrefs.SetString(GAME_STATE_SAVE_KEY, jsonGameState);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(GAME_STATE_SAVE_KEY))
        {
            string jsonGameState = PlayerPrefs.GetString(GAME_STATE_SAVE_KEY);
            var loadedState = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonGameState);

            if (loadedState != null)
            {
                _gameState = loadedState;
            }
        }
    }
}