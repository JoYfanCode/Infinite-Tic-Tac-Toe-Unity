using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum AIAlgorithm
{
    OneTurn,
    MiniMax,
}

[CreateAssetMenu(fileName = "AILevelConfigs", menuName = "Configs/AILevelConfigs")]
[Serializable]
public class AILevelConfigs : ScriptableObject
{
    [SerializeField, EnumToggleButtons] private AIAlgorithm algorithm;
    [SerializeField] private List<AIConfig> AILevelConfigsList;

    public AIConfig AIConfig(int index) => AILevelConfigsList[index];
    public int Count => AILevelConfigsList.Count;
    public AIAlgorithm Algorithm => algorithm;
}
