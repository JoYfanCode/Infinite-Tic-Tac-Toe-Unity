using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIOneTurn : AI
{
    protected List<int> turnsPoints;
    protected int maxTurnPoints;
    protected int maxTurnIndex;

    protected int percentsChanceNoticeWinTurn;
    protected int percentsChanceNoticeDontLoseTurn;

    protected const int WIN_TURN = 100;
    protected const int DONT_LOSE_TURN = 90;
    protected const int MAX_DX_POINTS = 4;

    protected List<SlotStates> Field;

    protected SlotStates AIState;
    protected SlotStates opponentState;

    protected List<AIConfig> configs;

    public AIOneTurn(List<AIConfig> configs) 
    {
        this.configs = configs;
    }

    public void SetAIConfig(AIConfig config)
    {
        percentsChanceNoticeWinTurn = config.PercentsNoticeWinTurn;
        percentsChanceNoticeDontLoseTurn = config.PercentsNoticeDontLoseTurn;
    }

    public override int DoTurn(List<SlotStates> Field, Queue<int> queueCirclesID, Queue<int> queueCrossesID, SlotStates AIState, int countTurns, int dxPoints)
    {
        this.Field = Field;
        this.AIState = AIState;

        SetAIConfig(configs[dxPoints + MAX_DX_POINTS]);

        if (AIState == SlotStates.Circle)
            opponentState = SlotStates.Cross;
        else
            opponentState = SlotStates.Circle;

        turnsPoints = new List<int>();

        for (int i = 0; i < this.Field.Count; i++)
            turnsPoints.Add(0);

        maxTurnPoints = 0;

        CalculateTurns();
        FindMaxPoints();

        if (maxTurnPoints != 0)
        {
            this.Field[maxTurnIndex] = this.AIState;
            return maxTurnIndex;
        }
        else
        {
            return RandomTurn();
        }
    }

    protected void CalculateTurns()
    {
        for (int i = 0; i < Field.Count; i++)
        {
            if (Field[i] == SlotStates.Empty)
            {
                CheckDontLoseTurn(i);
                CheckWinTurn(i);
            }
        }
    }

    protected void CheckDontLoseTurn(int index)
    {
        List<SlotStates> TurnSlotsStates = new List<SlotStates>(Field);
        TurnSlotsStates[index] = opponentState;

        if (FieldChecker.Check(TurnSlotsStates, opponentState))
            if (NumbericUtilities.RollChance(percentsChanceNoticeDontLoseTurn))
                turnsPoints[index] = DONT_LOSE_TURN;
    }

    protected void CheckWinTurn(int index)
    {
        List<SlotStates> TurnSlotsStates = new List<SlotStates>(Field);
        TurnSlotsStates[index] = AIState;

        if (FieldChecker.Check(TurnSlotsStates, AIState))
            if (NumbericUtilities.RollChance(percentsChanceNoticeWinTurn))
                turnsPoints[index] = WIN_TURN;
    }


    protected void FindMaxPoints()
    {
        for (int i = 0; i < Field.Count; i++)
        {
            if (turnsPoints[i] > maxTurnPoints)
            {
                maxTurnPoints = turnsPoints[i];
                maxTurnIndex = i;
            }
        }
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
}
