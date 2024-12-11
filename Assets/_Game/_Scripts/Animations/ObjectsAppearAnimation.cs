using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsAppearAnimation : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objects;
    [SerializeField] private float _scaleTime = 0.2f;
    [SerializeField] private float _nextAppearButtonTime = 0.1f; 
    [SerializeField] private float _nextDisappearButtonTime = 0.1f;
    [SerializeField] private float _appearCooldown = 0.3f;

    private List<Vector3> _defaultScale = new();
    private List<StaticButtonScalerAnimation> _scaleAnimations = new();

    private int _activeButtonsCount = 0;

    public event Action OnAppeared;
    public event Action OnDisappeared;

    public ObjectsAppearAnimation Init()
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            _defaultScale.Add(_objects[i].transform.localScale);
            _objects[i].TryGetComponent(out StaticButtonScalerAnimation anim);
            _scaleAnimations.Add(anim);
            _objects[i].transform.localScale = Vector3.zero;

            if (_scaleAnimations[i]) _scaleAnimations[i].enabled = false;
        }

        return this;
    }

    public void Appear()
    {
        _activeButtonsCount = 0;

        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].transform.localScale = Vector3.zero;
            if(_scaleAnimations[i]) _scaleAnimations[i].enabled = false;
        }

        StartCoroutine(AppearButtonsCoroutine());
    }

    public void Disappear()
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            _scaleAnimations[i].enabled = false;
        }

        StartCoroutine(DisappearButtonsCoroutine());
    }

    private IEnumerator AppearButtonsCoroutine()
    {
        yield return new WaitForSeconds(_appearCooldown);

        WaitForSeconds NextButtonTime = new WaitForSeconds(_nextAppearButtonTime);

        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].transform.LeanScale(_defaultScale[i], _scaleTime).setEaseOutBack().setOnComplete(EnableScaleAnimation);
            yield return NextButtonTime;
        }

        OnAppeared?.Invoke();
    }

    private void EnableScaleAnimation()
    {
        if (_scaleAnimations[_activeButtonsCount]) _scaleAnimations[_activeButtonsCount].enabled = true;
        _activeButtonsCount++;
    }

    private IEnumerator DisappearButtonsCoroutine()
    {
        WaitForSeconds NextButtonTime = new WaitForSeconds(_nextDisappearButtonTime);

        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].transform.LeanScale(Vector3.zero, _scaleTime).setEaseOutExpo();
            yield return NextButtonTime;
        }

        OnDisappeared?.Invoke();
    }
}