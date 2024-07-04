using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class AIMiniMax : AI
{
    protected int _maxDepth = 2;

    protected List<int> _turnsPoints;
    protected int MaxTurnPoints;
    protected int MaxTurnIndex;

    protected const int LIMIT_QUEUE_ID = 3;
    protected const int SLOTS_COUNT = 9;

    protected const int WIN_AI = 10;
    protected const int WIN_OPPONENT = -10;

    protected List<SlotStates> Field;

    protected SlotStates _AIState;
    protected SlotStates _opponentState;

    public AIMiniMax(int depth)
    {
        _maxDepth = depth;
    }

    public override int DoTurn(List<SlotStates> Field, Queue<int> queueCirclesID, Queue<int> queueCrossesID, SlotStates AIState)
    {
        this.Field = Field;
        _AIState = AIState;

        int BestScore = -1000;
        List<(int, int)> Moves = new List<(int, int)>();

        if (AIState == SlotStates.Circle)
            _opponentState = SlotStates.Cross;
        else
            _opponentState = SlotStates.Circle;

        MakeTurnsIterations(queueCirclesID, queueCrossesID, Moves, ref BestScore);

        return RandomBestTurn(Moves, BestScore);
    }

    private void MakeTurnsIterations(Queue<int> queueCirclesID, Queue<int> queueCrossesID, List<(int, int)> Moves, ref int BestScore)
    {
        int MoveScore = 0;

        for (int i = 0; i < SLOTS_COUNT; i++)
        {
            if (Field[i] == SlotStates.Empty)
            {
                List<SlotStates> TurnField = new List<SlotStates>(Field);
                Queue<int> TurnQueueCircleID = new Queue<int>(queueCirclesID);
                Queue<int> TurnQueueCrossID = new Queue<int>(queueCrossesID);

                TurnField[i] = _AIState;
                EnqueueStateID(_AIState, TurnQueueCircleID, TurnQueueCrossID, i);
                DequeueStateID(TurnField, TurnQueueCircleID, TurnQueueCrossID, _opponentState);

                MoveScore = MiniMax(TurnField, TurnQueueCircleID, TurnQueueCrossID, 1, false);

                Moves.Add((i, MoveScore));

                if (MoveScore > BestScore)
                {
                    BestScore = MoveScore;
                }
            }
        }
    }

    private int RandomBestTurn(List<(int, int)> Moves, int BestScore)
    {
        List<int> BestMovesIndexex = new List<int>();

        for (int i = 0; i < Moves.Count; i++)
        {
            if (Moves[i].Item2 == BestScore)
                BestMovesIndexex.Add(Moves[i].Item1);
        }

        return BestMovesIndexex[Random.Range(0, BestMovesIndexex.Count)];
    }

    private int MiniMax(List<SlotStates> Field, Queue<int> queueCirclesID, Queue<int> queueCrossesID,
        int depth, bool isMax)
    {
        int score = EvaluateScore(Field);
        int BestScore = 0;

        if (score == -10 || score == 10)
            return score;

        if (depth >= _maxDepth)
            return 0;

        if (isMax)
        {
            BestScore = -1000;

            for (int i = 0; i < SLOTS_COUNT; i++)
            {
                if (Field[i] == SlotStates.Empty)
                {
                    List<SlotStates> TurnField = new List<SlotStates>(Field);
                    Queue<int> TurnQueueCircleID = new Queue<int>(queueCirclesID);
                    Queue<int> TurnQueueCrossID = new Queue<int>(queueCrossesID);

                    TurnField[i] = _AIState;
                    EnqueueStateID(_AIState, TurnQueueCircleID, TurnQueueCrossID, i);
                    DequeueStateID(TurnField, TurnQueueCircleID, TurnQueueCrossID, _opponentState);

                    BestScore = Mathf.Max(BestScore, MiniMax(TurnField, TurnQueueCircleID, TurnQueueCrossID, depth + 1, !isMax));
                }
            }
        }
        else
        {
            BestScore = 1000;

            for (int i = 0; i < SLOTS_COUNT; i++)
            {
                if (Field[i] == SlotStates.Empty)
                {
                    List<SlotStates> TurnField = new List<SlotStates>(Field);
                    Queue<int> TurnQueueCircleID = new Queue<int>(queueCirclesID);
                    Queue<int> TurnQueueCrossID = new Queue<int>(queueCrossesID);

                    TurnField[i] = _opponentState;
                    EnqueueStateID(_opponentState, TurnQueueCircleID, TurnQueueCrossID, i);
                    DequeueStateID(TurnField, TurnQueueCircleID, TurnQueueCrossID, _AIState);

                    BestScore = Mathf.Min(BestScore, MiniMax(TurnField, TurnQueueCircleID, TurnQueueCrossID, depth + 1, !isMax));
                }
            }
        }

        return BestScore;
    }

    private int EvaluateScore(List<SlotStates> Field)
    {
        if (FieldChecker.Check(Field, _AIState))
            return 10;

        if (FieldChecker.Check(Field, _opponentState))
            return -10;

        return 0;
    }

    private void EnqueueStateID(SlotStates State, Queue<int> queueCircleID, Queue<int> queueCrossID, int id)
    {
        if (State == SlotStates.Circle)
        {
            queueCircleID.Enqueue(id);
        }
        else if (State == SlotStates.Cross)
        {
            queueCrossID.Enqueue(id);
        }
    }

    private void DequeueStateID(List<SlotStates> Field, Queue<int> queueCircleID, Queue<int> queueCrossID, SlotStates SlotState)
    {
        if (SlotState == SlotStates.Circle)
        {
            if (queueCircleID.Count >= LIMIT_QUEUE_ID)
                Field[queueCircleID.Dequeue()] = SlotStates.Empty;
        }
        else if (SlotState == SlotStates.Cross)
        {
            if (queueCrossID.Count >= LIMIT_QUEUE_ID)
                Field[queueCrossID.Dequeue()] = SlotStates.Empty;
        }
    }
}
