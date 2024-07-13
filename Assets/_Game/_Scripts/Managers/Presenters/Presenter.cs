using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Presenter
{
    protected Model _model;

    public event Action<List<SlotStates>, int> OnTurnDone;
    public event Action<int> OnCircleWon;
    public event Action<int> OnCrossWon;
    public event Action OnRestartedGame;
    public event Action<SlotStates> OnFirstStateDetermined;
    public event Action<List<int>> OnGameOver;

    public Presenter(Model model)
    {
        _model = model;
    }

    protected void OnTurnDoneEvent(List<SlotStates> Field, int CountTurns)
        => OnTurnDone?.Invoke(Field, CountTurns);

    protected void OnCircleWonEvent(int count)
        => OnCircleWon?.Invoke(count);

    protected void OnCrossWonEvent(int count)
        => OnCrossWon?.Invoke(count);

    protected void OnRestartedGameEvent()
        => OnRestartedGame?.Invoke();

    protected void OnFirstStateDeterminedEvent(SlotStates state)
        => OnFirstStateDetermined?.Invoke(state);

    protected void OnGameOverEvent(List<int> turnsList)
        => OnGameOver?.Invoke(turnsList);

    public virtual void OnClotClicked(int id)
    {
        if (_model.SlotsStates[id] == SlotStates.Empty)
            if (_model.isGameOn())
                DoTurn(id);
    }

    protected abstract void DoTurn(int id);

    public abstract void FirstMoveDetermination();

    public abstract void Restart();

    protected void CheckField(List<SlotStates> Field)
    {
        if (FieldChecker.Check(Field, SlotStates.Circle))
        {
            _model.SetStateWin(SlotStates.Circle);
            OnCircleWonEvent(_model.CountWinsCircle);
            OnGameOverEvent(_model.TurnsList);
        }
        else if (FieldChecker.Check(Field, SlotStates.Cross))
        {
            _model.SetStateWin(SlotStates.Cross);
            OnCrossWonEvent(_model.CountWinsCross);
            OnGameOverEvent(_model.TurnsList);
        }
    }
}
