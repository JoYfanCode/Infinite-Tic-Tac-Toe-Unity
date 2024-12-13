using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public abstract class GameplayPresenter
{
    protected readonly GameplayModel model;
    protected readonly GameplayView view;

    protected readonly int restartGameCooldown;
    protected const int RESTART_GAME_DEFAULT = 100;

    public GameplayPresenter(GameplayModel model, GameplayView view, int restartGameCooldown = RESTART_GAME_DEFAULT)
    {
        this.model = model;
        this.view = view;
        this.restartGameCooldown = restartGameCooldown;
    }

    public virtual void OnClotClicked(int id)
    {
        if (model.Field[id] == SlotStates.Empty)
            if (model.IsGameState)
                DoTurn(id);
    }

    protected abstract void DoTurn(int id);

    public abstract void FirstMoveDetermination();
    public abstract void FirstMoveAnotherPlayer();

    public async void RestartGame()
    {
        if (model.IsWin)
        {
            await WaitForSeconds(restartGameCooldown);

            if (model.IsCirclesWin) view.PlayWinEffects(SlotStates.Circle);
            else if (model.IsCrossesWin) view.PlayWinEffects(SlotStates.Cross);

            await WaitForSeconds(2 * restartGameCooldown);

            model.ResetCounters();
        }
        else
        {
            await WaitForSeconds(restartGameCooldown);
        }

        view.ClearFieldAnimation();
    }

    public void ResetFieldState()
    {
        model.ResetFieldState();
        view.DisplayCounters(model.CountCirclesPoints, model.CountCrossesPoints);
        view.LightUpColorSlots();
    }

    protected void CheckField(IReadOnlyList<SlotStates> Field)
    {
        CheckField(Field, SlotStates.Circle);
        CheckField(Field, SlotStates.Cross);
    }

    private void CheckField(IReadOnlyList<SlotStates> Field, SlotStates slotState)
    {
        if (FieldChecker.Check(Field, slotState, out List<int> WinIndexesSlots))
        {
            model.SetStateWin();
            model.AddPoint(slotState);
            view.DisplayCounters(model.CountCirclesPoints, model.CountCrossesPoints);
            view.PlayWinSound();

            for (int i = 0; i < WinIndexesSlots.Count; i++)
                view.BoomParticleSlot(WinIndexesSlots[i], slotState);

            RestartGame();
        }
    }

    protected async Task WaitForSeconds(int time) => await Task.Delay(time);
}
