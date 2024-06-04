using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PresenterAI : Presenter
{
    protected SlotStates _playerState = SlotStates.Circle;
    protected SlotStates _AIState = SlotStates.Cross;

    public PresenterAI(Model model) : base(model) { }

    public virtual void OnClotClicked(int id)
    {
        if (_model.IsWinCircle == false && _model.IsWinCross == false)
            if (_model.SlotsStates[id] == SlotStates.Empty)
            {
                TakeTurn(id);
                TakeAITurn();
            }
    }

    protected override void TakeTurn(int id)
    {
        List<SlotStates> TempSlotsStates = _model.SlotsStates;

        TempSlotsStates[id] = _playerState;

        EnqueueStateID(_playerState, id);
        DequeueStateID(ref TempSlotsStates, _playerState);

        _model.SetState(TempSlotsStates);
        _model.SetStateWinCircle(CheckField(SlotStates.Circle));
        _model.SetStateWinCross(CheckField(SlotStates.Cross));

    }

    private void TakeAITurn()
    {
        List<SlotStates> TempSlotsStates = _model.SlotsStates;

        // TODO: AI Algorithm
    }

    private void EnqueueStateID(SlotStates SlotState, int id)
    {
        if (SlotState == SlotStates.Circle)
        {
            _model.QueueCircleID.Enqueue(id);
        }
        else if (SlotState == SlotStates.Cross)
        {
            _model.QueueCrossID.Enqueue(id);
        }
    }
    private void DequeueStateID(ref List<SlotStates> TempSlotsStates, SlotStates SlotState)
    {
        if (SlotState == SlotStates.Circle)
        {
            if (_model.QueueCircleID.Count >= _model.LIMIT_QUEUE_ID)
                TempSlotsStates[_model.QueueCircleID.Dequeue()] = SlotStates.Empty;
        }
        else if (SlotState == SlotStates.Cross)
        {
            if (_model.QueueCrossID.Count >= _model.LIMIT_QUEUE_ID)
                TempSlotsStates[_model.QueueCrossID.Dequeue()] = SlotStates.Empty;
        }
    }
}
