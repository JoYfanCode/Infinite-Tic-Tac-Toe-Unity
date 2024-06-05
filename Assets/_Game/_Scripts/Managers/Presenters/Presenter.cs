using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Presenter
{
    protected Model _model;

    public Presenter(Model model)
    {
        _model = model;
    }

    public virtual void OnClotClicked(int id)
    {
        if (_model.SlotsStates[id] == SlotStates.Empty)
            if (_model.isGameOn())
                TakeTurn(id);
    }

    protected abstract void TakeTurn(int id);
}
