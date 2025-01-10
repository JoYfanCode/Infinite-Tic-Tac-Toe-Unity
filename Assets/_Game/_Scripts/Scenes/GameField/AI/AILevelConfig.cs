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
    public AIConfig AIConfig(int index) => AILevelConfigsList[index];
    public int Count => AILevelConfigsList.Count;
    public AIAlgorithm Algorithm => algorithm;

    [SerializeField, EnumToggleButtons] AIAlgorithm algorithm;
    [SerializeField] List<AIConfig> AILevelConfigsList;
}
