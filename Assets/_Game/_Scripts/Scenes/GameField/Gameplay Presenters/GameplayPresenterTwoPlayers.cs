using System.Collections.Generic;

public class GameplayPresenterTwoPlayers : GameplayPresenter
{
    protected SlotStates _currentState = SlotStates.Circle;
    protected SlotStates _startState;

    public GameplayPresenterTwoPlayers(GameplayModel model, GameplayView view, int restartGameCooldown) : base(model, view, restartGameCooldown) { }

    protected override void DoTurn(int id)
    {
        List<SlotStates> field = model.Field;

        field[id] = _currentState;

        EnqueueStateID(id);
        DequeueStateID(field);
        ChangeCurrentState();
        model.SetState(field);
        view.DisplayField(model.Field);

        CheckField(model.Field);
    }

    private void EnqueueStateID(int id)
    {
        if (_currentState == SlotStates.Circle)
        {
            view.BoomParticleSlot(id, SlotStates.Circle);
            model.QueueCircleID.Enqueue(id);
        }
        else if (_currentState == SlotStates.Cross)
        {
            view.BoomParticleSlot(id, SlotStates.Cross);
            model.QueueCrossID.Enqueue(id);
        }
    }

    private void DequeueStateID(List<SlotStates> Field)
    {
        if (_currentState == SlotStates.Circle)
        {
            if (model.QueueCircleID.Count > model.LIMIT_QUEUE_ID)
            {
                Field[model.QueueCircleID.Dequeue()] = SlotStates.Empty;
            }
        }
        else if (_currentState == SlotStates.Cross)
        {
            if (model.QueueCrossID.Count > model.LIMIT_QUEUE_ID)
            {
                Field[model.QueueCrossID.Dequeue()] = SlotStates.Empty;
            }
        }

        if (_currentState == SlotStates.Circle)
        {
            if (model.QueueCrossID.Count >= model.LIMIT_QUEUE_ID)
                view.LightDownColorSlot(model.QueueCrossID.Peek());
        }
        else if (_currentState == SlotStates.Cross)
        {
            if (model.QueueCircleID.Count >= model.LIMIT_QUEUE_ID)
                view.LightDownColorSlot(model.QueueCircleID.Peek());
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

        view.SetTurnState(_currentState);
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

        view.SetTurnState(_startState);
    }

    protected override void PlayWinEffects()
    {
        if (model.IsCirclesWin) view.PlayWinEffects(SlotStates.Circle);
        else if (model.IsCrossesWin) view.PlayWinEffects(SlotStates.Cross);
    }
}
