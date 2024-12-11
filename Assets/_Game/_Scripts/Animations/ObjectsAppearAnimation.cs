using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsAppearAnimation : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private float scaleTime = 0.2f;
    [SerializeField] private float nextAppearButtonTime = 0.1f; 
    [SerializeField] private float nextDisappearButtonTime = 0.1f;
    [SerializeField] private float appearCooldown = 0.2f;

    private List<Vector3> defaultScale = new();
    private List<StaticButtonScalerAnimation> scaleAnimations = new();

    private int activeButtonsCount = 0;

    public event Action OnAppeared;
    public event Action OnDisappeared;

    public ObjectsAppearAnimation Init()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            defaultScale.Add(objects[i].transform.localScale);
            objects[i].TryGetComponent(out StaticButtonScalerAnimation anim);
            scaleAnimations.Add(anim);
            objects[i].transform.localScale = Vector3.zero;

            if (scaleAnimations[i]) scaleAnimations[i].enabled = false;
        }

        return this;
    }

    public void Appear()
    {
        activeButtonsCount = 0;

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.localScale = Vector3.zero;
            if(scaleAnimations[i]) scaleAnimations[i].enabled = false;
        }

        StartCoroutine(AppearButtonsCoroutine());
    }

    public void Disappear()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            scaleAnimations[i].enabled = false;
        }

        StartCoroutine(DisappearButtonsCoroutine());
    }

    private IEnumerator AppearButtonsCoroutine()
    {
        yield return new WaitForSeconds(appearCooldown);

        WaitForSeconds NextButtonTime = new WaitForSeconds(nextAppearButtonTime);

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.LeanScale(defaultScale[i], scaleTime).setEaseOutBack().setOnComplete(EnableScaleAnimation);
            yield return NextButtonTime;
        }

        OnAppeared?.Invoke();
    }

    private void EnableScaleAnimation()
    {
        if (scaleAnimations[activeButtonsCount]) scaleAnimations[activeButtonsCount].enabled = true;
        activeButtonsCount++;
    }

    private IEnumerator DisappearButtonsCoroutine()
    {
        WaitForSeconds NextButtonTime = new WaitForSeconds(nextDisappearButtonTime);

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.LeanScale(Vector3.zero, scaleTime).setEaseOutExpo();
            yield return NextButtonTime;
        }

        OnDisappeared?.Invoke();
    }
}