using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class SaveLoadManager : MonoBehaviour
{
    [Inject] private ISaveLoader[] _saveLoaders;
    [Inject] private GameRepository _gameRepository;

    [Button]
    public void SaveGame()
    {
        foreach (var saveLoader in _saveLoaders)
        {
            saveLoader.SaveGame(_gameRepository);
        }

        print("Save Game");
    }

    [Button]
    public void LoadGame()
    {
        foreach (var saveLoader in _saveLoaders)
        {
            saveLoader.LoadGame(_gameRepository);
        }

        print("Load Game");
    }

    public void OnApplicationQuit()
    {
        SaveGame();
    }
}
