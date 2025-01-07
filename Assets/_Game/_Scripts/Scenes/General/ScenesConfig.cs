using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenesConfig", menuName = "Configs/ScenesConfig")]
public class ScenesConfig : ScriptableObject
{
    [SerializeField, FilePath(Extensions = ".unity")] private string menu;
    [SerializeField, FilePath(Extensions = ".unity")] private string gameField;

    public string Menu => menu;
    public string GameField => gameField;
}
