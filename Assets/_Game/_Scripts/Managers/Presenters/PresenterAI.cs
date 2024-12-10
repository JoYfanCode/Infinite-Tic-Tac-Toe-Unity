using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Threading.Tasks;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;

public class PresenterAI : Presenter
{
    protected SlotStates _playerState = SlotStates.Circle;
    protected SlotStates _AIState = SlotStates.Cross;
    protected SlotStates _startState;

    protected AI _AI;

    protected readonly float _AICooldownMin;
    protected readonly float _AICooldownMax;
    protected readonly float _restartGameCooldown;

    protected const int AI_COOLDOWN_MIN_DEFAULT = 500;
    protected const int AI_COOLDOWN_MAX_DEFAULT = 1000;
    protected const int RESTART_GAME_DEFAULT = 5000;
    protected const int WAIT_FIRST_MOVE_TIME_MULT = 2;

    private List<SlotStates> Field => _model.SlotsStates;

    public PresenterAI(Model model, View view, AI AI, float AICooldownMin = AI_COOLDOWN_MIN_DEFAULT, float AICooldownMax = AI_COOLDOWN_MAX_DEFAULT,
                        float restartGameCooldown = RESTART_GAME_DEFAULT) : base(model, view)
    {
        _AI = AI;
        _AICooldownMin = AICooldownMin;
        _AICooldownMax = AICooldownMax;
        _restartGameCooldown = restartGameCooldown;
    }

    public override void OnClotClicked(int id)
    {
        if (Field[id] == SlotStates.Empty && _model.isAIThinking == false)
        {
            if (_model.isGameOn) DoTurn(id);
            if (_model.isGameOn) DoAITurn(_AICooldownMin, _AICooldownMax);
        }
    }

    protected override void DoTurn(int id)
    {
        Field[id] = _playerState;
        EnqueueStateID(_playerState, id);
        DequeueStateID(Field, _playerState);
        CheckField(Field);

        _model.SetState(Field);
        _model.PlusTurn();
    }

    private async void DoAITurn(float AICooldownMin, float AICooldownMax)
    {
        _model.SetIsAIThinking(true);

        int id = _AI.DoTurn(new List<SlotStates>(Field), new Queue<int>(_model.QueueCircleID), new Queue<int>(_model.QueueCrossID), _AIState);

        float AITurnTime = Random.Range(AICooldownMin, AICooldownMax);
        await Task.Run(() => Thread.Sleep((int)AITurnTime));

        Field[id] = _AIState;
        EnqueueStateID(_AIState, id);
        DequeueStateID(Field, _AIState);
        CheckField(Field);

        _model.SetState(Field);
        _model.PlusTurn();
        _model.SetIsAIThinking(false);
        AudioSystem.inst.PlayClickSound();
    }

    private void EnqueueStateID(SlotStates SlotState, int id)
    {
        if (SlotState == SlotStates.Circle)
        {
            _view.BoomParticleSlot(id, SlotStates.Circle);
            _model.QueueCircleID.Enqueue(id);
        }
        else if (SlotState == SlotStates.Cross)
        {
            _view.BoomParticleSlot(id, SlotStates.Cross);
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

        if (SlotState == SlotStates.Circle)
        {
            if (_model.QueueCrossID.Count >= _model.LIMIT_QUEUE_ID)
                _view.LightDownColorSlot(_model.QueueCrossID.Peek());
        }
        else if (SlotState == SlotStates.Cross)
        {
            if (_model.QueueCircleID.Count >= _model.LIMIT_QUEUE_ID)
                _view.LightDownColorSlot(_model.QueueCircleID.Peek());
        }
    }

    public override void FirstMoveDetermination()
    {
        if (NumbericUtilities.RollChance(50))
        {
            _startState = SlotStates.Cross;
            _view.SetTurnState(_startState);
            DoAITurn(WAIT_FIRST_MOVE_TIME_MULT * _AICooldownMin, WAIT_FIRST_MOVE_TIME_MULT * _AICooldownMax);
        }
        else
        {
            _startState = SlotStates.Circle;
            _view.SetTurnState(_startState);
        }
    }

    public override void FirstMoveAnotherPlayer()
    {
        if (_startState == SlotStates.Circle)
        {
            _startState = SlotStates.Cross;

            DoAITurn(_AICooldownMin, _AICooldownMax);
        }
        else if (_startState == SlotStates.Cross)
        {
            _startState = SlotStates.Circle;
        }

        _view.SetTurnState(_startState);
    }

    public override async void RestartGame()
    {
        await Task.Run(() => Thread.Sleep((int)_restartGameCooldown));

        _model.ResetTurns();
        _model.ClearField();
    }
}
