using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PresenterTwoPlayers : Presenter
{
    protected SlotStates _currentState = SlotStates.Circle;

    private List<SlotStates> Field => _model.SlotsStates;
    public PresenterTwoPlayers(Model model) : base(model) { }

    protected override void DoTurn(int id)
    {
        Field[id] = _currentState;

        EnqueueStateID(id);
        ChangeCurrentState();
        DequeueStateID(Field);

        _model.SetState(Field);
        CheckField(Field);

        OnTurnDoneEvent(Field);
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

    private void DequeueStateID(List<SlotStates> Field)
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

    public override void FirstMoveDetermination()
    {
        int isFirstCircle = UnityEngine.Random.Range(0, 2);

        if (isFirstCircle == 1)
            _currentState = SlotStates.Circle;
        else
            _currentState = SlotStates.Cross;
    }

}
