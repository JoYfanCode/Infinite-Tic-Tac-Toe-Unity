using System;
using System.Collections;
using UnityEngine;

public class SceneChangerAnimation : MonoBehaviour
{
    [SerializeField] private float duration = 0.2f;   
    [SerializeField] private CanvasGroup blackScreen;

    public event Action OnFinishedAppear;
    public event Action OnFinishedFade;

    public void Appear()
    {
        blackScreen.alpha = 0f;
        blackScreen.gameObject.SetActive(true);
        StartCoroutine(AppearCoroutine());
    }

    private IEnumerator AppearCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            blackScreen.alpha = elapsedTime / duration;
            yield return null;
        }

        OnFinishedAppear?.Invoke();
    }

    public void Fade()
    {
        blackScreen.alpha = 1f;
        blackScreen.gameObject.SetActive(true);
        StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            blackScreen.alpha = 1 - elapsedTime / duration;
            yield return null;
        }

        OnFinishedFade?.Invoke();
    }
}