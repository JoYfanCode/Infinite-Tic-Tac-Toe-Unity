using System.Collections.Generic;
using System.Threading.Tasks;

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
            PlayWinEffects();
            await Task.Delay(2 * restartGameCooldown);

            if (SetUp.GameMode == GameModes.OnePlayer && model.IsCirclesWin)
            {
                if (SetUp.CurrentLevelIndex == SetUp.CountCompletedLevels)
                {
                    SetUp.CountCompletedLevels++;
                    SetUp.isOpenedNewDifficulty = true;
                    await view.OpenMenuAsync();
                }

                GameAnalyticsManager.inst.OnLevelCompleted(SetUp.CurrentLevelIndex + 1);
            }

            if (SetUp.GameMode == GameModes.OnePlayer && model.IsCrossesWin)
                GameAnalyticsManager.inst.OnLevelFailed(SetUp.CurrentLevelIndex + 1);

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

    public abstract void FirstMoveDetermination();
    public abstract void FirstMoveAnotherPlayer();

    protected abstract void PlayWinEffects();

    public void ResetFieldState()
    {
        model.ResetFieldState();
        view.LightUpColorSlots();
    }

    protected void CheckField(IReadOnlyList<SlotStates> Field)
    {
        CheckField(Field, SlotStates.Circle);
        CheckField(Field, SlotStates.Cross);
    }

    protected abstract void DoTurn(int id);

    private void CheckField(IReadOnlyList<SlotStates> Field, SlotStates slotState)
    {
        if (FieldChecker.Check(Field, slotState, out List<int> WinIndexesSlots))
        {
            model.SetStateWin();
            model.AddPoint(slotState);
            view.PlayWinSound();

            for (int i = 0; i < WinIndexesSlots.Count; i++)
                view.BoomParticleSlot(WinIndexesSlots[i], slotState);

            RestartGame();
        }
    }
}
