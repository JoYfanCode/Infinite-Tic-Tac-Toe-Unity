using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIOneTurn : AI
{
    protected List<int> _turnsPoints;
    protected int MaxTurnPoints;
    protected int MaxTurnIndex;

    protected const int SLOTS_COUNT = 9;

    protected const int WIN_TURN = 100;
    protected const int DONT_LOSE_TURN = 90;

    protected List<SlotStates> Field;

    protected SlotStates AIState;
    protected SlotStates opponentState;

    public override int DoTurn(List<SlotStates> Field, Queue<int> queueCirclesID, Queue<int> queueCrossesID, SlotStates AIState)
    {
        this.Field = Field;
        this.AIState = AIState;

        if (AIState == SlotStates.Circle)
            opponentState = SlotStates.Cross;
        else
            opponentState = SlotStates.Circle;

        _turnsPoints = new List<int>();

        for (int i = 0; i < SLOTS_COUNT; i++)
            _turnsPoints.Add(0);

        MaxTurnPoints = 0;

        CalculateTurns();
        FindMaxPoints();

        if (MaxTurnPoints != 0)
        {
            this.Field[MaxTurnIndex] = this.AIState;
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
            if (Field[i] == SlotStates.Empty)
            {
                CheckWinTurn(i);
                CheckDontLoseTurn(i);
            }
        }
    }

    protected void CheckWinTurn(int index)
    {
        List<SlotStates> TurnSlotsStates = new List<SlotStates>(Field);
        TurnSlotsStates[index] = AIState;

        if (FieldChecker.Check(TurnSlotsStates, AIState))
            _turnsPoints[index] = WIN_TURN;
    }

    protected void CheckDontLoseTurn(int index)
    {
        List<SlotStates> TurnSlotsStates = new List<SlotStates>(Field);
        TurnSlotsStates[index] = opponentState;

        if (FieldChecker.Check(TurnSlotsStates, opponentState))
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
            if (Field[i] == SlotStates.Empty)
                EmptyIndexes.Add(i);
        }

        int RandomIndex = EmptyIndexes[Random.Range(0, EmptyIndexes.Count)];

        return RandomIndex;
    }
}
