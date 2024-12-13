using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class GameplayPresenter
{
    protected readonly GameplayModel model;
    protected readonly GameplayView view;

    protected readonly int restartGameCooldown;

    public GameplayPresenter(GameplayModel model, GameplayView view, int restartGameCooldown)
    {
        this.model = model;
        this.view = view;
        this.restartGameCooldown = restartGameCooldown;
    }

    public virtual void OnClotClicked(int id)
    {
        if (model.Field[id] == SlotStates.Empty && model.IsGameState)
        {
            DoTurn(id);
        }
    }

    public async void RestartGame()
    {
        if (model.IsWin)
        {
            await Task.Delay(restartGameCooldown / 2);

            if (model.IsCirclesWin) view.PlayWinEffects(SlotStates.Circle);
            else if (model.IsCrossesWin) view.PlayWinEffects(SlotStates.Cross);

            await Task.Delay(2 * restartGameCooldown);

            if (SetUp.GameMode == GameModes.OnePlayer && model.IsCirclesWin)
            {
                if (SetUp.AIDifficultiesComplited[SetUp.AIDifficulty] == false)
                {
                    SetUp.AIDifficultiesComplited[SetUp.AIDifficulty] = true;
                    SetUp.isOpenedNewDifficulty = true;
                    view.OpenMenu();
                }
            }

            model.ResetCounters();
        }
        else
        {
            await Task.Delay(restartGameCooldown);
        }

        await view.ClearFieldAnimation();

        ResetFieldState();
        FirstMoveAnotherPlayer();
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

    protected abstract void DoTurn(int id);
    public abstract void FirstMoveDetermination();
    public abstract void FirstMoveAnotherPlayer();
}
