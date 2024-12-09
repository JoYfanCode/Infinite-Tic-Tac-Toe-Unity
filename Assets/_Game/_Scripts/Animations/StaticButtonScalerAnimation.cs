using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using System;

[RequireComponent(typeof(Button))]
public class StaticButtonScalerAnimation : MonoBehaviour, IPointerMoveHandler
{
    [SerializeField] private bool isSetScale = false;
    [SerializeField] private Vector2 _normalScaleValue = Vector2.one;

    private Vector2 _normalScale;
    private Vector2 _maxScale;

    private float _radiusSize = 1.1f;
    private float _animationTime = 0.1f;

    private Button _button;
    private RectTransform _rect;
    private PointerEventData _eventData;

    private void Awake()
    {
        if (isSetScale)
        {
            _normalScale = _normalScaleValue;
        }
        else
        {
            _normalScale = transform.localScale;
        }

        _maxScale = new Vector2(_normalScale.x * _radiusSize, _normalScale.y * _radiusSize);

        _button = GetComponent<Button>();
        _rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        //this.enabled = false;

        if (_button.interactable == false || _eventData == null)
            return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, _eventData.position, _eventData.pressEventCamera, out localPoint);

        Vector2 size = _rect.sizeDelta;

        float uvX = (localPoint.x + size.x / 2) / size.x;
        float uvY = (localPoint.y + size.y / 2) / size.y;

        if (uvX >= 0 && uvY >= 0 && uvX <= 1 && uvY <= 1)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _maxScale, Time.deltaTime / _animationTime);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _normalScale, Time.deltaTime / _animationTime);
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        _eventData = eventData;
    }
}
