using UnityEngine;
using Zenject;

public class GameFieldInstaller : MonoInstaller
{
    [SerializeField] private GameplayView gameplayView;
    [SerializeField] private PointsModel circlesPointsModel;
    [SerializeField] private PointsModel crossesPointsModel;
    [SerializeField] private PointsView circlesPointsView;
    [SerializeField] private PointsView crossesPointsView;

    public override void InstallBindings()
    {
        Container.Bind<PointsView>().WithId("CirclesPointsView").FromInstance(circlesPointsView).AsTransient().NonLazy();
        Container.Bind<PointsView>().WithId("CrossesPointsView").FromInstance(crossesPointsView).AsTransient().NonLazy();

        Container.Bind<GameplayModel>().AsSingle().WithArguments(circlesPointsModel, crossesPointsModel).NonLazy();
        Container.Bind<GameplayView>().FromInstance(gameplayView).AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<PointsHandler>().AsSingle()
            .WithArguments(circlesPointsModel, crossesPointsModel, circlesPointsView, crossesPointsView).NonLazy();
    }
}
