using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public abstract class Presenter
{
    protected readonly Model _model;
    protected readonly View _view;

    protected readonly float _restartGameCooldown;

    protected const int RESTART_GAME_DEFAULT = 100;

    public Presenter(Model model, View view, float restartGameCooldown = RESTART_GAME_DEFAULT)
    {
        _model = model;
        _view = view;
        _restartGameCooldown = restartGameCooldown;
    }

    public virtual void OnClotClicked(int id)
    {
        if (_model.Field[id] == SlotStates.Empty)
            if (_model.isGameState)
                DoTurn(id);
    }

    protected abstract void DoTurn(int id);

    public abstract void FirstMoveDetermination();
    public abstract void FirstMoveAnotherPlayer();

    public async void RestartGame()
    {
        await Task.Run(() => Thread.Sleep((int)_restartGameCooldown));

        if (_model.CountWinsCircle == _model.POINTS_FOR_WIN || _model.CountCrossesPoints == _model.POINTS_FOR_WIN)
        {
            _model.ResetCounters();
            _view.DisplayCounters(_model.CountWinsCircle, _model.CountCrossesPoints);
        }

        _view.ClearFieldAnimation();
    }

    public void ResetFieldState()
    {
        _model.ResetFieldState();
        _view.LightUpColorSlots();
    }

    protected void CheckField(List<SlotStates> Field)
    {
        List<int> WinIndexesSlots;

        if (FieldChecker.Check(Field, SlotStates.Circle, out WinIndexesSlots))
        {
            _model.SetStateWin();
            _model.AddCirclesPoint();
            _view.DisplayCounters(_model.CountWinsCircle, _model.CountCrossesPoints);

            for (int i = 0; i < WinIndexesSlots.Count; i++)
                _view.BoomParticleSlot(WinIndexesSlots[i], SlotStates.Circle);

            RestartGame();
        }
        else if (FieldChecker.Check(Field, SlotStates.Cross, out WinIndexesSlots))
        {
            _model.SetStateWin();
            _model.AddCrossesPoint();
            _view.DisplayCounters(_model.CountWinsCircle, _model.CountCrossesPoints);

            for (int i = 0; i < WinIndexesSlots.Count; i++)
                _view.BoomParticleSlot(WinIndexesSlots[i], SlotStates.Cross);

            RestartGame();
        }
    }
}
