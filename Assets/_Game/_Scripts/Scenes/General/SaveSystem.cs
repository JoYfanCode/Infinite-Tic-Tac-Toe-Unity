using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem inst;

    const string COUNT_COMPLETED_LEVELS = "CountCompletedLevels";

    public void Init()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(inst);
        }

        LoadCountCompletedLevels();
    }

    void OnApplicationQuit()
    {
        SaveCountCompletedLevels();
    }

    public static void SaveCountCompletedLevels()
    {
        PlayerPrefs.SetInt(COUNT_COMPLETED_LEVELS, SetUp.CountCompletedLevels);
        PlayerPrefs.Save();
    }

    public static void LoadCountCompletedLevels()
    {
        if (PlayerPrefs.HasKey(COUNT_COMPLETED_LEVELS))
        {
            SetUp.CountCompletedLevels = PlayerPrefs.GetInt(COUNT_COMPLETED_LEVELS);
        }
        else
        {
            SetUp.CountCompletedLevels = 0;
        }
    }
}
