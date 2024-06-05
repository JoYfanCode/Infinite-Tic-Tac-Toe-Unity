using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PresenterTwoPlayers : Presenter
{
    protected SlotStates _currentState = SlotStates.Circle;

    public PresenterTwoPlayers(Model model) : base(model) { }

    protected override void TakeTurn(int id)
    {
        List<SlotStates> TempSlotsStates = _model.SlotsStates;

        TempSlotsStates[id] = _currentState;

        EnqueueStateID(id);
        ChangeCurrentState();
        DequeueStateID(ref TempSlotsStates);

        _model.SetState(TempSlotsStates);
        _model.SetStateWinCircle(FieldChecker.Check(TempSlotsStates, SlotStates.Circle));
        _model.SetStateWinCross(FieldChecker.Check(TempSlotsStates, SlotStates.Cross));

    }

    private void EnqueueStateID(int id)
    {
        if (_currentState == SlotStates.Circle)
        {
            _model.QueueCircleID.Enqueue(id);
        }
        else if (_currentState == SlotStates.Cross)
        {
            _model.QueueCrossID.Enqueue(id);
        }
    }
    private void DequeueStateID(ref List<SlotStates> TempSlotsStates)
    {
        if (_currentState == SlotStates.Circle)
        {
            if (_model.QueueCircleID.Count >= _model.LIMIT_QUEUE_ID)
                TempSlotsStates[_model.QueueCircleID.Dequeue()] = SlotStates.Empty;
        }
        else if (_currentState == SlotStates.Cross)
        {
            if (_model.QueueCrossID.Count >= _model.LIMIT_QUEUE_ID)
                TempSlotsStates[_model.QueueCrossID.Dequeue()] = SlotStates.Empty;
        }
    }

    private void ChangeCurrentState()
    {
        if (_currentState == SlotStates.Circle)
            _currentState = SlotStates.Cross;
        else
            _currentState = SlotStates.Circle;
    }
}
