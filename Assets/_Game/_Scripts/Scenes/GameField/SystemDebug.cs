using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class SystemDebug : MonoBehaviour
{
    [Inject]
    [ShowInInspector, HideInEditorMode]
    [SerializeField] GameplayModel gameplayModel;
}