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

    public void OnClotClicked(int id)
    {
        TakeTurn(id);
    }

    protected void TakeTurn(int id)
    {
        List<SlotStates> TempSlotsStates = _model.SlotsStates;

        TempSlotsStates[id] = SlotStates.Circle;

        _model.SetState(TempSlotsStates);
        _model.SetStateResult(AnalyzeResult());
    }

    private bool AnalyzeResult()
    {
        List<SlotStates> SlotsStates = _model.SlotsStates;

        if (SlotsStates[0] == SlotStates.Circle && SlotsStates[1] == SlotStates.Circle && SlotsStates[2] == SlotStates.Circle)
            return true;
        else if (SlotsStates[3] == SlotStates.Circle && SlotsStates[4] == SlotStates.Circle && SlotsStates[5] == SlotStates.Circle)
            return true;
        else if (SlotsStates[6] == SlotStates.Circle && SlotsStates[7] == SlotStates.Circle && SlotsStates[8] == SlotStates.Circle)
            return true;

        if (SlotsStates[0] == SlotStates.Circle && SlotsStates[3] == SlotStates.Circle && SlotsStates[6] == SlotStates.Circle)
            return true;
        else if (SlotsStates[1] == SlotStates.Circle && SlotsStates[4] == SlotStates.Circle && SlotsStates[7] == SlotStates.Circle)
            return true;
        else if (SlotsStates[2] == SlotStates.Circle && SlotsStates[5] == SlotStates.Circle && SlotsStates[8] == SlotStates.Circle)
            return true;

        if (SlotsStates[0] == SlotStates.Circle && SlotsStates[4] == SlotStates.Circle && SlotsStates[8] == SlotStates.Circle)
            return true;
        else if (SlotsStates[2] == SlotStates.Circle && SlotsStates[4] == SlotStates.Circle && SlotsStates[6] == SlotStates.Circle)
            return true;

        return false;
    }
}
