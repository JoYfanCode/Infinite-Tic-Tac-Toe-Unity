using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class View : MonoBehaviour
{
    protected Presenter _presenter;

    public virtual void Init(Presenter presenter)
    {
        _presenter = presenter;

        _presenter.OnTurnDone += DisplayField;
        _presenter.OnTurnDone += ChangeTurnState;
        _presenter.OnCircleWon += DisplayWinCircle;
        _presenter.OnCrossWon += DisplayWinCross;
        _presenter.OnAppearedSlotState += BoomParticleSlot;
        _presenter.OnRemovedSlotState += LightDownColorSlot;
        _presenter.OnRestartedGame += LightUpColorSlots;
        _presenter.OnFirstStateDetermined += SetTurnState;

        _presenter.FirstMoveDetermination();
    }

    public virtual void OnDisable()
    {
        _presenter.OnTurnDone -= DisplayField;
        _presenter.OnTurnDone -= ChangeTurnState;
        _presenter.OnCircleWon -= DisplayWinCircle;
        _presenter.OnCrossWon -= DisplayWinCross;
        _presenter.OnAppearedSlotState -= BoomParticleSlot;
        _presenter.OnRemovedSlotState -= LightDownColorSlot;
        _presenter.OnRestartedGame -= LightUpColorSlots;
        _presenter.OnFirstStateDetermined -= SetTurnState;
    }

    protected abstract void DisplayField(List<SlotStates> Field, int CountTurns);
    protected abstract void BoomParticleSlot(int indexSlot, SlotStates slotState);
    protected abstract void LightDownColorSlot(int indexSlot);
    protected abstract void LightUpColorSlots();
    protected abstract void DisplayWinCircle(int countWins);
    protected abstract void DisplayWinCross(int countWins);
    protected abstract void SetTurnState(SlotStates state);
    protected abstract void ChangeTurnState(List<SlotStates> Field, int CountTurns);
}
