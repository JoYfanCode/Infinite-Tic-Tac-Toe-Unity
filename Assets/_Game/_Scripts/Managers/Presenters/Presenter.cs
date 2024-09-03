using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

public abstract class Presenter
{
    protected Model _model;
    protected View _view;

    public Presenter(Model model, View view)
    {
        _model = model;
        _view = view;
    }

    public virtual void OnClotClicked(int id)
    {
        if (_model.SlotsStates[id] == SlotStates.Empty)
            if (_model.isGameOn)
                DoTurn(id);
    }

    protected abstract void DoTurn(int id);

    public abstract void FirstMoveDetermination();

    public abstract void RestartGame();

    protected void CheckField(List<SlotStates> Field)
    {
        List<int> WinIndexesSlots;

        if (FieldChecker.Check(Field, SlotStates.Circle, out WinIndexesSlots))
        {
            _model.SetStateWin(SlotStates.Circle);

            for (int i = 0; i < WinIndexesSlots.Count; i++)
                _view.BoomParticleSlot(WinIndexesSlots[i], SlotStates.Circle);

            RestartGame();
        }
        else if (FieldChecker.Check(Field, SlotStates.Cross, out WinIndexesSlots))
        {
            _model.SetStateWin(SlotStates.Cross);

            for (int i = 0; i < WinIndexesSlots.Count; i++)
                _view.BoomParticleSlot(WinIndexesSlots[i], SlotStates.Cross);

            RestartGame();
        }
    }
}
