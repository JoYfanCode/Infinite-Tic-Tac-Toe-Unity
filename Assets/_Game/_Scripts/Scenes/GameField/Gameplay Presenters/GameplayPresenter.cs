using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

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
        if (model.IsCirclesWin) view.PlayWinEffects(SlotStates.Circle);
        else if (model.IsCrossesWin) view.PlayWinEffects(SlotStates.Cross);

        await WaitForSeconds(restartGameCooldown);

        if (model.IsWin) model.ResetCounters();

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
        List<int> WinIndexesSlots;

        if (FieldChecker.Check(Field, SlotStates.Circle, out WinIndexesSlots))
        {
            model.SetStateWin();
            model.AddCirclesPoint();
            view.DisplayCounters(model.CountCirclesPoints, model.CountCrossesPoints);
            view.PlayWinSound();

            for (int i = 0; i < WinIndexesSlots.Count; i++)
                view.BoomParticleSlot(WinIndexesSlots[i], SlotStates.Circle);

            RestartGame();
        }
        else if (FieldChecker.Check(Field, SlotStates.Cross, out WinIndexesSlots))
        {
            model.SetStateWin();
            model.AddCrossesPoint();
            view.DisplayCounters(model.CountCirclesPoints, model.CountCrossesPoints);
            view.PlayWinSound();

            for (int i = 0; i < WinIndexesSlots.Count; i++)
                view.BoomParticleSlot(WinIndexesSlots[i], SlotStates.Cross);

            RestartGame();
        }
    }

    protected async Task WaitForSeconds(int time) => await Task.Delay(time);
}
