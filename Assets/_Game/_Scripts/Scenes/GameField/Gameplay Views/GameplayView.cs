using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

public abstract class GameplayView : MonoBehaviour
{
    protected GameplayPresenter presenter;

    public virtual void Init(GameplayPresenter presenter)
    {
        this.presenter = presenter;
    }

    public abstract void DisplayField(IReadOnlyList<SlotStates> Field);
    public abstract void BoomParticleSlot(int indexSlot, SlotStates slotState);
    public abstract void LightDownColorSlot(int indexSlot);
    public abstract void LightUpColorSlots();
    public abstract void DisplayCounters(int countCirclesPoints, int countCrossesPoints);
    public abstract void SetTurnState(SlotStates state);
    public abstract Task ClearFieldAnimation();
    public abstract void PlayWinEffects(SlotStates winState);
    public abstract Task OpenMenuAsync();

    public void PlayClickSound() => AudioSystem.PlayClickSound();
    public void PlayWinSound() => AudioSystem.PlaySound(AudioSystem.inst.Win);
    public void PlayFireworkSound() => AudioSystem.PlaySound(AudioSystem.inst.Firework);
}
