using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Threading.Tasks;
using System.Threading;

public class PresenterTwoAIFast : Presenter
{
    protected SlotStates _playerState = SlotStates.Circle;
    protected SlotStates _AIState = SlotStates.Cross;

    protected AI _AI;

    protected readonly float _restartCooldown;

    protected const int RESTART_GAME_DEFAULT = 100;

    private List<SlotStates> Field => _model.SlotsStates;

    public PresenterTwoAIFast(Model model, View view, AI AI, float restartCooldown = RESTART_GAME_DEFAULT) : base(model, view)
    {
        _AI = AI;

        _restartCooldown = restartCooldown;
    }

    private void Game()
    {
        while (true)
        {
            if (_model.isGameOn)
            {
                DoAITurn(SlotStates.Circle);
            }
            else
            {
                break;
            }

            if (_model.isGameOn)
            {
                DoAITurn(SlotStates.Cross);
            }
            else
            {
                break;
            }
        }

        RestartGame();
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
        List<SlotStates> field = Field;
        
        await Task.Run(() => Thread.Sleep((int)_restartCooldown));

        _model.ResetTurns();
        _model.ClearField();

        _view.DisplayField(field, _model.CountTurns);
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
