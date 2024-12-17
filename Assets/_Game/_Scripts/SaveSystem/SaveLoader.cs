using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AIDifficultyData
{
    public List<AIDifficultyEntry> entries = new List<AIDifficultyEntry>();
}

[System.Serializable]
public class AIDifficultyEntry
{
    public AIDifficulties difficulty;
    public bool completed;
}

internal class SaveLoader : MonoBehaviour
{
    public static SaveLoader inst;

    public void Init()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(inst);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadAIDifficulties();
    }

    private void OnApplicationQuit()
    {
        SaveAIDifficulties();
    }

    public static void SaveAIDifficulties()
    {
        AIDifficultyData data = new AIDifficultyData();

        foreach (var entry in SetUp.AIDifficultiesCompleted)
        {
            data.entries.Add(new AIDifficultyEntry { difficulty = entry.Key, completed = entry.Value });
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("AIDifficulties", json);
        PlayerPrefs.Save();
    }

    public static void LoadAIDifficulties()
    {
        if (PlayerPrefs.HasKey("AIDifficulties"))
        {
            string json = PlayerPrefs.GetString("AIDifficulties");
            AIDifficultyData data = JsonUtility.FromJson<AIDifficultyData>(json);

            SetUp.AIDifficultiesCompleted.Clear();

            foreach (var entry in data.entries)
            {
                SetUp.AIDifficultiesCompleted[entry.difficulty] = entry.completed;
            }
        }
    }
}
