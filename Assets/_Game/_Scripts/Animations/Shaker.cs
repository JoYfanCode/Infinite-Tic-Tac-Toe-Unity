using Sirenix.OdinInspector;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [SerializeField] float downscaleSpeed = 5f;
    [SerializeField] float radius = 1.25f;
    [SerializeField] bool isSetScale = false;
    [SerializeField, ShowIf("isSetScale")] Vector2 normalScaleValue = Vector2.one;

    Vector2 _maxScale;
    Vector2 _normalScale;
    bool _isPulse = false;

    const float EPSILON = 0.01f;

    void Start()
    {
        _normalScale = isSetScale ? normalScaleValue : transform.localScale;
        _maxScale = new Vector2(_normalScale.x * radius, _normalScale.y * radius);
    }

    public void Shake()
    {
        transform.localScale = _maxScale;
        _isPulse = true;
    }

    void Update()
    {
        if (_isPulse)
        {
            float step = downscaleSpeed * Time.deltaTime;
            transform.localScale = Vector2.MoveTowards(transform.localScale, _normalScale, step);

            if (Vector2.Distance(transform.localScale, _normalScale) < EPSILON)
                _isPulse = false;
        }
    }

}
