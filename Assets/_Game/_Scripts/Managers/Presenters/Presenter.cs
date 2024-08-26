using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

public abstract class Presenter
{
    protected Model _model;

    public event Action<List<SlotStates>, int> OnTurnDone;
    public event Action<int> OnCircleWon;
    public event Action<int> OnCrossWon;
    public event Action OnRestartedGame;
    public event Action<SlotStates> OnFirstStateDetermined;
    public event Action<List<int>> OnGameOver;
    public event Action<int> OnRemovedSlotState;
    public event Action<int, SlotStates> OnAppearedSlotState;

    public Presenter(Model model)
    {
        _model = model;
    }

    protected void OnTurnDoneEvent(List<SlotStates> Field, int CountTurns)
        => OnTurnDone?.Invoke(Field, CountTurns);

    protected void OnCircleWonEvent(int count)
        => OnCircleWon?.Invoke(count);

    protected void OnCrossWonEvent(int count)
        => OnCrossWon?.Invoke(count);

    protected void OnRestartedGameEvent()
        => OnRestartedGame?.Invoke();

    protected void OnFirstStateDeterminedEvent(SlotStates state)
        => OnFirstStateDetermined?.Invoke(state);

    protected void OnGameOverEvent(List<int> turnsList)
        => OnGameOver?.Invoke(turnsList);

    protected void OnRemovedSlotStateEvent(int indexSlot)
        => OnRemovedSlotState?.Invoke(indexSlot);

    protected void OnAppearedSlotStateEvent(int indexSlot, SlotStates slotState)
        => OnAppearedSlotState?.Invoke(indexSlot, slotState);

    public virtual void OnClotClicked(int id)
    {
        if (_model.SlotsStates[id] == SlotStates.Empty)
            if (_model.isGameOn)
                DoTurn(id);
    }

    protected abstract void DoTurn(int id);

    public abstract void FirstMoveDetermination();

    public abstract void RestartGame();

    protected void CheckField(List<SlotStates> Field)
    {
        List<int> WinIndexesSlots;

        if (FieldChecker.Check(Field, SlotStates.Circle, out WinIndexesSlots))
        {
            _model.SetStateWin(SlotStates.Circle);
            OnCircleWonEvent(_model.CountWinsCircle);

            for (int i = 0; i < WinIndexesSlots.Count; i++)
                OnAppearedSlotStateEvent(WinIndexesSlots[i], SlotStates.Circle);

            OnGameOverEvent(_model.TurnsList);
            RestartGame();
        }
        else if (FieldChecker.Check(Field, SlotStates.Cross, out WinIndexesSlots))
        {
            _model.SetStateWin(SlotStates.Cross);
            OnCrossWonEvent(_model.CountWinsCross);

            for (int i = 0; i < WinIndexesSlots.Count; i++)
                OnAppearedSlotStateEvent(WinIndexesSlots[i], SlotStates.Cross);

            OnGameOverEvent(_model.TurnsList);
            RestartGame();
        }
    }
}
