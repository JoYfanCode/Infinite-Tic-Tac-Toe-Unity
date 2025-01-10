using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameplayPresenterAI : GameplayPresenter
{
    protected SlotStates playerState = SlotStates.Circle;
    protected SlotStates AIState = SlotStates.Cross;
    protected SlotStates startState;

    protected AI AI;
    protected readonly Vector2Int AICooldownRange;

    public GameplayPresenterAI(GameplayModel model, GameplayView view, AI AI, int restartGameCooldown, Vector2Int AICooldownRange) : base(model, view, restartGameCooldown)
    {
        this.AI = AI;
        this.AICooldownRange = AICooldownRange;
    }

    public override void OnClotClicked(int id)
    {
        if (model.Field[id] == SlotStates.Empty && model.IsAIThinking == false)
        {
            if (model.IsGameState) DoTurn(id);
            if (model.IsGameState) DoAITurn(AICooldownRange);
        }
    }


    public override void FirstMoveDetermination()
    {
        if (Utilities.RollChance(50))
        {
            startState = SlotStates.Cross;
            view.SetTurnState(startState);
            DoAITurn(Vector2Int.zero);
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
            DoAITurn(Vector2Int.zero);
        }
        else if (startState == SlotStates.Cross)
        {
            startState = SlotStates.Circle;
        }

        view.SetTurnState(startState);
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

    async void DoAITurn(Vector2Int AICooldownRange)
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

        int randomAICooldown = Random.Range(AICooldownRange.x, AICooldownRange.y);
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

    void EnqueueStateID(SlotStates SlotState, int id)
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

    void DequeueStateID(List<SlotStates> Field, SlotStates SlotState)
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


    protected override void PlayWinEffects()
    {
        if (playerState == SlotStates.Circle && model.IsCirclesWin) view.PlayWinEffects(SlotStates.Circle);
        else if (playerState == SlotStates.Cross && model.IsCrossesWin) view.PlayWinEffects(SlotStates.Cross);
    }
}
