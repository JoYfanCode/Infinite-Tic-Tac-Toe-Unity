using System.Collections.Generic;
using UnityEngine;

public sealed class AIOneTurn : AI
{
    List<int> _turnsPoints;
    int _maxTurnPoints;
    int _maxTurnIndex;

    int _percentsChanceNoticeWinTurn;
    int _percentsChanceNoticeDontLoseTurn;

    List<SlotStates> _field;
    SlotStates _AIState;
    SlotStates _opponentState;
    AILevelConfigs _configs;

    const int WIN_TURN = 100;
    const int DONT_LOSE_TURN = 90;
    const int MAX_DX_POINTS = 4;

    public AIOneTurn(AILevelConfigs configs)
    {
        _configs = configs;
    }

    public void SetAIConfig(AIConfig config)
    {
        _percentsChanceNoticeWinTurn = config.PercentsNoticeWinTurn;
        _percentsChanceNoticeDontLoseTurn = config.PercentsNoticeDontLoseTurn;
    }

    public override int DoTurn(List<SlotStates> Field, Queue<int> queueCirclesID, Queue<int> queueCrossesID, SlotStates AIState, int countTurns, int dxPoints)
    {
        _field = Field;
        _AIState = AIState;

        if (dxPoints + MAX_DX_POINTS > 0 && dxPoints + MAX_DX_POINTS < _configs.Count)
            SetAIConfig(_configs.AIConfig(dxPoints + MAX_DX_POINTS));

        if (AIState == SlotStates.Circle)
            _opponentState = SlotStates.Cross;
        else
            _opponentState = SlotStates.Circle;

        _turnsPoints = new List<int>();

        for (int i = 0; i < _field.Count; i++)
            _turnsPoints.Add(0);

        _maxTurnPoints = 0;

        CalculateTurns();
        FindMaxPoints();

        if (_maxTurnPoints != 0)
        {
            _field[_maxTurnIndex] = _AIState;
            return _maxTurnIndex;
        }
        else
        {
            return RandomTurn();
        }
    }

    void CalculateTurns()
    {
        for (int i = 0; i < _field.Count; i++)
        {
            if (_field[i] == SlotStates.Empty)
            {
                CheckDontLoseTurn(i);
                CheckWinTurn(i);
            }
        }
    }

    void CheckDontLoseTurn(int index)
    {
        List<SlotStates> TurnSlotsStates = new List<SlotStates>(_field);
        TurnSlotsStates[index] = _opponentState;

        if (FieldChecker.Check(TurnSlotsStates, _opponentState))
            if (Utilities.RollChance(_percentsChanceNoticeDontLoseTurn))
                _turnsPoints[index] = DONT_LOSE_TURN;
    }

    void CheckWinTurn(int index)
    {
        List<SlotStates> TurnSlotsStates = new List<SlotStates>(_field);
        TurnSlotsStates[index] = _AIState;

        if (FieldChecker.Check(TurnSlotsStates, _AIState))
            if (Utilities.RollChance(_percentsChanceNoticeWinTurn))
                _turnsPoints[index] = WIN_TURN;
    }


    void FindMaxPoints()
    {
        for (int i = 0; i < _field.Count; i++)
        {
            if (_turnsPoints[i] > _maxTurnPoints)
            {
                _maxTurnPoints = _turnsPoints[i];
                _maxTurnIndex = i;
            }
        }
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
}
