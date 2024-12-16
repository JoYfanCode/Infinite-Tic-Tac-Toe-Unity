using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class GameFieldBootstrap : MonoBehaviour
{
    [Tab("Parameters")]

    [SerializeField, Range(0, 1000)] private int AICooldownMin = 300;
    [SerializeField, Range(0, 2000)] private int AICooldownMax = 600;
    [SerializeField, Range(0, 5000)] private int restartGameCooldown = 1500;

    [Tab("Configs")]

    [SerializeField] private List<AIConfig> AINormalConfigs;
    [SerializeField] private List<AIConfig> AIHardConfigs;
    [SerializeField] private List<AIConfig> AIVeryHardConfigs;

    [Tab("Settings")]

    [SerializeField] private GameplayViewStandart gameplayView;

    [SerializeField] private SceneChangerAnimation sceneChangerAnimation;
    [SerializeField] private ObjectsAppearAnimation slotsAppearAnimation;
    [SerializeField] private ObjectsAppearAnimation circlesPointsAnimation;
    [SerializeField] private ObjectsAppearAnimation crossesPointsAnimation;

    private GameModes gameMode;
    private AIDifficulties AIDifficulty;

    public async void Awake()
    {
        gameMode = SetUp.GameMode;
        AIDifficulty = SetUp.AIDifficulty;

        GameplayModel gameplayModel = new GameplayModel();
        GameplayPresenter gameplayPresenter = CreatePresenter(gameMode, AIDifficulty, gameplayModel, gameplayView);
        gameplayView.Init(gameplayPresenter);

        slotsAppearAnimation.Init();
        circlesPointsAnimation.Init();
        crossesPointsAnimation.Init();

        await sceneChangerAnimation.FadeAsync();

        circlesPointsAnimation.Appear();
        crossesPointsAnimation.Appear();

        await slotsAppearAnimation.AppearAsync();

        gameplayPresenter.FirstMoveDetermination();
    }

    private GameplayPresenter CreatePresenter(GameModes gameMode, AIDifficulties AIDifficulty, GameplayModel model, GameplayView view)
    {
        if (gameMode == GameModes.TwoPlayers)
        {
            return new GameplayPresenterTwoPlayers(model, view, restartGameCooldown);
        }
        else if (gameMode == GameModes.OnePlayer)
        {
            if (AIDifficulty == AIDifficulties.NORMAL)
            {
                return new GameplayPresenterAI(model, view, new AIOneTurn(AINormalConfigs), restartGameCooldown, AICooldownMin, AICooldownMax);
            }
            else if (AIDifficulty == AIDifficulties.HARD)
            {
                return new GameplayPresenterAI(model, view, new AIMiniMax(AIHardConfigs), restartGameCooldown, AICooldownMin, AICooldownMax);
            }
            else if (AIDifficulty == AIDifficulties.VERY_HARD)
            {
                return new GameplayPresenterAI(model, view, new AIMiniMax(AIVeryHardConfigs), restartGameCooldown, AICooldownMin, AICooldownMax);
            }
        }

        return new GameplayPresenterTwoPlayers(model, view, restartGameCooldown);
    }
}