using UnityEngine;
using Zenject;

public class GameFieldInstaller : MonoInstaller
{
    [SerializeField] GameplayView gameplayView;
    [SerializeField] PointsModel circlesPointsModel;
    [SerializeField] PointsModel crossesPointsModel;
    [SerializeField] PointsView circlesPointsView;
    [SerializeField] PointsView crossesPointsView;

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
