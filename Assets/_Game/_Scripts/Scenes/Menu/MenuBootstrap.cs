using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MenuBootstrap : MonoBehaviour
{
    [SerializeField] private ScenesConfig scenesConfig;

    [SerializeField] private ObjectsAppearAnimation startButtonsAppearAnimation;
    [SerializeField] private ObjectsAppearAnimation AIDifficultyButtonsAppearAnimation;

    [SerializeField] private int milisecUntilUnlockNewDif = 1000;
    [SerializeField] private DifficultiesUnlocker difficultiesManager;

    [SerializeField] private SceneChangerAnimation sceneChangerAnimation;
    [Inject] private AudioSystem audioSystem;
    [Inject] private SaveSystem saveSystem;

    public async void Awake()
    {
        saveSystem.Init();
        audioSystem.Init();
        ScenesChanger.Init(scenesConfig);

        AIDifficultyButtonsAppearAnimation.Init();
        startButtonsAppearAnimation.Init();

        sceneChangerAnimation.Init();
        await SceneChangerAnimation.inst.FadeAsync();

        if (SetUp.isOpenedNewDifficulty)
        {
            OpenNewDifficulty();
        }
        else
        {
            difficultiesManager.Unlock(SetUp.CountCompletedLevels);
            startButtonsAppearAnimation.Appear();
        }
    }

    private async void OpenNewDifficulty()
    {
        difficultiesManager.Unlock(SetUp.CountCompletedLevels - 1);
        await AIDifficultyButtonsAppearAnimation.AppearAsync();
        await Task.Delay(milisecUntilUnlockNewDif);
        difficultiesManager.UnlockLastOneWithEffect(SetUp.CountCompletedLevels);
        SetUp.isOpenedNewDifficulty = false;
    }
}