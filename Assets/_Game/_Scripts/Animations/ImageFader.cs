using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ImageFader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField] private float waitDuration = 0f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>(); 
    }

    public void Fade()
    {
        canvasGroup.alpha = 1f;
        StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        yield return new WaitForSeconds(waitDuration);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = 1 - elapsedTime / fadeDuration;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}