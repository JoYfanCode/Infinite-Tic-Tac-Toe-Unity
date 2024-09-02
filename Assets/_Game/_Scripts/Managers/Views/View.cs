using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class View : MonoBehaviour
{
    protected Presenter _presenter;

    public virtual void Init(Presenter presenter)
    {
        _presenter = presenter;

        _presenter.FirstMoveDetermination();
    }

    public abstract void DisplayField(List<SlotStates> Field, int CountTurns);
    public abstract void BoomParticleSlot(int indexSlot, SlotStates slotState);
    public abstract void LightDownColorSlot(int indexSlot);
    public abstract void LightUpColorSlots();
    public abstract void DisplayWinCircle(int countWins);
    public abstract void DisplayWinCross(int countWins);
    public abstract void SetTurnState(SlotStates state);
    public abstract void UpdateAverageText(List<int> turnsList);
}
