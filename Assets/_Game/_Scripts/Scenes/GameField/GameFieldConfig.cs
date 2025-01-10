using Sirenix.OdinInspector;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameFieldConfig", menuName = "Configs/GameFieldConfig")]
public class GameFieldConfig : ScriptableObject
{
    public Vector2Int AICooldownRange => _AICooldownRange;
    public int RestartGameCooldown => restartGameCooldown;

    [SerializeField, MinMaxSlider(0, 2000)] Vector2Int _AICooldownRange = new Vector2Int(200, 500);
    [SerializeField, Range(0, 5000)] int restartGameCooldown = 1000;
}