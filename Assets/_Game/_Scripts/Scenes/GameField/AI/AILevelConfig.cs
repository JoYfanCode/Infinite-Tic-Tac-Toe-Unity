using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AILevelConfigs", menuName = "Configs/AILevelConfigs")]
[Serializable]
public class AILevelConfigs : ScriptableObject
{
    [SerializeField] private List<AIConfig> AILevelConfigsList;

    public AIConfig AIConfig(int index) => AILevelConfigsList[index];
}
