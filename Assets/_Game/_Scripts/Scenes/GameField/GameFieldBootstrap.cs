using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

// Put away MonoBehaviour
public class GameFieldBootstrap : MonoBehaviour
{
    // One Config
    [SerializeField, TabGroup("Parameters"), Range(0, 1000)] private int AICooldownMin = 300;
    [SerializeField, TabGroup("Parameters"), Range(0, 2000)] private int AICooldownMax = 600;
    [SerializeField, TabGroup("Parameters"), Range(0, 5000)] private int restartGameCooldown = 1500;

    // One Config
    [SerializeField, TabGroup("Configs")] private List<AIConfig> AINormalConfigs;
    [SerializeField, TabGroup("Configs")] private List<AIConfig> AIHardConfigs;
    [SerializeField, TabGroup("Configs")] private List<AIConfig> AIVeryHardConfigs;

    // Put away monobeh
    [SerializeField, TabGroup("Animations")] private ObjectsAppearAnimation slotsAppearAnimation;
    [SerializeField, TabGroup("Animations")] private ObjectsAppearAnimation circlesPointsAnimation;
    [SerializeField, TabGroup("Animations")] private ObjectsAppearAnimation crossesPointsAnimation;

    [Inject(Id = "CirclesPointsView")] private PointsView circlesPointsView;
    [Inject(Id = "CrossesPointsView")] private PointsView crossesPointsView;
    [Inject] private GameplayView _gameplayView;
    [Inject] private GameplayModel _gameplayModel;
    [Inject] private PointsHandler _pointsHandler;

    public async void Awake()
    {
        // GameplayHandler
        GameplayPresenter gameplayPresenter = CreatePresenter();
        _gameplayView.Init(gameplayPresenter);

        _pointsHandler.Init();

        // AnimationsHandler
        slotsAppearAnimation.Init();
        circlesPointsAnimation.Init(Utilities.ConverToGameObjects(circlesPointsView.Points));
        crossesPointsAnimation.Init(Utilities.ConverToGameObjects(crossesPointsView.Points));

        await SceneChangerAnimation.inst.FadeAsync();

        circlesPointsAnimation.Appear();
        crossesPointsAnimation.Appear();

        await slotsAppearAnimation.AppearAsync();

        gameplayPresenter.FirstMoveDetermination();
    }

    // Model and View have to inject to Zenject Installer
    // And mb create Factory
    private GameplayPresenter CreatePresenter()
    {
        if (SetUp.GameMode == GameModes.TwoPlayers)
        {
            return new GameplayPresenterTwoPlayers(_gameplayModel, _gameplayView, restartGameCooldown);
        }
        else if (SetUp.GameMode == GameModes.OnePlayer)
        {
            if (SetUp.CurrentLevelIndex == 0)
            {
                return new GameplayPresenterAI(_gameplayModel, _gameplayView, new AIOneTurn(AINormalConfigs), restartGameCooldown, AICooldownMin, AICooldownMax);
            }
            else if (SetUp.CurrentLevelIndex == 1)
            {
                return new GameplayPresenterAI(_gameplayModel, _gameplayView, new AIMiniMax(AIHardConfigs), restartGameCooldown, AICooldownMin, AICooldownMax);
            }
            else if (SetUp.CurrentLevelIndex == 2)
            {
                return new GameplayPresenterAI(_gameplayModel, _gameplayView, new AIMiniMax(AIVeryHardConfigs), restartGameCooldown, AICooldownMin, AICooldownMax);
            }
        }

        return new GameplayPresenterTwoPlayers(_gameplayModel, _gameplayView, restartGameCooldown);
    }
}