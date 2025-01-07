using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    [SerializeField] private DifficultiesUnlocker difficultiesManager;
    [SerializeField] private MenuButtonsHandler menuButtonsHandler;

    public override void InstallBindings()
    {
        Container.Bind<DifficultiesUnlocker>().FromInstance(difficultiesManager).AsSingle().NonLazy();
        Container.Bind<MenuButtonsHandler>().FromInstance(menuButtonsHandler).AsSingle().NonLazy();
    }
}
