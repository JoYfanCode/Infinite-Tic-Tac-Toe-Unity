using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ImageFader : MonoBehaviour
{
    [SerializeField] float fadeDuration = 0.2f;
    [SerializeField] float waitDuration = 0f;

    CanvasGroup _canvasGroup;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Fade()
    {
        _canvasGroup.alpha = 1f;
        StartCoroutine(FadeCoroutine());
    }

    IEnumerator FadeCoroutine()
    {
        yield return new WaitForSeconds(waitDuration);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            _canvasGroup.alpha = 1 - elapsedTime / fadeDuration;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}