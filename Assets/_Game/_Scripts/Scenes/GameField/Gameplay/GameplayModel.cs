using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public sealed class GameplayModel
{
    public List<SlotStates> Field => new List<SlotStates>(slotStates);
    public Queue<int> QueueCircleID => queueCirclesID;
    public Queue<int> QueueCrossID => queueCrossesID;
    public bool IsAIThinking => isAIThinking;
    public int CountCirclesPoints => circlesPointsModel.CountPointsOn;
    public int CountCrossesPoints => crossesPointsModel.CountPointsOn;
    public bool IsGameState => isWinState == false;
    public bool IsCirclesWin => circlesPointsModel.IsMax;
    public bool IsCrossesWin => crossesPointsModel.IsMax;
    public bool IsWin => IsCirclesWin || IsCrossesWin;
    public int CountTurns => countTurns;

    [HideInInspector] public int LIMIT_QUEUE_ID = 3;
    [HideInInspector] public int SLOTS_COUNT = 9;

    [SerializeField] Queue<int> queueCirclesID;
    [SerializeField] Queue<int> queueCrossesID;

    List<SlotStates> slotStates;
    bool isAIThinking;
    bool isWinState;
    int countTurns;
    PointsModel circlesPointsModel;
    PointsModel crossesPointsModel;

    public GameplayModel()
    {
        ResetFieldState();
    }

    [Inject]
    public void Construct(PointsModel circlesPointsModel, PointsModel crossesPointsModel)
    {
        this.circlesPointsModel = circlesPointsModel;
        this.crossesPointsModel = crossesPointsModel;
    }

    public void SetState(List<SlotStates> Field)
    {
        slotStates = Field;
        countTurns++;
    }

    public void SetIsAIThinking(bool isAIThinking) => this.isAIThinking = isAIThinking;

    public void SetStateWin() => isWinState = true;

    public void ResetFieldState()
    {
        queueCirclesID = new Queue<int>();
        queueCrossesID = new Queue<int>();
        slotStates = new List<SlotStates>();

        for (int i = 0; i < SLOTS_COUNT; i++)
            slotStates.Add(SlotStates.Empty);

        isWinState = false;
        isAIThinking = false;
        countTurns = 0;
    }

    public void AddPoint(SlotStates slotState)
    {
        if (slotState == SlotStates.Circle) circlesPointsModel.AddPointOn();
        else if (slotState == SlotStates.Cross) crossesPointsModel.AddPointOn();
    }

    [Button(ButtonSizes.Large)] void AddCirclePoint() => AddPoint(SlotStates.Circle);
    [Button(ButtonSizes.Large)] void AddCrossPoint() => AddPoint(SlotStates.Cross);

    [Button(ButtonSizes.Large)]
    public void ResetCounters()
    {
        circlesPointsModel.SetPointsOn(0);
        crossesPointsModel.SetPointsOn(0);
    }
}
