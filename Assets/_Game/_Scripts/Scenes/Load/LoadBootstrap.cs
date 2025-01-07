using UnityEngine;

public class LoadBootstrap : MonoBehaviour
{
    [SerializeField] private ScenesConfig scenesConfig;
    [SerializeField] private AudioSystem audioSystem;
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private SceneChangerAnimation sceneChangerAnimation;

    public void Awake()
    {
        saveSystem.Init();
        audioSystem.Init();
        sceneChangerAnimation.Init();
        ScenesChanger.Init(scenesConfig);

        ScenesChanger.OpenScene(ScenesChanger.scenes.Menu);
    }
}