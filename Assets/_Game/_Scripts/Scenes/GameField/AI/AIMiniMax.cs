using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class AIMiniMax : AI
{
    protected int maxDepth;
    protected int secondTurnDepth = 2;

    protected List<int> turnsPoints;
    protected int MaxTurnPoints;
    protected int MaxTurnIndex;

    protected List<SlotStates> Field;

    protected SlotStates AIState;
    protected SlotStates opponentState;

    protected const int LIMIT_QUEUE_ID = 3;
    protected const int SLOTS_COUNT = 9;

    protected const int WIN_AI = 10;
    protected const int WIN_OPPONENT = -10;

    public AIMiniMax(int depth)
    {
        if (depth >= 1)
            maxDepth = depth;
        else
            maxDepth = 1;
    }

    public override int DoTurn(List<SlotStates> Field, Queue<int> queueCirclesID, Queue<int> queueCrossesID, SlotStates AIState)
    {
        this.Field = Field;
        this.AIState = AIState;

        int BestScore = -1000;
        List<(int, int)> Moves = new List<(int, int)>();

        if (AIState == SlotStates.Circle)
            opponentState = SlotStates.Cross;
        else
            opponentState = SlotStates.Circle;

        if (maxDepth == 1)
            secondTurnDepth = 1;

        int EmptySlots = 0;
        foreach (SlotStates slot in Field)
        {
            if (slot == SlotStates.Empty)
                EmptySlots++;
        }

        if (EmptySlots == 9)
            return Random.Range(0, SLOTS_COUNT);

        MakeTurnsIterations(queueCirclesID, queueCrossesID, Moves, ref BestScore);

        if (BestScore == WIN_OPPONENT)
        {
            int tempDepth = maxDepth;
            maxDepth = secondTurnDepth;
            BestScore = -1000;
            Moves = new List<(int, int)>();
            MakeTurnsIterations(queueCirclesID, queueCrossesID, Moves, ref BestScore);
            maxDepth = tempDepth;
        }

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

                TurnField[i] = AIState;
                EnqueueStateID(AIState, TurnQueueCircleID, TurnQueueCrossID, i);
                DequeueStateID(TurnField, TurnQueueCircleID, TurnQueueCrossID, AIState);

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

        if (depth >= maxDepth)
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

                    TurnField[i] = AIState;
                    EnqueueStateID(AIState, TurnQueueCircleID, TurnQueueCrossID, i);
                    DequeueStateID(TurnField, TurnQueueCircleID, TurnQueueCrossID, AIState);

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

                    TurnField[i] = opponentState;
                    EnqueueStateID(opponentState, TurnQueueCircleID, TurnQueueCrossID, i);
                    DequeueStateID(TurnField, TurnQueueCircleID, TurnQueueCrossID, opponentState);

                    BestScore = Mathf.Min(BestScore, MiniMax(TurnField, TurnQueueCircleID, TurnQueueCrossID, depth + 1, !isMax));
                }
            }
        }

        return BestScore;
    }

    private int EvaluateScore(List<SlotStates> Field)
    {
        if (FieldChecker.Check(Field, AIState))
            return 10;

        if (FieldChecker.Check(Field, opponentState))
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
            if (queueCircleID.Count > LIMIT_QUEUE_ID)
                Field[queueCircleID.Dequeue()] = SlotStates.Empty;
        }
        else if (SlotState == SlotStates.Cross)
        {
            if (queueCrossID.Count > LIMIT_QUEUE_ID)
                Field[queueCrossID.Dequeue()] = SlotStates.Empty;
        }
    }
}
