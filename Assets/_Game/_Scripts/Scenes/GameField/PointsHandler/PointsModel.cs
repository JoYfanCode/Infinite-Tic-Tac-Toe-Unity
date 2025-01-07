using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public sealed class PointsModel
{
    public event Action<int> OnPointsOnChanged;

    [SerializeField, HideInPlayMode] private int countMaxPoints = 5;
    [SerializeField] private Point pointPrefab;

    private int _countPointsOn = 0;

    public Point PointPrefab => pointPrefab;
    public int CountMaxPoints => countMaxPoints;
    public int CountPointsOn => _countPointsOn;
    public bool IsMax => _countPointsOn == countMaxPoints;

    [Button(ButtonSizes.Large)]
    public void AddPointOn()
    {
        if (_countPointsOn < countMaxPoints) _countPointsOn++;
        OnPointsOnChanged?.Invoke(_countPointsOn);
    }

    [Button(ButtonSizes.Large)]
    public void SetPointsOn(int count)
    {
        if (count <= countMaxPoints) _countPointsOn = count;
        OnPointsOnChanged?.Invoke(_countPointsOn);
    }
}
