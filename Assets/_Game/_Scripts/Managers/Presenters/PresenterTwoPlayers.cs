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

    public PresenterTwoPlayers(Model model, View view, float restartGameCooldown = RESTART_GAME_DEFAULT) : base(model, view, restartGameCooldown) { }

    protected override void DoTurn(int id)
    {
        _model.Field[id] = _currentState;

        EnqueueStateID(id);
        DequeueStateID(_model.Field);
        ChangeCurrentState();
        CheckField(_model.Field);

        _model.SetState(_model.Field);

        _view.DisplayField(_model.Field);
    }

    private void EnqueueStateID(int id)
    {
        if (_currentState == SlotStates.Circle)
        {
            _view.BoomParticleSlot(id, SlotStates.Circle);
            _model.QueueCircleID.Enqueue(id);
        }
        else if (_currentState == SlotStates.Cross)
        {
            _view.BoomParticleSlot(id, SlotStates.Cross);
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
                _view.LightDownColorSlot(_model.QueueCrossID.Peek());
        }
        else if (_currentState == SlotStates.Cross)
        {
            if (_model.QueueCircleID.Count >= _model.LIMIT_QUEUE_ID)
                _view.LightDownColorSlot(_model.QueueCircleID.Peek());
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
        if (NumbericUtilities.RollChance(50))
        {
            _startState = SlotStates.Circle;
            _currentState = SlotStates.Circle;
        }
        else
        {
            _startState = SlotStates.Cross;
            _currentState = SlotStates.Cross;
        }

        _view.SetTurnState(_currentState);
    }

    public override void FirstMoveAnotherPlayer()
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

        _view.SetTurnState(_startState);
    }
}
