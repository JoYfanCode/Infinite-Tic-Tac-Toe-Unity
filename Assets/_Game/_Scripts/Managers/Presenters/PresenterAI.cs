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

    protected const int AI_TURN_TIME_MIN = 2500;
    protected const int AI_TURN_TIME_MAX = 4000;

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
                TakeTurn(id);
            }

            if (_model.isGameOn())
            {
                int AITurnTime = Random.Range(AI_TURN_TIME_MIN, AI_TURN_TIME_MAX);
                await Task.Run(() => Thread.Sleep(AITurnTime));
                TakeAITurn();
            }
        }
    }

    protected override void TakeTurn(int id)
    {
        List<SlotStates> TempSlotsStates = _model.SlotsStates;

        TempSlotsStates[id] = _playerState;

        EnqueueStateID(_playerState, id);
        DequeueStateID(ref TempSlotsStates, _AIState);

        _model.SetState(TempSlotsStates);
        _model.SetStateWinCircle(FieldChecker.Check(TempSlotsStates, SlotStates.Circle));
        _model.SetStateWinCross(FieldChecker.Check(TempSlotsStates, SlotStates.Cross));

    }

    private void TakeAITurn()
    {
        List<SlotStates> TempSlotsStates = _model.SlotsStates;

        int id = _AI.TakeTurn(ref TempSlotsStates, _AIState);

        EnqueueStateID(_AIState, id);
        DequeueStateID(ref TempSlotsStates, _playerState);

        _model.SetState(TempSlotsStates);
        _model.SetStateWinCircle(FieldChecker.Check(TempSlotsStates, SlotStates.Circle));
        _model.SetStateWinCross(FieldChecker.Check(TempSlotsStates, SlotStates.Cross));
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
