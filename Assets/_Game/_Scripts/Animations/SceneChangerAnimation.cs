using System;
using System.Collections;
using UnityEngine;

public class SceneChangerAnimation : MonoBehaviour
{
    [SerializeField] private float _duration = 0.5f;   
    [SerializeField] private CanvasGroup _blackScreen;

    public event Action OnFinishedAppear;
    public event Action OnFinishedFade;

    public void Appear()
    {
        _blackScreen.alpha = 0f;
        _blackScreen.gameObject.SetActive(true);
        StartCoroutine(AppearCoroutine());
    }

    private IEnumerator AppearCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            _blackScreen.alpha = elapsedTime / _duration;
            yield return null;
        }

        OnFinishedAppear?.Invoke();
    }

    public void Fade()
    {
        _blackScreen.alpha = 1f;
        _blackScreen.gameObject.SetActive(true);
        StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            _blackScreen.alpha = 1 - elapsedTime / _duration;
            yield return null;
        }

        OnFinishedFade?.Invoke();
    }
}