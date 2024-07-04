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

    protected const int AI_TURN_TIME_MIN = 250;
    protected const int AI_TURN_TIME_MAX = 750;

    private List<SlotStates> Field => _model.SlotsStates;

    public PresenterAI(Model model, AI AI) : base(model)
    {
        _AI = AI;
    }

    public override async void OnClotClicked(int id)
    {
        if (Field[id] == SlotStates.Empty)
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
        Field[id] = _playerState;
        EnqueueStateID(_playerState, id);
        DequeueStateID(Field, _AIState);

        _model.SetState(Field);
        CheckField(Field);

        OnTurnDoneEvent(Field);
    }

    private void DoAITurn()
    {
        int id = _AI.DoTurn(new List<SlotStates>(Field), new Queue<int>(_model.QueueCircleID), 
            new Queue<int>(_model.QueueCrossID), _AIState);

        Field[id] = _AIState;
        EnqueueStateID(_AIState, id);
        DequeueStateID(Field, _playerState);

        _model.SetState(Field);
        CheckField(Field);

        OnTurnDoneEvent(Field);
    }

    private void EnqueueStateID(SlotStates State, int id)
    {
        if (State == SlotStates.Circle)
        {
            _model.QueueCircleID.Enqueue(id);
        }
        else if (State == SlotStates.Cross)
        {
            _model.QueueCrossID.Enqueue(id);
        }
    }

    private void DequeueStateID(List<SlotStates> Field, SlotStates SlotState)
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

    public override void FirstMoveDetermination()
    {
        int isFirstAI = UnityEngine.Random.Range(0, 2);

        if (isFirstAI == 1)
            DoAITurn();
    }
}
