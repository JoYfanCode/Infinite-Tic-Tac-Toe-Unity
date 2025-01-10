public class GameplayPresenterFactory
{
    GameplayModel _gameplayModel;
    GameplayView _gameplayView;

    public GameplayPresenterFactory(GameplayModel model, GameplayView view)
    {
        _gameplayModel = model;
        _gameplayView = view;
    }

    public GameplayPresenter CreateGameplayPresenter(GameFieldConfig gameFieldConfig, AILevelsConfigs AILevelsConfigs)
    {
        if (SetUp.GameMode == GameModes.TwoPlayers)
        {
            return new GameplayPresenterTwoPlayers(_gameplayModel, _gameplayView, gameFieldConfig.RestartGameCooldown);
        }
        else
        {
            AI AI;
            int levelIndex = SetUp.CurrentLevelIndex;

            if (AILevelsConfigs.AILevelConfigs(levelIndex).Algorithm == AIAlgorithm.OneTurn)
            {
                AI = new AIOneTurn(AILevelsConfigs.AILevelConfigs(levelIndex));
            }
            else
            {
                AI = new AIMiniMax(AILevelsConfigs.AILevelConfigs(levelIndex));
            }

            return new GameplayPresenterAI(_gameplayModel, _gameplayView, AI, gameFieldConfig.RestartGameCooldown, gameFieldConfig.AICooldownRange);
        }
    }

}