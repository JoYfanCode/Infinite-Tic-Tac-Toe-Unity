using UnityEngine;

public class LoadBootstrap : MonoBehaviour
{
    [SerializeField] ScenesConfig scenesConfig;
    [SerializeField] AudioSystem audioSystem;
    [SerializeField] SaveSystem saveSystem;
    [SerializeField] SceneChangerAnimation sceneChangerAnimation;
    [SerializeField] GameAnalyticsManager gameAnalyticsManager;

    public void Awake()
    {
        saveSystem.Init();
        audioSystem.Init();
        sceneChangerAnimation.Init();
        ScenesChanger.Init(scenesConfig);
        gameAnalyticsManager.Initialize();

        ScenesChanger.OpenScene(ScenesChanger.scenes.Menu);
    }
}