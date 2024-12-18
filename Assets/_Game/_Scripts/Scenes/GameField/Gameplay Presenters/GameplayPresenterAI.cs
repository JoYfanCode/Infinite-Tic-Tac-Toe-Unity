using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

public class GameplayPresenterAI : GameplayPresenter
{
    protected SlotStates playerState = SlotStates.Circle;
    protected SlotStates AIState = SlotStates.Cross;
    protected SlotStates startState;

    protected AI AI;

    protected readonly int AICooldownMin;
    protected readonly int AICooldownMax;

    protected const int AI_COOLDOWN_MIN_DEFAULT = 250;
    protected const int AI_COOLDOWN_MAX_DEFAULT = 500;

    public GameplayPresenterAI(GameplayModel model, GameplayView view, AI AI, int restartGameCooldown, int AICooldownMin = AI_COOLDOWN_MIN_DEFAULT,
        int AICooldownMax = AI_COOLDOWN_MAX_DEFAULT) : base(model, view, restartGameCooldown)
    {
        this.AI = AI;
        this.AICooldownMin = AICooldownMin;
        this.AICooldownMax = AICooldownMax;
    }

    public override void OnClotClicked(int id)
    {
        if (model.Field[id] == SlotStates.Empty && model.IsAIThinking == false)
        {
            if (model.IsGameState) DoTurn(id);
            if (model.IsGameState) DoAITurn(AICooldownMin, AICooldownMax);
        }
    }

    protected override void DoTurn(int id)
    {
        List<SlotStates> field = model.Field;

        field[id] = playerState;
        EnqueueStateID(playerState, id);
        DequeueStateID(field, playerState);
        model.SetState(field);
        view.DisplayField(model.Field);

        CheckField(model.Field);
    }

    private async void DoAITurn(int AICooldownMin = 0, int AICooldownMax = 0)
    {
        model.SetIsAIThinking(true);

        int dxPoints = 0;

        if (AIState == SlotStates.Circle)
        {
            dxPoints = model.CountCirclesPoints - model.CountCrossesPoints;
        }
        else if (AIState == SlotStates.Cross)
        {
            dxPoints = model.CountCrossesPoints - model.CountCirclesPoints;
        }

        int id = AI.DoTurn(new List<SlotStates>(model.Field), new Queue<int>(model.QueueCircleID), new Queue<int>(model.QueueCrossID), AIState, model.CountTurns, dxPoints);

        int randomAICooldown = Random.Range(AICooldownMin, AICooldownMax);
        await Task.Delay(randomAICooldown);

        List<SlotStates> field = model.Field;

        field[id] = AIState;
        EnqueueStateID(AIState, id);
        DequeueStateID(field, AIState);
        model.SetState(field);
        view.DisplayField(model.Field);
        model.SetIsAIThinking(false);
        view.PlayClickSound();
        
        CheckField(model.Field);
    }

    private void EnqueueStateID(SlotStates SlotState, int id)
    {
        if (SlotState == SlotStates.Circle)
        {
            view.BoomParticleSlot(id, SlotStates.Circle);
            model.QueueCircleID.Enqueue(id);
        }
        else if (SlotState == SlotStates.Cross)
        {
            view.BoomParticleSlot(id, SlotStates.Cross);
            model.QueueCrossID.Enqueue(id);
        }
    }

    private void DequeueStateID(List<SlotStates> Field, SlotStates SlotState)
    {
        if (SlotState == SlotStates.Circle)
        {
            if (model.QueueCircleID.Count > model.LIMIT_QUEUE_ID)
                Field[model.QueueCircleID.Dequeue()] = SlotStates.Empty;
        }
        else if (SlotState == SlotStates.Cross)
        {
            if (model.QueueCrossID.Count > model.LIMIT_QUEUE_ID)
                Field[model.QueueCrossID.Dequeue()] = SlotStates.Empty;
        }

        if (SlotState == SlotStates.Circle)
        {
            if (model.QueueCrossID.Count >= model.LIMIT_QUEUE_ID)
                view.LightDownColorSlot(model.QueueCrossID.Peek());
        }
        else if (SlotState == SlotStates.Cross)
        {
            if (model.QueueCircleID.Count >= model.LIMIT_QUEUE_ID)
                view.LightDownColorSlot(model.QueueCircleID.Peek());
        }
    }

    public override void FirstMoveDetermination()
    {
        if (NumbericUtilities.RollChance(50))
        {
            startState = SlotStates.Cross;
            view.SetTurnState(startState);
            DoAITurn();
        }
        else
        {
            startState = SlotStates.Circle;
            view.SetTurnState(startState);
        }
    }

    public override void FirstMoveAnotherPlayer()
    {
        if (startState == SlotStates.Circle)
        {
            startState = SlotStates.Cross;

            DoAITurn(AICooldownMin, AICooldownMax);
        }
        else if (startState == SlotStates.Cross)
        {
            startState = SlotStates.Circle;
        }

        view.SetTurnState(startState);
    }

    protected override void PlayWinEffects()
    {
        if (playerState == SlotStates.Circle && model.IsCirclesWin) view.PlayWinEffects(SlotStates.Circle);
        else if (playerState == SlotStates.Cross && model.IsCrossesWin) view.PlayWinEffects(SlotStates.Cross);
    }
}
