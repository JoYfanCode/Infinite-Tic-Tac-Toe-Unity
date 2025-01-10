using System;
using Zenject;

public class PointsHandler : IDisposable
{
    PointsModel _circlesPointsModel;
    PointsModel _crossesPointsModel;
    PointsView _circlesPointsView;
    PointsView _crossesPointsView;
    PointsObserver _circlesPointsObserver;
    PointsObserver _crossesPointsObserver;

    [Inject]
    public void Construct(PointsModel circlesPointsModel, PointsModel crossesPointsModel,
                                  PointsView circlesPointsView, PointsView crossesPointsView)
    {
        _circlesPointsModel = circlesPointsModel;
        _crossesPointsModel = crossesPointsModel;
        _circlesPointsView = circlesPointsView;
        _crossesPointsView = crossesPointsView;
    }

    public void Init()
    {
        _circlesPointsObserver = new PointsObserver(_circlesPointsModel, _circlesPointsView);
        _crossesPointsObserver = new PointsObserver(_crossesPointsModel, _crossesPointsView);
    }

    public void Dispose()
    {
        _circlesPointsObserver.Dispose();
        _crossesPointsObserver.Dispose();
    }
}
