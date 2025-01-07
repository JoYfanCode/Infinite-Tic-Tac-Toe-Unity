using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuBootstrap : MonoBehaviour
{
    [SerializeField, BoxGroup("General")] private int milisecUntilUnlockNewDif = 1000;

    [SerializeField, BoxGroup("Animations")] private ObjectsAppearAnimationConfig modeButtonsAnimationConfig;
    [SerializeField, BoxGroup("Animations")] private ObjectsAppearAnimationConfig levelsButtonsAppearAnimationConfig;

    [Inject] private MenuButtonsHandler menuButtonHandler;
    [Inject] private DifficultiesUnlocker difficultiesManager;

    public async void Awake()
    {
        ObjectsAppearAnimation<Button> modeButtonsAnimation = new ObjectsAppearAnimation<Button>(modeButtonsAnimationConfig, menuButtonHandler.ModeButtons);
        ObjectsAppearAnimation<Button> levelsButtonsAppearAnimation = new ObjectsAppearAnimation<Button>(levelsButtonsAppearAnimationConfig, menuButtonHandler.LevelsButtons);

        menuButtonHandler.Init(modeButtonsAnimation, levelsButtonsAppearAnimation);

        await SceneChangerAnimation.inst.FadeAsync();

        if (SetUp.isOpenedNewDifficulty)
        {
            difficultiesManager.Unlock(SetUp.CountCompletedLevels - 1);
            await levelsButtonsAppearAnimation.AppearAsync();
            await Task.Delay(milisecUntilUnlockNewDif);
            difficultiesManager.UnlockLastOneWithEffect(SetUp.CountCompletedLevels);
            SetUp.isOpenedNewDifficulty = false;
        }
        else
        {
            difficultiesManager.Unlock(SetUp.CountCompletedLevels);
            modeButtonsAnimation.Appear();
        }
    }
}