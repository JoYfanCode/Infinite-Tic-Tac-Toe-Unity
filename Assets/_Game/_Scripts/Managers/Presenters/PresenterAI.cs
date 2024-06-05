using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class PresenterAI : Presenter
{
    protected SlotStates _playerState = SlotStates.Circle;
    protected SlotStates _AIState = SlotStates.Cross;

    protected AI _AI;

    protected const int AI_TURN_TIME_MIN = 1500;
    protected const int AI_TURN_TIME_MAX = 3000;

    public PresenterAI(Model model, AI AI) : base(model)
    {
        _AI = AI;
    }

    public override async void OnClotClicked(int id)
    {
        if (_model.SlotsStates[id] == SlotStates.Empty)
        {
            if (_model.isGameOn())
            {
                DoTurn(id);
            }

            if (_model.isGameOn())
            {
                int AITurnTime = Random.Range(AI_TURN_TIME_MIN, AI_TURN_TIME_MAX);
                await Task.Run(() => Thread.Sleep(AITurnTime));
                DoAITurn();
            }
        }
    }

    protected override void DoTurn(int id)
    {
        List<SlotStates> TempField = _model.SlotsStates;

        TempField[id] = _playerState;

        EnqueueStateID(_playerState, id);
        DequeueStateID(ref TempField, _AIState);

        _model.SetState(TempField);
        CheckField(TempField);

        OnTurnDoneEvent(TempField);
    }

    private void DoAITurn()
    {
        List<SlotStates> TempField = _model.SlotsStates;

        int id = _AI.DoTurn(ref TempField, _AIState);

        EnqueueStateID(_AIState, id);
        DequeueStateID(ref TempField, _playerState);

        _model.SetState(TempField);
        CheckField(TempField);

        OnTurnDoneEvent(TempField);
    }

    private void EnqueueStateID(SlotStates Field, int id)
    {
        if (Field == SlotStates.Circle)
        {
            _model.QueueCircleID.Enqueue(id);
        }
        else if (Field == SlotStates.Cross)
        {
            _model.QueueCrossID.Enqueue(id);
        }
    }

    private void DequeueStateID(ref List<SlotStates> Field, SlotStates SlotState)
    {
        if (SlotState == SlotStates.Circle)
        {
            if (_model.QueueCircleID.Count >= _model.LIMIT_QUEUE_ID)
                Field[_model.QueueCircleID.Dequeue()] = SlotStates.Empty;
        }
        else if (SlotState == SlotStates.Cross)
        {
            if (_model.QueueCrossID.Count >= _model.LIMIT_QUEUE_ID)
                Field[_model.QueueCrossID.Dequeue()] = SlotStates.Empty;
        }
    }
}
