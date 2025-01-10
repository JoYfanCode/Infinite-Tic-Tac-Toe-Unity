using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StaticButtonScalerAnimation : MonoBehaviour, IPointerMoveHandler
{
    [SerializeField] bool isSetScale = false;

    [ShowIf("isSetScale")]
    [SerializeField] Vector2 normalScaleValue = Vector2.one;

    Button _button;
    RectTransform _rect;
    PointerEventData _eventData;
    Vector2 _maxScale;
    Vector2 _normalScale;
    float _radiusSize = 1.1f;
    float _animationTime = 0.1f;

    void Awake()
    {
        if (IsMobile())
            enabled = false;

        _normalScale = isSetScale ? normalScaleValue : transform.localScale;
        _maxScale = new Vector2(_normalScale.x * _radiusSize, _normalScale.y * _radiusSize);

        _button = GetComponent<Button>();
        _rect = GetComponent<RectTransform>();
    }

    void OnEnable()
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

    bool IsMobile()
    {
        return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }
}
