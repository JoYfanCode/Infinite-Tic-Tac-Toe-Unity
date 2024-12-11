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

    private Modes _mode = Modes.TwoPlayers;
    private const int AI_NORMAL_DEPTH = 2;
    private const int AI_HARD_DEPTH = 3;

    public void Awake()
    {
        _mode = SetUp.Mode;

        GameplayModel gameplayModel = new GameplayModel();
        GameplayPresenter gameplayPresenter = CreatePresenter(_mode, gameplayModel, gameplayView);

        gameplayView.Init(gameplayPresenter);

        sceneChangerAnimation.Fade();
        slotsAppearAnimation.Init().Appear();
        circlesPointsAnimation.Init().Appear();
        crossesPointsAnimation.Init().Appear();
    }

    private GameplayPresenter CreatePresenter(Modes mode, GameplayModel model, GameplayView view)
    {
        if (_mode == Modes.TwoPlayers) return new GameplayPresenterTwoPlayers(model, view, restartGameCooldown);
        else if (_mode == Modes.AINormal) return new GameplayPresenterAI(model, view, new AIOneTurn(), AICooldownMin, AICooldownMax, restartGameCooldown);
        else if (_mode == Modes.AIHard) return new GameplayPresenterAI(model, view, new AIMiniMax(AI_NORMAL_DEPTH), AICooldownMin, AICooldownMax, restartGameCooldown);
        else if (_mode == Modes.AIVeryHard) return new GameplayPresenterAI(model, view, new AIMiniMax(AI_HARD_DEPTH), AICooldownMin, AICooldownMax, restartGameCooldown);
        else return new GameplayPresenterTwoPlayers(model, view, restartGameCooldown);
    }
}