using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Threading.Tasks;
using System.Threading;

public class PresenterTwoAI : Presenter
{
    protected SlotStates _playerState = SlotStates.Circle;
    protected SlotStates _AIState = SlotStates.Cross;

    protected AI _AI;

    protected readonly float _AICooldownMin;
    protected readonly float _AICooldownMax;
    protected readonly float _restartGameCooldown;

    protected const int AI_COOLDOWN_MIN_DEFAULT = 500;
    protected const int AI_COOLDOWN_MAX_DEFAULT = 1000;
    protected const int RESTART_GAME_DEFAULT = 5000;

    private List<SlotStates> Field => _model.SlotsStates;

    public PresenterTwoAI(Model model, View view, AI AI, float AICooldownMin = AI_COOLDOWN_MIN_DEFAULT, float AICooldownMax = AI_COOLDOWN_MAX_DEFAULT,
                        float restartGameCooldown = RESTART_GAME_DEFAULT) : base(model, view)
    {
        _AI = AI;
        _AICooldownMin = AICooldownMin;
        _AICooldownMax = AICooldownMax;
        _restartGameCooldown = restartGameCooldown;
    }

    private async void Game()
    {
        float AITurnTime = 0;

        while (true)
        {
            AITurnTime = Random.Range(_AICooldownMin, _AICooldownMax);
            await Task.Run(() => Thread.Sleep((int)AITurnTime));

            if (_model.isGameOn)
            {
                DoAITurn(SlotStates.Circle);
            }
            else
            {
                break;
            }

            AITurnTime = Random.Range(_AICooldownMin, _AICooldownMax);
            await Task.Run(() => Thread.Sleep((int)AITurnTime));

            if (_model.isGameOn)
            {
                DoAITurn(SlotStates.Cross);
            }
            else
            {
                break;
            }
        }
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
        CheckField(Field);

        _model.SetState(Field);
        _model.PlusTurn();
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

    public override async void RestartGame()
    {
        await Task.Run(() => Thread.Sleep((int)_restartGameCooldown));

        _model.ResetTurns();
        _model.ClearField();

        _view.LightUpColorSlots();
        _view.SetTurnState(SlotStates.Circle);
        Game();
    }

    public override void FirstMoveDetermination()
    {
        _view.SetTurnState(SlotStates.Circle);
        Game();
    }
}
