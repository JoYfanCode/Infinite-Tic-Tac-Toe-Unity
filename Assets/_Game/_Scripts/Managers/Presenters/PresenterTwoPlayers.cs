using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PresenterTwoPlayers : Presenter
{
    protected SlotStates _currentState = SlotStates.Circle;

    public PresenterTwoPlayers(Model model) : base(model) { }

    protected override void DoTurn(int id)
    {
        List<SlotStates> TempField = _model.SlotsStates;

        TempField[id] = _currentState;

        EnqueueStateID(id);
        ChangeCurrentState();
        DequeueStateID(ref TempField);

        _model.SetState(TempField);
        CheckField(TempField);

        OnTurnDoneEvent(TempField);
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

    private void DequeueStateID(ref List<SlotStates> Field)
    {
        if (_currentState == SlotStates.Circle)
        {
            if (_model.QueueCircleID.Count >= _model.LIMIT_QUEUE_ID)
                Field[_model.QueueCircleID.Dequeue()] = SlotStates.Empty;
        }
        else if (_currentState == SlotStates.Cross)
        {
            if (_model.QueueCrossID.Count >= _model.LIMIT_QUEUE_ID)
                Field[_model.QueueCrossID.Dequeue()] = SlotStates.Empty;
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
