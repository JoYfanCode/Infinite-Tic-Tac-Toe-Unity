using UnityEngine;
using System.Collections;
using VInspector;

public class Shaker : MonoBehaviour
{
    [SerializeField] private float downscaleSpeed = 5f;
    [SerializeField] private float radius = 1.25f;

    [SerializeField] private bool isSetScale = false;

    [ShowIf("isSetScale")]
    [SerializeField] private Vector2 normalScaleValue = Vector2.one;
    [EndIf]

    private Vector2 _maxScale;
    private Vector2 _normalScale;

    private bool isPulse = false;

    private const float EPSILON = 0.01f;

    private void Start()
    {
        if (isSetScale)
        {
            _normalScale = normalScaleValue;
        }
        else
        {
            _normalScale = transform.localScale;
        }

        _maxScale = new Vector2(_normalScale.x * radius, _normalScale.y * radius);
    }

    private void Update()
    {
        if (isPulse)
        {
            float step = downscaleSpeed * Time.deltaTime;
            transform.localScale = Vector2.MoveTowards(transform.localScale, _normalScale, step);

            if (Vector2.Distance(transform.localScale, _normalScale) < EPSILON)
                isPulse = false;
        }
    }

    public void Shake()
    {
        transform.localScale = _maxScale;
        isPulse = true;
    }
}
