using System.Threading.Tasks;
using UnityEngine;

// Put away MonoBehaviour
public class MenuBootstrap : MonoBehaviour
{
    [SerializeField] private int milisecUntilUnlockNewDif = 1000;

    [SerializeField] private AudioSystem audioSystem;
    [SerializeField] private SaveLoader saveLoader;

    [SerializeField] private SceneChangerAnimation sceneChangerAnimation;
    [SerializeField] private ObjectsAppearAnimation startButtonsAppearAnimation;
    [SerializeField] private ObjectsAppearAnimation AIDifficultyButtonsAppearAnimation;
    [SerializeField] private DifficultiesUnlocker difficultiesManager;

    [SerializeField] private ScenesConfig scenesConfig;

    public async void Awake()
    {
        saveLoader.Init();
        audioSystem.Init();
        ScenesChanger.Init(scenesConfig);

        AIDifficultyButtonsAppearAnimation.Init();
        startButtonsAppearAnimation.Init();

        sceneChangerAnimation.Init();
        await SceneChangerAnimation.inst.FadeAsync();

        if (SetUp.isOpenedNewDifficulty)
        {
            difficultiesManager.Unlock(SetUp.CountCompletedLevels - 1);
            await AIDifficultyButtonsAppearAnimation.AppearAsync();
            await Task.Delay(milisecUntilUnlockNewDif);
            difficultiesManager.UnlockLastOneWithEffect(SetUp.CountCompletedLevels);
            SetUp.isOpenedNewDifficulty = false;
        }
        else
        {
            difficultiesManager.Unlock(SetUp.CountCompletedLevels);
            startButtonsAppearAnimation.Appear();
        }
    }
}