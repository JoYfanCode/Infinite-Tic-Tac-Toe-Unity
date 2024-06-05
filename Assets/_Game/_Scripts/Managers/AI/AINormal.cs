using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AINormal : AI
{
    protected List<int> _turnsPoints;
    protected int MaxTurnPoints;
    protected int MaxTurnIndex;

    protected const int SLOTS_COUNT = 9;

    protected const int WIN_TURN = 100;
    protected const int DONT_LOSE_TURN = 90;

    protected List<SlotStates> SlotsStates;
    protected SlotStates _AIState;
    protected SlotStates _playerState;

    public AINormal() { }

    public override int TakeTurn(ref List<SlotStates> SlotsStates, SlotStates AIState)
    {
        this.SlotsStates = SlotsStates;
        _AIState = AIState;

        if (AIState == SlotStates.Circle)
            _playerState = SlotStates.Cross;
        else
            _playerState = SlotStates.Circle;

        _turnsPoints = new List<int>();

        for (int i = 0; i < SLOTS_COUNT; i++)
            _turnsPoints.Add(0);

        MaxTurnPoints = 0;

        CalculateTurns();
        FindMaxPoints();

        if (MaxTurnPoints != 0)
        {
            this.SlotsStates[MaxTurnIndex] = _AIState;
            return MaxTurnIndex;
        }
        else
        {
            return RandomTurn();
        }
    }

    protected void CalculateTurns()
    {
        for (int i = 0; i < SLOTS_COUNT; i++)
        {
            if (SlotsStates[i] == SlotStates.Empty)
            {
                CheckWinTurn(i);
                CheckDontLoseTurn(i);
            }
        }
    }

    protected void CheckWinTurn(int index)
    {
        List<SlotStates> TurnSlotsStates = new List<SlotStates>(SlotsStates);
        TurnSlotsStates[index] = _AIState;

        if (FieldChecker.Check(TurnSlotsStates, _AIState))
            _turnsPoints[index] = WIN_TURN;
    }

    protected void CheckDontLoseTurn(int index)
    {
        List<SlotStates> TurnSlotsStates = new List<SlotStates>(SlotsStates);
        TurnSlotsStates[index] = _playerState;

        if (FieldChecker.Check(TurnSlotsStates, _playerState))
            _turnsPoints[index] = DONT_LOSE_TURN;
    }

    protected void FindMaxPoints()
    {
        for (int i = 0; i < SLOTS_COUNT; i++)
        {
            if (_turnsPoints[i] > MaxTurnPoints)
            {
                MaxTurnPoints = _turnsPoints[i];
                MaxTurnIndex = i;
            }
        }
    }

    protected int RandomTurn()
    {
        List<int> EmptyIndexes = new List<int>();

        for (int i = 0; i < SLOTS_COUNT; i++)
        {
            if (SlotsStates[i] == SlotStates.Empty)
                EmptyIndexes.Add(i);
        }

        int RandomIndex = EmptyIndexes[Random.Range(0, EmptyIndexes.Count)];
        SlotsStates[RandomIndex] = _AIState;

        return RandomIndex;
    }
}
