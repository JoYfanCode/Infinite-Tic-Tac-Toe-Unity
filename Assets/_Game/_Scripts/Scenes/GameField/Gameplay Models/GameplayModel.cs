using System.Collections.Generic;

public class GameplayModel
{
    protected List<SlotStates> slotStates;
    protected Queue<int> queueCirclesID;
    protected Queue<int> queueCrossesID;

    protected bool isAIThinking;
    protected bool isWinState;

    protected int countCirclesPoints;
    protected int countCrossesPoints;

    public int LIMIT_QUEUE_ID = 3;
    public int SLOTS_COUNT = 9;
    public int POINTS_FOR_WIN = 5;

    public List<SlotStates> Field => new List<SlotStates>(slotStates);
    public Queue<int> QueueCircleID => queueCirclesID;
    public Queue<int> QueueCrossID => queueCrossesID;
    public bool IsAIThinking => isAIThinking;
    public int CountCirclesPoints => countCirclesPoints;
    public int CountCrossesPoints => countCrossesPoints;
    public bool IsGameState => isWinState == false;
    public bool IsCirclesWin => countCirclesPoints == POINTS_FOR_WIN;
    public bool IsCrossesWin => CountCrossesPoints == POINTS_FOR_WIN;
    public bool IsWin => IsCirclesWin || IsCrossesWin;

    public GameplayModel()
    {
        ResetFieldState();
    }

    public void SetState(List<SlotStates> Field) => slotStates = Field;

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
    }

    public void AddCirclesPoint() => countCirclesPoints++;

    public void AddCrossesPoint() => countCrossesPoints++;
    public void ResetCounters()
    {
        countCirclesPoints = 0;
        countCrossesPoints = 0;
    }
}
