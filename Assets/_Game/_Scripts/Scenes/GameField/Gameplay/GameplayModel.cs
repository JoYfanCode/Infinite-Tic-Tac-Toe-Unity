using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public sealed class GameplayModel
{
    private List<SlotStates> slotStates;
    [SerializeField] private Queue<int> queueCirclesID;
    [SerializeField] private Queue<int> queueCrossesID;

    private bool isAIThinking;
    private bool isWinState;

    private int countTurns;

    private PointsModel circlesPointsModel;
    private PointsModel crossesPointsModel;

    [HideInInspector] public int LIMIT_QUEUE_ID = 3;
    [HideInInspector] public int SLOTS_COUNT = 9;

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

    [Button(ButtonSizes.Large)] private void AddCirclePoint() => AddPoint(SlotStates.Circle);
    [Button(ButtonSizes.Large)] private void AddCrossPoint() => AddPoint(SlotStates.Cross);

    [Button(ButtonSizes.Large)]
    public void ResetCounters()
    {
        circlesPointsModel.SetPointsOn(0);
        crossesPointsModel.SetPointsOn(0);
    }
}
