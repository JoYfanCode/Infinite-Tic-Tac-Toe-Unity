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

    [Tab("Objects")]

    [SerializeField] private GameplayViewStandart gameplayView;

    [SerializeField] private SceneChangerAnimation sceneChangerAnimation;
    [SerializeField] private ObjectsAppearAnimation slotsAppearAnimation;
    [SerializeField] private ObjectsAppearAnimation circlesPointsAnimation;
    [SerializeField] private ObjectsAppearAnimation crossesPointsAnimation;

    private GameModes gameMode = GameModes.TwoPlayers;
    private AIDifficulties AIDifficulty = AIDifficulties.NORMAL;

    private const int AI_NORMAL_DEPTH = 2;
    private const int AI_HARD_DEPTH = 3;

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

        await sceneChangerAnimation.Fade();

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
            if (AIDifficulty == AIDifficulties.NORMAL) return new GameplayPresenterAI(model, view, new AIOneTurn(), restartGameCooldown, AICooldownMin, AICooldownMax);
            else if (AIDifficulty == AIDifficulties.HARD) return new GameplayPresenterAI(model, view, new AIMiniMax(AI_NORMAL_DEPTH), restartGameCooldown, AICooldownMin, AICooldownMax);
            else if (AIDifficulty == AIDifficulties.VERY_HARD) return new GameplayPresenterAI(model, view, new AIMiniMax(AI_HARD_DEPTH), restartGameCooldown, AICooldownMin, AICooldownMax);
        }

        return new GameplayPresenterTwoPlayers(model, view, restartGameCooldown);
    }
}