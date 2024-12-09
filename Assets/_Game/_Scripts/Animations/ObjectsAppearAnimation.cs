using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsAppearAnimation : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private float _scaleTime = 0.2f;
    [SerializeField] private float _nextAppearButtonTime = 0.1f;
    [SerializeField] private float _nextDisappearButtonTime = 0.1f;
    [SerializeField] private float _playOnStartCooldown = 1f;
    [SerializeField] private float _startCooldown = 0.3f;

    [SerializeField] private bool _isPlayOnStart = false;

    private List<Vector3> _defaultScale = new();
    private List<StaticButtonScalerAnimation> _scaleAnimations = new();

    private int ActiveButtonsCount = 0;

    public event Action OnAppeared;
    public event Action OnDisappeared;

    private void Awake()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _defaultScale.Add(_buttons[i].transform.localScale);
            _scaleAnimations.Add(_buttons[i].GetComponent<StaticButtonScalerAnimation>());
            _buttons[i].transform.localScale = Vector3.zero;
            _scaleAnimations[i].enabled = false;
        }
    }

    private void Start()
    {
        if (_isPlayOnStart)
            Appear(true);
    }

    public void Appear(bool isPlayOnStart)
    {
        ActiveButtonsCount = 0;

        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].transform.localScale = Vector3.zero;
            _scaleAnimations[i].enabled = false;
        }

        StartCoroutine(AppearButtonsCoroutine(isPlayOnStart));
    }

    private IEnumerator AppearButtonsCoroutine(bool isPlayOnStart = false)
    {
        if (isPlayOnStart)
            yield return new WaitForSeconds(_playOnStartCooldown);
        else
            yield return new WaitForSeconds(_startCooldown);

        WaitForSeconds NextButtonTime = new WaitForSeconds(_nextAppearButtonTime);

        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].transform.LeanScale(_defaultScale[i], _scaleTime).setEaseOutBack().setOnComplete(EnableScaleAnimation);
            yield return NextButtonTime;
        }

        OnAppeared?.Invoke();
    }

    private void EnableScaleAnimation()
    {
        _scaleAnimations[ActiveButtonsCount].enabled = true;
        ActiveButtonsCount++;
    }

    public void Disappear()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _scaleAnimations[i].enabled = false;
        }

        StartCoroutine(DisappearButtonsCoroutine());
    }

    private IEnumerator DisappearButtonsCoroutine()
    {
        WaitForSeconds NextButtonTime = new WaitForSeconds(_nextDisappearButtonTime);

        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].transform.LeanScale(Vector3.zero, _scaleTime).setEaseOutExpo();
            yield return NextButtonTime;
        }

        OnDisappeared?.Invoke();
    }
}