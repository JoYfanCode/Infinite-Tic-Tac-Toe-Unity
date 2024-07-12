using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Threading.Tasks;
using System.Threading;

public class PresenterTwoAISecondFast : Presenter
{
    protected SlotStates _playerState = SlotStates.Circle;
    protected SlotStates _AIState = SlotStates.Cross;

    protected AI _AI;

    protected readonly float _restartCooldown;

    private List<SlotStates> Field => _model.SlotsStates;

    public PresenterTwoAISecondFast(Model model, AI AI, float restartCooldown = 2000) : base(model)
    {
        _AI = AI;

        _restartCooldown = restartCooldown;
    }

    private void Game()
    {
        while (true)
        {
            if (_model.isGameOn())
            {
                DoAITurn(SlotStates.Circle);
            }
            else
            {
                break;
            }

            if (_model.isGameOn())
            {
                DoAITurn(SlotStates.Cross);
            }
            else
            {
                break;
            }
        }

        Restart();
    }

    public override void OnClotClicked(int id) { }

    protected override void DoTurn(int id) { }

    private void DoAITurn(SlotStates AIState)
    {
        int id = _AI.DoTurn(new List<SlotStates>(Field), new Queue<int>(_model.QueueCircleID),
            new Queue<int>(_model.QueueCrossID), AIState);

        Field[id] = AIState;
        EnqueueStateID(AIState, id);
        DequeueStateID(Field, AIState);

        _model.SetState(Field);
        CheckField(Field);
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

    public override async void Restart()
    {
        OnTurnDoneEvent(Field);
        await Task.Run(() => Thread.Sleep((int)_restartCooldown));

        _model.ClearField();
        OnRestartedGameEvent();
        Game();
    }

    public override void FirstMoveDetermination()
    {
        OnFirstStateDeterminedEvent(SlotStates.Circle);
        Game();
    }
}
