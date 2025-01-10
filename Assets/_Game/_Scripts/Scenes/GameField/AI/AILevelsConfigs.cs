using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AILevelsConfigs", menuName = "Configs/AILevelsConfigs")]
public class AILevelsConfigs : ScriptableObject
{
    public AILevelConfigs AILevelConfigs(int index) => AILevelsConfigsList[index];

    [SerializeField] List<AILevelConfigs> AILevelsConfigsList;
}
