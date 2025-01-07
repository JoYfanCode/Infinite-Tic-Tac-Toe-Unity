using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class GameFieldBootstrap : MonoBehaviour
{
    [SerializeField, BoxGroup("General")] private GameFieldConfig gameFieldConfig;
    [SerializeField, BoxGroup("General")] private AILevelsConfigs AILevelsConfigs;

    [SerializeField, BoxGroup("Animations")] private ObjectsAppearAnimationConfig slotsAnimationConfig;
    [SerializeField, BoxGroup("Animations")] private ObjectsAppearAnimationConfig circlesPointsAnimationConfig;
    [SerializeField, BoxGroup("Animations")] private ObjectsAppearAnimationConfig crossesPointsAnimationConfig;

    [Inject(Id = "CirclesPointsView")] private PointsView circlesPointsView;
    [Inject(Id = "CrossesPointsView")] private PointsView crossesPointsView;
    [Inject] private GameplayView gameplayView;
    [Inject] private GameplayModel gameplayModel;
    [Inject] private PointsHandler pointsHandler;

    public async void Awake()
    {
        GameplayPresenterFactory gameplayPresenterFactroy = new GameplayPresenterFactory(gameplayModel, gameplayView);
        GameplayPresenter gameplayPresenter = gameplayPresenterFactroy.CreateGameplayPresenter(gameFieldConfig, AILevelsConfigs);
        gameplayView.Init(gameplayPresenter);

        pointsHandler.Init();

        await Animations();

        gameplayPresenter.FirstMoveDetermination();
    }

    private async Task Animations()
    {
        ObjectsAppearAnimation<Slot> slotsAppearAnimation = new ObjectsAppearAnimation<Slot>(slotsAnimationConfig, gameplayView.Slots);
        ObjectsAppearAnimation<Point> circlesPointsAnimation = new ObjectsAppearAnimation<Point>(circlesPointsAnimationConfig, circlesPointsView.Points);
        ObjectsAppearAnimation<Point> crossesPointsAnimation = new ObjectsAppearAnimation<Point>(crossesPointsAnimationConfig, crossesPointsView.Points);

        await SceneChangerAnimation.inst.FadeAsync();
        circlesPointsAnimation.Appear();
        crossesPointsAnimation.Appear();
        await slotsAppearAnimation.AppearAsync();
    }
}