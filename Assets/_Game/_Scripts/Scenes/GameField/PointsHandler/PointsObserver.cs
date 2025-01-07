public sealed class PointsObserver
{
    private readonly PointsModel model;
    private readonly PointsView view;

    public PointsObserver(PointsModel model, PointsView view)
    {
        this.model = model;
        this.view = view;

        view.SetPointPrefab(model.PointPrefab);
        view.CreatePoints(model.CountMaxPoints);

        model.OnPointsOnChanged += view.SetPointsOn;
    }

    public void Dispose()
    {
        model.OnPointsOnChanged -= view.SetPointsOn;
    }
}
