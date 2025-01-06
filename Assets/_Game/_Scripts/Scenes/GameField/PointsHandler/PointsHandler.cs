using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsHandler : MonoBehaviour
{
    [BoxGroup("Parameters")]

    [SerializeField] private int countPoints = 5;
    [SerializeField] private float nextPointClearCooldown = 0.1f;
    [SerializeField] private Color colorOff = Color.gray;
    [SerializeField] private Color colorOn = Color.white;

    [BoxGroup("Objects")]

    [SerializeField] private Point pointPrefab;
    [SerializeField] private List<Point> points;

    private int countPointsOn = 0;

    [Button]
    private void CreatePoints()
    {
        for (int i = 0; i < countPoints; ++i)
        {
            points.Add(Instantiate(pointPrefab, transform));
            points[^1].Image.color = colorOff;
        }
    }

    [Button]
    private void ClearPoints()
    {
        int countPoints = points.Count;

        for (int i = 0; i < countPoints; i++)
        {
            DestroyImmediate(points[0].gameObject);
            points.RemoveAt(0);
        }
    }

    public void SetPoints(int countTurnOnPoints)
    {
        if (countTurnOnPoints > points.Count) throw new Exception("Points don't enough");

        if (countTurnOnPoints != countPointsOn && countTurnOnPoints == 0)
        {
            StartCoroutine(ClearTurnedOnPointsAnimation());
        }
        else
        {
            for (int i = 0; i < countTurnOnPoints; i++)
            {
                points[i].Image.color = colorOn;
            }

            for (int i = countTurnOnPoints; i < points.Count; i++)
            {
                points[i].Image.color = colorOff;
            }
        }

        if (countTurnOnPoints > countPointsOn)
        {
            for (int i = 0; i < countTurnOnPoints; i++)
            {
                points[i].Shaker.Shake();
            }
        }

        countPointsOn = countTurnOnPoints;
    }

    private IEnumerator ClearTurnedOnPointsAnimation()
    {
        for (int i = points.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(nextPointClearCooldown);

            points[i].Image.color = colorOff;
            points[i].Shaker.Shake();
        }
    }
}
