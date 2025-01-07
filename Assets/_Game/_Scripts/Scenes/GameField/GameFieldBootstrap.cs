using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

// Put away MonoBehaviour
public class GameFieldBootstrap : MonoBehaviour
{
    [SerializeField] private GameFieldConfig gameFieldConfig;
    [SerializeField] private AILevelsConfigs AILevelsConfigs;

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
        GameplayPresenterFactory gameplayPresenterFactroy = new GameplayPresenterFactory(_gameplayModel, _gameplayView);
        GameplayPresenter gameplayPresenter = gameplayPresenterFactroy.CreateGameplayPresenter(gameFieldConfig, AILevelsConfigs);
        _gameplayView.Init(gameplayPresenter);

        _pointsHandler.Init();

        // AnimationsHandler
        slotsAppearAnimation.Init(_gameplayView.Slots);
        circlesPointsAnimation.Init(circlesPointsView.Points);
        crossesPointsAnimation.Init(crossesPointsView.Points);
        await SceneChangerAnimation.inst.FadeAsync();
        circlesPointsAnimation.Appear();
        crossesPointsAnimation.Appear();
        await slotsAppearAnimation.AppearAsync();

        gameplayPresenter.FirstMoveDetermination();
    }
}