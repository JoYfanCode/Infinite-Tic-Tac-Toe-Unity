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

        saveLoadManager.LoadGame();

        MoneyStorage moneyStorage = ServiceLocator.GetService<MoneyStorage>();
        moneyStorage.AddMoney(100);
        print(moneyStorage.Money);
    }

    async void OpenNewDifficulty()
    {
        await SceneChangerAnimation.inst.FadeAsync();

        difficultiesManager.Unlock(SetUp.CountCompletedLevels - 1);
        await _levelsButtonsAppearAnimation.AppearAsync();
        await Task.Delay(milisecUntilUnlockNewDif);
        difficultiesManager.UnlockLastOneWithEffect(SetUp.CountCompletedLevels);
        SetUp.isOpenedNewDifficulty = false;
    }

    async void ClassicInitScene()
    {
        await SceneChangerAnimation.inst.FadeAsync();

        difficultiesManager.Unlock(SetUp.CountCompletedLevels);
        _modeButtonsAnimation.Appear();
    }
}