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
        if (_model.IsWinCircle == false && _model.IsWinCross == false)
            if (_model.SlotsStates[id] == SlotStates.Empty)
                TakeTurn(id);
    }

    protected abstract void TakeTurn(int id);

    protected bool CheckField(SlotStates State)
    {
        List<SlotStates> SlotsStates = _model.SlotsStates;

        if (SlotsStates[0] == State && SlotsStates[1] == State && SlotsStates[2] == State)
            return true;
        else if (SlotsStates[3] == State && SlotsStates[4] == State && SlotsStates[5] == State)
            return true;
        else if (SlotsStates[6] == State && SlotsStates[7] == State && SlotsStates[8] == State)
            return true;

        if (SlotsStates[0] == State && SlotsStates[3] == State && SlotsStates[6] == State)
            return true;
        else if (SlotsStates[1] == State && SlotsStates[4] == State && SlotsStates[7] == State)
            return true;
        else if (SlotsStates[2] == State && SlotsStates[5] == State && SlotsStates[8] == State)
            return true;

        if (SlotsStates[0] == State && SlotsStates[4] == State && SlotsStates[8] == State)
            return true;
        else if (SlotsStates[2] == State && SlotsStates[4] == State && SlotsStates[6] == State)
            return true;

        return false;
    }
}
