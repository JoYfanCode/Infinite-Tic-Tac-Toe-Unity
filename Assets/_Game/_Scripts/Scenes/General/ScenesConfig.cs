using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenesConfig", menuName = "Configs/ScenesConfig")]
public class ScenesConfig : ScriptableObject
{
    [SerializeField, FilePath(Extensions = ".unity")] string menu;
    [SerializeField, FilePath(Extensions = ".unity")] string gameField;
    [SerializeField, FilePath(Extensions = ".unity")] string load;

    public string Menu => menu;
    public string GameField => gameField;
    public string Load => load;
}
