using System.Collections.Generic;
using UnityEngine;

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

public sealed class AIMiniMax : AI
{
    int _maxDepth;
    int _loseDepth = 0;
    int _secondTurnDepth = 2;

    int _percentDontLose1Depth;
    int _percentDontLose2Depth;
    int _percentDontLose3Depth;
    int _percentNoticeBestTurn;
    int _percentNoticeSecondBestTurn;

    int _lose1DepthInTurns;
    int _lose2DepthInTurns;
    int _lose3DepthInTurns;
    int _lose4DepthInTurns;

    List<int> _turnsPoints;
    int _maxTurnPoints;
    int _maxTurnIndex;

    List<SlotStates> _field;
    SlotStates _AIState;
    SlotStates _opponentState;
    AILevelConfigs _configs;

    int MaxDepth => _maxDepth - _loseDepth;

    const int LIMIT_QUEUE_ID = 3;
    const int WIN_AI = 10;
    const int NOBODY_WIN = 0;
    const int WIN_OPPONENT = -10;
    const int MIN_SCORE = -1000;
    const int MAX_SCORE = 1000;
    const int MAX_DX_POINTS = 4;


    public AIMiniMax(AILevelConfigs configs)
    {
        this._configs = configs;
    }

    public void SetAIConfig(AIConfig config)
    {
        _maxDepth = config.MaxDepth;

        _percentDontLose1Depth = config.PercentDontLose1Depth;
        _percentDontLose2Depth = config.PercentDontLose2Depth;
        _percentDontLose3Depth = config.PercentDontLose3Depth;

        _percentNoticeBestTurn = config.PercentNoticeBestTurn;
        _percentNoticeSecondBestTurn = config.PercentNoticeSecondBestTurn;

        _lose1DepthInTurns = config.Lose1DepthInTurns;
        _lose2DepthInTurns = config.Lose2DepthInTurns;
        _lose3DepthInTurns = config.Lose3DepthInTurns;
        _lose4DepthInTurns = config.Lose4DepthInTurns;
    }

    public override int DoTurn(List<SlotStates> Field, Queue<int> queueCirclesID, Queue<int> queueCrossesID, SlotStates AIState, int countTurns, int dxPoints)
    {
        this._field = Field;
        this._AIState = AIState;

        if (dxPoints + MAX_DX_POINTS > 0 && dxPoints + MAX_DX_POINTS < _configs.Count)
            SetAIConfig(_configs.AIConfig(dxPoints + MAX_DX_POINTS));

        InitLoseDepth(countTurns);

        int BestScore = MIN_SCORE;
        List<Turn> Turns = new List<Turn>();

        if (AIState == SlotStates.Circle) _opponentState = SlotStates.Cross;
        else if (AIState == SlotStates.Cross) _opponentState = SlotStates.Circle;

        if (MaxDepth == 1)
            _secondTurnDepth = 1;

        int EmptySlots = 0;
        foreach (SlotStates slot in Field)
        {
            if (slot == SlotStates.Empty)
                EmptySlots++;
        }

        if (EmptySlots == this._field.Count)
            return Random.Range(0, this._field.Count);

        MakeTurnsIterations(queueCirclesID, queueCrossesID, Turns, ref BestScore);

        if (BestScore == WIN_OPPONENT)
        {
            int tempDepth = _maxDepth;
            _loseDepth = 0;
            _maxDepth = _secondTurnDepth;
            BestScore = MAX_SCORE;
            Turns = new List<Turn>();
            MakeTurnsIterations(queueCirclesID, queueCrossesID, Turns, ref BestScore);
            _maxDepth = tempDepth;
            _secondTurnDepth = 2;
        }

        FindThreeBestScores(Turns, out int FirstBestScore, out int SecondBestScore, out int ThirdBestScore);

        if (Utilities.RollChance(_percentNoticeBestTurn))
            return RandomBestTurn(Turns, FirstBestScore);
        else if (Utilities.RollChance(_percentNoticeSecondBestTurn))
            return RandomBestTurn(Turns, SecondBestScore);
        else
            return RandomBestTurn(Turns, ThirdBestScore);
    }

    void InitLoseDepth(int countTurns)
    {
        if (countTurns >= _lose4DepthInTurns) _loseDepth = 4;
        else if (countTurns >= _lose3DepthInTurns) _loseDepth = 3;
        else if (countTurns >= _lose2DepthInTurns) _loseDepth = 2;
        else if (countTurns >= _lose1DepthInTurns) _loseDepth = 1;
        else _loseDepth = 0;

        if (Utilities.RollChance(100 - _percentDontLose3Depth)) _loseDepth += 3;
        else if (Utilities.RollChance(100 - _percentDontLose2Depth)) _loseDepth += 2;
        else if (Utilities.RollChance(100 - _percentDontLose1Depth)) _loseDepth += 1;
    }

    void MakeTurnsIterations(Queue<int> queueCirclesID, Queue<int> queueCrossesID, List<Turn> Moves, ref int BestScore)
    {
        int MoveScore = 0;

        for (int i = 0; i < _field.Count; i++)
        {
            if (_field[i] == SlotStates.Empty)
            {
                List<SlotStates> TurnField = new List<SlotStates>(_field);
                Queue<int> TurnQueueCircleID = new Queue<int>(queueCirclesID);
                Queue<int> TurnQueueCrossID = new Queue<int>(queueCrossesID);

                TurnField[i] = _AIState;
                EnqueueStateID(_AIState, TurnQueueCircleID, TurnQueueCrossID, i);
                DequeueStateID(TurnField, TurnQueueCircleID, TurnQueueCrossID, _AIState);

                MoveScore = MiniMax(TurnField, TurnQueueCircleID, TurnQueueCrossID, 1, false);

                Moves.Add(new Turn(i, MoveScore));

                if (MoveScore > BestScore) BestScore = MoveScore;
            }
        }
    }

    void FindThreeBestScores(List<Turn> Moves, out int FirstBestScore, out int SecondBestScore, out int ThirdBestScore)
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

    int RandomBestTurn(List<Turn> Moves, int BestScore)
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

    int RandomTurn()
    {
        List<int> EmptyIndexes = new List<int>();

        for (int i = 0; i < _field.Count; i++)
        {
            if (_field[i] == SlotStates.Empty)
                EmptyIndexes.Add(i);
        }

        int RandomIndex = EmptyIndexes[Random.Range(0, EmptyIndexes.Count)];

        return RandomIndex;
    }

    int MiniMax(List<SlotStates> Field, Queue<int> queueCirclesID, Queue<int> queueCrossesID,
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

                    TurnField[i] = _AIState;
                    EnqueueStateID(_AIState, TurnQueueCircleID, TurnQueueCrossID, i);
                    DequeueStateID(TurnField, TurnQueueCircleID, TurnQueueCrossID, _AIState);

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

                    TurnField[i] = _opponentState;
                    EnqueueStateID(_opponentState, TurnQueueCircleID, TurnQueueCrossID, i);
                    DequeueStateID(TurnField, TurnQueueCircleID, TurnQueueCrossID, _opponentState);

                    BestScore = Mathf.Min(BestScore, MiniMax(TurnField, TurnQueueCircleID, TurnQueueCrossID, depth + 1, !isMax));
                }
            }
        }

        return BestScore;
    }

    int EvaluateScore(List<SlotStates> Field)
    {
        if (FieldChecker.Check(Field, _AIState))
            return WIN_AI;

        if (FieldChecker.Check(Field, _opponentState))
            return WIN_OPPONENT;

        return NOBODY_WIN;
    }

    void EnqueueStateID(SlotStates State, Queue<int> queueCircleID, Queue<int> queueCrossID, int id)
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

    void DequeueStateID(List<SlotStates> Field, Queue<int> queueCircleID, Queue<int> queueCrossID, SlotStates SlotState)
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
