using UnityEngine;

// Put away MonoBehaviour
public class MenuBootstrap : MonoBehaviour
{
    [SerializeField] private AudioSystem audioSystem;
    [SerializeField] private SaveLoader saveLoader;

    [SerializeField] private ScenesChanger scenesChanger;
    [SerializeField] private SceneChangerAnimation sceneChangerAnimation;
    [SerializeField] private ObjectsAppearAnimation startButtonsAppearAnimation;
    [SerializeField] private ObjectsAppearAnimation AIDifficultyButtonsAppearAnimation;
    [SerializeField] private DifficultiesManager difficultiesManager;

    public async void Awake()
    {
        saveLoader.Init();
        audioSystem.Init();
        scenesChanger.Init();

        AIDifficultyButtonsAppearAnimation.Init();
        startButtonsAppearAnimation.Init();

        sceneChangerAnimation.Init();
        await SceneChangerAnimation.inst.FadeAsync();

        if (SetUp.isOpenedNewDifficulty)
        {
            difficultiesManager.UnlockWithoutNew();
            await AIDifficultyButtonsAppearAnimation.AppearAsync();
            difficultiesManager.UnlockNew();

            SetUp.isOpenedNewDifficulty = false;
        }
        else
        {
            difficultiesManager.Unlock();
            startButtonsAppearAnimation.Appear();
        }
    }
}