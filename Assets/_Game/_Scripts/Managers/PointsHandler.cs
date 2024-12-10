using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class PointsHandler : MonoBehaviour
{
    [Tab("Parameters")]

    [SerializeField] private int _countPoints = 5;
    [SerializeField] private Color _colorOff = Color.gray;
    [SerializeField] private Color _colorOn = Color.white;

    [Tab("Objects")]

    [SerializeField] private Image _pointPrefab;

    private List<Image> _points = new();
    private List<Shaker> _pointsShakers = new();

    public void Init()
    {
        for (int i = 0; i < _countPoints; ++i)
        {
            _points.Add(Instantiate(_pointPrefab, transform));
            _pointsShakers.Add(_points[^1].GetComponent<Shaker>());
            _points[^1].color = _colorOff;
        }
    }

    public void SetPoints(int countTurnOnPoints)
    {
        if (countTurnOnPoints > _points.Count)
        {
            throw new Exception("Points don't enough");
        }

        for (int i = 0; i < countTurnOnPoints; i++)
        {
            _points[i].color = _colorOn;

            if (i == countTurnOnPoints - 1)
                _pointsShakers[i].Shake();
        }

        for (int i = countTurnOnPoints; i < _points.Count; i++)
        {
            _points[i].color = _colorOff;
        }
    }
}
