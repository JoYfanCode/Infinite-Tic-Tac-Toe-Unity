using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using System.Threading.Tasks;
using System.Threading;

public class PresenterTwoPlayers : Presenter
{
    protected SlotStates _currentState = SlotStates.Circle;
    protected SlotStates _startState;

    protected readonly float _restartGameCooldown;

    protected const int RESTART_GAME_DEFAULT = 100;

    private List<SlotStates> Field => _model.SlotsStates;
    public PresenterTwoPlayers(Model model, float restartGameCooldown = RESTART_GAME_DEFAULT) : base(model) 
    {
        _restartGameCooldown = restartGameCooldown;
    }

    protected override void DoTurn(int id)
    {
        Field[id] = _currentState;

        EnqueueStateID(id);
        DequeueStateID(Field);
        ChangeCurrentState();

        _model.SetState(Field);
        _model.PlusTurn();
        CheckField(Field);

        OnTurnDoneEvent(Field, _model.CountTurns);
    }

    private void EnqueueStateID(int id)
    {
        if (_currentState == SlotStates.Circle)
        {
            OnAppearedSlotStateEvent(id, SlotStates.Circle);
            _model.QueueCircleID.Enqueue(id);
        }
        else if (_currentState == SlotStates.Cross)
        {
            OnAppearedSlotStateEvent(id, SlotStates.Cross);
            _model.QueueCrossID.Enqueue(id);
        }
    }

    private void DequeueStateID(List<SlotStates> Field)
    {
        if (_currentState == SlotStates.Circle)
        {
            if (_model.QueueCircleID.Count > _model.LIMIT_QUEUE_ID)
            {
                Field[_model.QueueCircleID.Dequeue()] = SlotStates.Empty;
            }
        }
        else if (_currentState == SlotStates.Cross)
        {
            if (_model.QueueCrossID.Count > _model.LIMIT_QUEUE_ID)
            {
                Field[_model.QueueCrossID.Dequeue()] = SlotStates.Empty;
            }
        }

        if (_currentState == SlotStates.Circle)
        {
            if (_model.QueueCrossID.Count >= _model.LIMIT_QUEUE_ID)
                OnRemovedSlotStateEvent(_model.QueueCrossID.Peek());
        }
        else if (_currentState == SlotStates.Cross)
        {
            if (_model.QueueCircleID.Count >= _model.LIMIT_QUEUE_ID)
                OnRemovedSlotStateEvent(_model.QueueCircleID.Peek());
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
        {
            _startState = SlotStates.Circle;
            _currentState = SlotStates.Circle;
        }
        else
        {
            _startState = SlotStates.Cross;
            _currentState = SlotStates.Cross;
        }

        OnFirstStateDeterminedEvent(_currentState);
    }

    private void FirstMoveAnotherPlayer()
    {
        if (_startState == SlotStates.Circle)
        {
            _startState = SlotStates.Cross;
            _currentState = SlotStates.Cross;
        }
        else if (_startState == SlotStates.Cross)
        {
            _startState = SlotStates.Circle;
            _currentState = SlotStates.Circle;
        }

        OnFirstStateDeterminedEvent(_startState);
    }

    public override async void RestartGame()
    {
        await Task.Run(() => Thread.Sleep((int)_restartGameCooldown));

        _model.ResetTurns();
        _model.ClearField();

        OnRestartedGameEvent();
        OnTurnDoneEvent(Field, _model.CountTurns);
        FirstMoveAnotherPlayer();
    }
}
