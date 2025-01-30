using GameAnalyticsSDK;
using UnityEngine;

public class GameAnalyticsManager : MonoBehaviour
{
    public static GameAnalyticsManager inst;

    private float _startTime;

    public void Initialize()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        GameAnalytics.Initialize();
        GameAnalytics.NewDesignEvent("Game:Start");
        _startTime = Time.time;
    }

    public void OnLevelStarted(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level", level.ToString());
    }

    public void OnLevelCompleted(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level", level.ToString());
    }

    public void OnLevelFailed(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level", level.ToString());
    }

    public void OnApplicationQuit()
    {
        float sessionDuration = Time.time - _startTime;
        GameAnalytics.NewDesignEvent("Session:Duration", sessionDuration / 60);
    }
}
