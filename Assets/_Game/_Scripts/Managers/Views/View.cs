using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class View : MonoBehaviour
{
    protected Presenter _presenter;

    public virtual void Init(Presenter presenter)
    {
        _presenter = presenter;

        _presenter.FirstMoveDetermination();
    }

    public abstract void DisplayField(List<SlotStates> Field);
    public abstract void BoomParticleSlot(int indexSlot, SlotStates slotState);
    public abstract void LightDownColorSlot(int indexSlot);
    public abstract void LightUpColorSlots();
    public abstract void DisplayCounters(int countCirclesPoints, int countCrossesPoints);
    public abstract void SetTurnState(SlotStates state);
    public abstract void ClearFieldAnimation();
}
