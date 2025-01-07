using Sirenix.OdinInspector;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameFieldConfig", menuName = "Configs/GameFieldConfig")]
public class GameFieldConfig : ScriptableObject
{
    [SerializeField, MinMaxSlider(0, 2000)] private Vector2Int _AICooldownRange = new Vector2Int(200, 500);
    [SerializeField, Range(0, 5000)] private int restartGameCooldown = 1000;

    public Vector2Int AICooldownRange => _AICooldownRange;
    public int RestartGameCooldown => restartGameCooldown;
}