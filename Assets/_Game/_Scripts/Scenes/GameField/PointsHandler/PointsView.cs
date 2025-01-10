using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class PointsView : MonoBehaviour
{
    public IReadOnlyList<Point> Points => _points;
    public void SetPointPrefab(Point point) => _pointPrefab = point;

    [SerializeField] float nextPointClearCooldown = 0.1f;
    [SerializeField] Color colorOff = Color.gray;
    [SerializeField] Color colorOn = Color.white;

    Point _pointPrefab;
    List<Point> _points = new();
    int _countPointsOn = 0;

    public void SetPointsOn(int countTurnOnPoints)
    {
        if (countTurnOnPoints > _points.Count) throw new Exception("Points don't enough");

        if (countTurnOnPoints != _countPointsOn && countTurnOnPoints == 0)
        {
            StartCoroutine(ClearTurnedOnPointsAnimation());
        }
        else
        {
            for (int i = 0; i < countTurnOnPoints; i++)
            {
                _points[i].Image.color = colorOn;
            }

            for (int i = countTurnOnPoints; i < _points.Count; i++)
            {
                _points[i].Image.color = colorOff;
            }
        }

        if (countTurnOnPoints > _countPointsOn)
        {
            for (int i = 0; i < countTurnOnPoints; i++)
            {
                _points[i].Shaker.Shake();
            }
        }

        _countPointsOn = countTurnOnPoints;
    }

    public void CreatePoints(int countMaxPoints)
    {
        for (int i = 0; i < countMaxPoints; ++i)
        {
            _points.Add(Instantiate(_pointPrefab, transform));
            _points[^1].Image.color = colorOff;
        }
    }

    [Button]
    void ClearPoints()
    {
        int countPoints = _points.Count;

        for (int i = 0; i < countPoints; i++)
        {
            DestroyImmediate(_points[0].gameObject);
            _points.RemoveAt(0);
        }
    }

    IEnumerator ClearTurnedOnPointsAnimation()
    {
        for (int i = _points.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(nextPointClearCooldown);

            _points[i].Image.color = colorOff;
            _points[i].Shaker.Shake();
        }
    }
}
