using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class SystemDebug : MonoBehaviour
{
    [Inject(Id = "Circles")]
    [ShowInInspector, HideInEditorMode]
    private PointsHandler circlesPointsHandler;

    [Inject(Id = "Crosses")]
    [ShowInInspector, HideInEditorMode]
    private PointsHandler crossesPointsHandler;
}