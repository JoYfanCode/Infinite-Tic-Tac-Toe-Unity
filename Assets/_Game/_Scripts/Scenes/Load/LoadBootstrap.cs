using UnityEngine;

[DefaultExecutionOrder(99)]
public class LoadBootstrap : MonoBehaviour
{
    [SerializeField] ScenesConfig scenesConfig;
    [SerializeField] AudioSystem audioSystem;
    [SerializeField] SaveSystem saveSystem;
    [SerializeField] SceneChangerAnimation sceneChangerAnimation;

    public void Awake()
    {
        saveSystem.Init();
        audioSystem.Init();
        sceneChangerAnimation.Init();
        ScenesChanger.Init(scenesConfig);

        ScenesChanger.OpenScene(ScenesChanger.scenes.Menu);
    }
}