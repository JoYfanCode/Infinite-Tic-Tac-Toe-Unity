using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    [SerializeField] private SaveSystem saveLoader;
    [SerializeField] private AudioSystem audioSystem;

    public override void InstallBindings()
    {
        Container.Bind<SaveSystem>().FromInstance(saveLoader).AsSingle().NonLazy();
        Container.Bind<AudioSystem>().FromInstance(audioSystem).AsSingle().NonLazy();
    }
}
