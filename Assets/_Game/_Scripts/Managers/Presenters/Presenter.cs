using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Presenter
{
    protected Model _model;

    public event Action<List<SlotStates>> OnTurnDone;
    public event Action OnCircleWon;
    public event Action OnCrossWon;

    public Presenter(Model model)
    {
        _model = model;
    }

    protected void OnTurnDoneEvent(List<SlotStates> Field)
        => OnTurnDone?.Invoke(Field);

    protected void OnCircleWonEvent()
        => OnCircleWon?.Invoke();

    protected void OnCrossWonEvent()
        => OnCrossWon?.Invoke();

    public virtual void OnClotClicked(int id)
    {
        if (_model.SlotsStates[id] == SlotStates.Empty)
            if (_model.isGameOn())
                DoTurn(id);
    }

    protected abstract void DoTurn(int id);

    public abstract void FirstMoveDetermination();

    protected void CheckField(List<SlotStates> Field)
    {
        if (FieldChecker.Check(Field, SlotStates.Circle))
        {
            _model.SetStateWin(SlotStates.Circle);
            OnCircleWonEvent();
        }
        else if (FieldChecker.Check(Field, SlotStates.Cross))
        {
            _model.SetStateWin(SlotStates.Cross);
            OnCrossWonEvent();
        }
    }
}
