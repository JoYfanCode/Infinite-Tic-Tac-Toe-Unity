using UnityEngine;
using System.Collections;

public class Shaker : MonoBehaviour
{
    [SerializeField] private float _downscaleSpeed = 5f;
    [SerializeField] private float _radius = 1.25f;

    [SerializeField] private bool isSetScale = false;
    [SerializeField] private Vector2 _normalScaleValue = Vector2.one;

    private Vector2 _maxScale;
    private Vector2 _normalScale;

    private bool isPulse = false;

    private const float EPSILON = 0.01f;

    private void Start()
    {
        if (isSetScale)
        {
            _normalScale = _normalScaleValue;
        }
        else
        {
            _normalScale = transform.localScale;
        }

        _maxScale = new Vector2(_normalScale.x * _radius, _normalScale.y * _radius);
    }

    private void Update()
    {
        if (isPulse)
        {
            float step = _downscaleSpeed * Time.deltaTime;
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
