using UnityEngine;
using Zenject;

internal class UIInstaller : MonoInstaller
{
    [SerializeField] private PointsHandler circlesPointHandler;
    [SerializeField] private PointsHandler crossesPointHandler;

    public override void InstallBindings()
    {
        Container
            .Bind<PointsHandler>()
            .WithId("Circles")
            .FromInstance(circlesPointHandler)
            .AsTransient()
            .NonLazy();

        Container
            .Bind<PointsHandler>()
            .WithId("Crosses")
            .FromInstance(crossesPointHandler)
            .AsTransient()
            .NonLazy();
    }
}
