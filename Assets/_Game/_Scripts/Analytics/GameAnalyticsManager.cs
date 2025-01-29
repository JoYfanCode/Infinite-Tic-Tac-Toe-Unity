using GameAnalyticsSDK;
using UnityEngine;

public class GameAnalyticsManager : MonoBehaviour
{
    public static GameAnalyticsManager inst;

    public void Initialize()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
            GameAnalytics.Initialize();
            print("Game Analytics Init");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnLevelStarted(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level - " + level);
        print("Level - " + level);
    }

    public void OnLevelCompleted(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level - " + level);
        print("Level - " + level);
    }

    public void OnLevelFailed(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level - " + level);
        print("Level - " + level);
    }
}
