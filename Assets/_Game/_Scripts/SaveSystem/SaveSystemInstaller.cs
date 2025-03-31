using UnityEngine;
using Zenject;

public class SaveSystemInstaller : MonoInstaller
{
    [SerializeField] private SaveLoadManager saveLoadManager;

    public override void InstallBindings()
    {
        Container.Bind<GameRepository>().AsSingle().NonLazy();

        Container.Bind<SaveLoadManager>().FromInstance(saveLoadManager).AsSingle().NonLazy();

        Container.Bind<ISaveLoader>().To<LevelsSaveLoader>().AsSingle().NonLazy();
    }
}