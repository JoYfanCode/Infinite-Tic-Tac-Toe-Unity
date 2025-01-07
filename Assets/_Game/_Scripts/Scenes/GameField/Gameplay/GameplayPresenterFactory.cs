public class GameplayPresenterFactory
{
    private GameplayModel _gameplayModel;
    private GameplayView _gameplayView;

    public GameplayPresenter CreateGameplayPresenter(int levelIndex)
    {
        //...
        return null;
    }

    private void Construct(GameplayModel model, GameplayView view)
    {
        _gameplayModel = model;
        _gameplayView = view;
    }
}