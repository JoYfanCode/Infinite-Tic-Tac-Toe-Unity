using UnityEngine;

public class LoadBootstrap : MonoBehaviour
{
    [SerializeField] ScenesConfig scenesConfig;
    [SerializeField] AudioSystem audioSystem;
    [SerializeField] SceneChangerAnimation sceneChangerAnimation;
    [SerializeField] GameAnalyticsManager gameAnalyticsManager;
    [SerializeField] ServicesInstaller servicesInstaller;
    [SerializeField] SaveLoadManager saveLoadManager;

    public void Awake()
    {
        servicesInstaller.Init();
        audioSystem.Init();
        sceneChangerAnimation.Init();
        ScenesChanger.Init(scenesConfig);
        gameAnalyticsManager.Initialize();

        ScenesChanger.OpenScene(ScenesChanger.scenes.Menu);
    }
}