using System.Threading.Tasks;
using UnityEngine;

public class SceneChangerAnimation : MonoBehaviour
{
    public static SceneChangerAnimation inst;

    [SerializeField] float duration = 0.2f;
    [SerializeField] int delayMilisecDuration = 10;
    [SerializeField] CanvasGroup blackScreen;

    public void Init()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

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

        blackScreen.alpha = 1;
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

        blackScreen.alpha = 0;
    }
}