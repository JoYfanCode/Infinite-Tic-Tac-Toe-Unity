using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class SceneChangerAnimation : MonoBehaviour
{
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private int delayMilisecDuration = 10;
    [SerializeField] private CanvasGroup blackScreen;

    public async Task AppearAsync()
    {
        blackScreen.gameObject.SetActive(true);
        blackScreen.alpha = 0f;

        float startTime = Time.time;
        float timeDx = 0f;

        while (timeDx < duration)
        {
            blackScreen.alpha = timeDx / duration;
            await Task.Delay(delayMilisecDuration);
            timeDx = Time.time - startTime;
        }
    }

    public async Task FadeAsync()
    {
        blackScreen.gameObject.SetActive(true);
        blackScreen.alpha = 1f;

        float startTime = Time.time;
        float timeDx = 0f;

        while (timeDx < duration)
        {
            blackScreen.alpha = 1 - timeDx / duration;
            await Task.Delay(delayMilisecDuration);
            timeDx = Time.time - startTime;
        }
    }
}