using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Threading.Tasks;
using System.Threading;

public class PresenterTwoAISecond : Presenter
{
    protected SlotStates _playerState = SlotStates.Circle;
    protected SlotStates _AIState = SlotStates.Cross;

    protected AI _AI;

    protected const int AI_TURN_TIME_MIN = 50;
    protected const int AI_TURN_TIME_MAX = 100;

    private List<SlotStates> Field => _model.SlotsStates;

    public PresenterTwoAISecond(Model model, AI AI) : base(model)
    {
        _AI = AI;
    }

    private async void Game()
    {
        int AITurnTime = 0;

        while (true)
        {
            AITurnTime = Random.Range(AI_TURN_TIME_MIN, AI_TURN_TIME_MAX);
            await Task.Run(() => Thread.Sleep(AITurnTime));

            if (_model.isGameOn())
            {
                DoAITurn(SlotStates.Circle);
            }

            AITurnTime = Random.Range(AI_TURN_TIME_MIN, AI_TURN_TIME_MAX);
            await Task.Run(() => Thread.Sleep(AITurnTime));

            if (_model.isGameOn())
            {
                DoAITurn(SlotStates.Cross);
            }
        }
    }

    public override async void OnClotClicked(int id)
    {
        if (Field[id] == SlotStates.Empty)
        {
            if (_model.isGameOn())
            {
                int AITurnTime = Random.Range(AI_TURN_TIME_MIN, AI_TURN_TIME_MAX);
                await Task.Run(() => Thread.Sleep(AITurnTime));
                DoAITurn(SlotStates.Circle);
            }

            if (_model.isGameOn())
            {
                int AITurnTime = Random.Range(AI_TURN_TIME_MIN, AI_TURN_TIME_MAX);
                await Task.Run(() => Thread.Sleep(AITurnTime));
                DoAITurn(SlotStates.Cross);
            }
        }
    }

    protected override void DoTurn(int id)
    {
        Field[id] = _playerState;
        EnqueueStateID(_playerState, id);
        DequeueStateID(Field, _playerState);

        _model.SetState(Field);
        CheckField(Field);

        OnTurnDoneEvent(Field);
    }

    private void DoAITurn(SlotStates AIState)
    {
        int id = _AI.DoTurn(new List<SlotStates>(Field), new Queue<int>(_model.QueueCircleID),
            new Queue<int>(_model.QueueCrossID), AIState);

        Field[id] = AIState;
        EnqueueStateID(AIState, id);
        DequeueStateID(Field, AIState);

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
            if (_model.QueueCircleID.Count > _model.LIMIT_QUEUE_ID)
                Field[_model.QueueCircleID.Dequeue()] = SlotStates.Empty;
        }
        else if (SlotState == SlotStates.Cross)
        {
            if (_model.QueueCrossID.Count > _model.LIMIT_QUEUE_ID)
                Field[_model.QueueCrossID.Dequeue()] = SlotStates.Empty;
        }
    }

    public override void FirstMoveDetermination()
    {
        Game();
    }
}
