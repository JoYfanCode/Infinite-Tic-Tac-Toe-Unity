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
    [SerializeField] private Vector2 normalScaleValue = Vector2.one;

    private Vector2 normalScale;
    private Vector2 maxScale;

    private float radiusSize = 1.1f;
    private float animationTime = 0.1f;

    private Button _button;
    private RectTransform _rect;
    private PointerEventData _eventData;

    private void Awake()
    {
        if (IsMobile())
            enabled = false;

        if (isSetScale)
        {
            normalScale = normalScaleValue;
        }
        else
        {
            normalScale = transform.localScale;
        }

        maxScale = new Vector2(normalScale.x * radiusSize, normalScale.y * radiusSize);

        _button = GetComponent<Button>();
        _rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (IsMobile())
            enabled = false;
    }

    void Update()
    {
        if (_button.interactable == false || _eventData == null)
            return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, _eventData.position, _eventData.pressEventCamera, out localPoint);

        Vector2 size = _rect.sizeDelta;

        float uvX = (localPoint.x + size.x / 2) / size.x;
        float uvY = (localPoint.y + size.y / 2) / size.y;

        if (uvX >= 0 && uvY >= 0 && uvX <= 1 && uvY <= 1)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, maxScale, Time.deltaTime / animationTime);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, normalScale, Time.deltaTime / animationTime);
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        _eventData = eventData;
    }

    bool IsMobile()
    {
        return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }
}
