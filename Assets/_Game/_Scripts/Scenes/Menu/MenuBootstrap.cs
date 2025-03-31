using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[DefaultExecutionOrder(-100)]
public class MenuBootstrap : MonoBehaviour
{
    [SerializeField, BoxGroup("General")] int milisecUntilUnlockNewDif = 1000;

    [SerializeField, BoxGroup("Animations")] ObjectsAppearAnimationConfig modeButtonsAnimationConfig;
    [SerializeField, BoxGroup("Animations")] ObjectsAppearAnimationConfig levelsButtonsAppearAnimationConfig;

    [Inject] MenuButtonsHandler menuButtonHandler;
    [Inject] DifficultiesUnlocker difficultiesManager;
    [Inject] SaveLoadManager saveLoadManager;

    ObjectsAppearAnimation<Button> _modeButtonsAnimation;
    ObjectsAppearAnimation<Button> _levelsButtonsAppearAnimation;

    public void Awake()
    {
        Init();

        if (SetUp.isOpenedNewDifficulty)
        {
            OpenNewDifficulty();
        }
        else
        {
            ClassicInitScene();
        }
    }

    void Init()
    {
        _modeButtonsAnimation = new ObjectsAppearAnimation<Button>(modeButtonsAnimationConfig, menuButtonHandler.ModeButtons);
        _levelsButtonsAppearAnimation = new ObjectsAppearAnimation<Button>(levelsButtonsAppearAnimationConfig, menuButtonHandler.LevelsButtons);

        menuButtonHandler.Init(_modeButtonsAnimation, _levelsButtonsAppearAnimation);
    }

    async void OpenNewDifficulty()
    {
        await SceneChangerAnimation.inst.FadeAsync();

        difficultiesManager.Unlock(SetUp.LevelsStorage.CompletedLevels - 1);
        await _levelsButtonsAppearAnimation.AppearAsync();
        await Task.Delay(milisecUntilUnlockNewDif);
        difficultiesManager.UnlockLastOneWithEffect(SetUp.LevelsStorage.CompletedLevels);
        SetUp.isOpenedNewDifficulty = false;
        saveLoadManager.SaveGame();
    }

    async void ClassicInitScene()
    {
        saveLoadManager.LoadGame();
        SetUp.LevelsStorage = ServiceLocator.GetService<LevelsStorage>();

        await SceneChangerAnimation.inst.FadeAsync();

        difficultiesManager.Unlock(SetUp.LevelsStorage.CompletedLevels);
        _modeButtonsAnimation.Appear();
    }
}