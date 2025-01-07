using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AILevelsConfigs", menuName = "Configs/AILevelsConfigs")]
public class AILevelsConfigs : ScriptableObject
{
    [SerializeField] private List<AILevelConfigs> AILevelsConfigsList;

    public AILevelConfigs AILevelConfigs(int index) => AILevelsConfigsList[index];
}
