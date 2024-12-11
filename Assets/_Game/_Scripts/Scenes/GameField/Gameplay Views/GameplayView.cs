using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class GameplayView : MonoBehaviour
{
    protected GameplayPresenter presenter;

    public virtual void Init(GameplayPresenter presenter)
    {
        this.presenter = presenter;
        this.presenter.FirstMoveDetermination();
    }

    public abstract void DisplayField(IReadOnlyList<SlotStates> Field);
    public abstract void BoomParticleSlot(int indexSlot, SlotStates slotState);
    public abstract void LightDownColorSlot(int indexSlot);
    public abstract void LightUpColorSlots();
    public abstract void DisplayCounters(int countCirclesPoints, int countCrossesPoints);
    public abstract void SetTurnState(SlotStates state);
    public abstract void ClearFieldAnimation();

    public void PlayClickSound() => AudioSystem.inst.PlayClickSound();
    public void PlayWinSound() => AudioSystem.PlaySound(AudioSystem.inst.Win);
}
