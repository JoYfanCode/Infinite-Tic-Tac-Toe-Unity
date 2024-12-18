using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using static UnityEngine.Rendering.STP;

public struct Turn
{
    public int Index;
    public int Score;

    public Turn(int index, int score)
    {
        Index = index;
        Score = score;
    }
}

public class AIMiniMax : AI
{
    protected int maxDepth;
    protected int loseDepth = 0;
    protected int secondTurnDepth = 2;

    protected int percentDontLose1Depth;
    protected int percentDontLose2Depth;
    protected int percentDontLose3Depth;

    protected int percentNoticeBestTurn;
    protected int percentNoticeSecondBestTurn;

    protected int lose1DepthInTurns;
    protected int lose2DepthInTurns;
    protected int lose3DepthInTurns;
    protected int lose4DepthInTurns;

    protected List<int> turnsPoints;
    protected int maxTurnPoints;
    protected int maxTurnIndex;

    protected List<SlotStates> Field;

    protected SlotStates AIState;
    protected SlotStates opponentState;

    protected int MaxDepth => maxDepth - loseDepth;

    protected const int LIMIT_QUEUE_ID = 3;
    protected const int WIN_AI = 10;
    protected const int NOBODY_WIN = 0;
    protected const int WIN_OPPONENT = -10;
    protected const int MIN_SCORE = -1000;
    protected const int MAX_SCORE = 1000;
    protected const int MAX_DX_POINTS = 4;

    protected List<AIConfig> configs;

    public AIMiniMax(List<AIConfig> configs)
    {
        this.configs = configs;
    }

    public void SetAIConfig(AIConfig config)
    {
        maxDepth = config.MaxDepth;

        percentDontLose1Depth = config.PercentDontLose1Depth;
        percentDontLose2Depth = config.PercentDontLose2Depth;
        percentDontLose3Depth = config.PercentDontLose3Depth;

        percentNoticeBestTurn = config.PercentNoticeBestTurn;
        percentNoticeSecondBestTurn = config.PercentNoticeSecondBestTurn;

        lose1DepthInTurns = config.Lose1DepthInTurns;
        lose2DepthInTurns = config.Lose2DepthInTurns;
        lose3DepthInTurns = config.Lose3DepthInTurns;
        lose4DepthInTurns = config.Lose4DepthInTurns;
    }

    public override int DoTurn(List<SlotStates> Field, Queue<int> queueCirclesID, Queue<int> queueCrossesID, SlotStates AIState, int countTurns, int dxPoints)
    {
        this.Field = Field;
        this.AIState = AIState;

        SetAIConfig(configs[dxPoints + MAX_DX_POINTS]);

        if (countTurns >= lose4DepthInTurns) loseDepth = 4;
        else if (countTurns >= lose3DepthInTurns) loseDepth = 3;
        else if (countTurns >= lose2DepthInTurns) loseDepth = 2;
        else if (countTurns >= lose1DepthInTurns) loseDepth = 1;
        else loseDepth = 0;

        if (NumbericUtilities.RollChance(100 - percentDontLose3Depth)) loseDepth += 3;
        else if (NumbericUtilities.RollChance(100 - percentDontLose2Depth)) loseDepth += 2;
        else if (NumbericUtilities.RollChance(100 - percentDontLose1Depth)) loseDepth += 1;

        int BestScore = MIN_SCORE;
        List<Turn> Turns = new List<Turn>();

        if (AIState == SlotStates.Circle) opponentState = SlotStates.Cross;
        else if (AIState == SlotStates.Cross) opponentState = SlotStates.Circle;

        if (MaxDepth == 1)
            secondTurnDepth = 1;

        int EmptySlots = 0;
        foreach (SlotStates slot in Field)
        {
            if (slot == SlotStates.Empty)
                EmptySlots++;
        }

        if (EmptySlots == this.Field.Count)
            return Random.Range(0, this.Field.Count);

        MakeTurnsIterations(queueCirclesID, queueCrossesID, Turns, ref BestScore);

        if (BestScore == WIN_OPPONENT)
        {
            int tempDepth = maxDepth;
            loseDepth = 0;
            maxDepth = secondTurnDepth;
            BestScore = MAX_SCORE;
            Turns = new List<Turn>();
            MakeTurnsIterations(queueCirclesID, queueCrossesID, Turns, ref BestScore);
            maxDepth = tempDepth;
            secondTurnDepth = 2;
        }

        FindThreeBestScores(Turns, out int FirstBestScore, out int SecondBestScore, out int ThirdBestScore);

        if (NumbericUtilities.RollChance(percentNoticeBestTurn))
            return RandomBestTurn(Turns, FirstBestScore);
        else if (NumbericUtilities.RollChance(percentNoticeSecondBestTurn))
            return RandomBestTurn(Turns, SecondBestScore);
        else
            return RandomBestTurn(Turns, ThirdBestScore);
    }

    private void MakeTurnsIterations(Queue<int> queueCirclesID, Queue<int> queueCrossesID, List<Turn> Moves, ref int BestScore)
    {
        int MoveScore = 0;

        for (int i = 0; i < Field.Count; i++)
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

                Moves.Add(new Turn(i, MoveScore));

                if (MoveScore > BestScore) BestScore = MoveScore;
            }
        }
    }

    private void FindThreeBestScores(List<Turn> Moves, out int FirstBestScore, out int SecondBestScore, out int ThirdBestScore)
    {
        FirstBestScore = MIN_SCORE;
        SecondBestScore = MIN_SCORE;
        ThirdBestScore = MIN_SCORE;

        for (int i = 0; i < Moves.Count; i++)
        {
            if (Moves[i].Score > FirstBestScore)
            {
                ThirdBestScore = SecondBestScore;
                SecondBestScore = FirstBestScore;
                FirstBestScore = Moves[i].Score;
            }
        }
    }

    private int RandomBestTurn(List<Turn> Moves, int BestScore)
    {
        List<int> BestMovesIndexex = new List<int>();

        for (int i = 0; i < Moves.Count; i++)
        {
            if (Moves[i].Score == BestScore)
                BestMovesIndexex.Add(Moves[i].Index);
        }

        if (BestMovesIndexex.Count > 0)
            return BestMovesIndexex[Random.Range(0, BestMovesIndexex.Count)];
        else
            return RandomTurn();
    }

    protected int RandomTurn()
    {
        List<int> EmptyIndexes = new List<int>();

        for (int i = 0; i < Field.Count; i++)
        {
            if (Field[i] == SlotStates.Empty)
                EmptyIndexes.Add(i);
        }

        int RandomIndex = EmptyIndexes[Random.Range(0, EmptyIndexes.Count)];

        return RandomIndex;
    }

    private int MiniMax(List<SlotStates> Field, Queue<int> queueCirclesID, Queue<int> queueCrossesID,
        int depth, bool isMax)
    {
        int score = EvaluateScore(Field);
        int BestScore = 0;

        if (score == WIN_OPPONENT || score == WIN_AI)
            return score;

        if (depth >= MaxDepth)
            return 0;

        if (isMax)
        {
            BestScore = MIN_SCORE;

            for (int i = 0; i < Field.Count; i++)
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
            BestScore = MAX_SCORE;

            for (int i = 0; i < Field.Count; i++)
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
            return WIN_AI;

        if (FieldChecker.Check(Field, opponentState))
            return WIN_OPPONENT;

        return NOBODY_WIN;
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
