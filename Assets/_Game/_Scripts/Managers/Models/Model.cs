using System.Collections.Generic;

public abstract class Model
{
    protected List<SlotStates> _slotStates;
    protected Queue<int> _queueCirclesID;
    protected Queue<int> _queueCrossesID;

    protected bool _isAIThinking;
    protected bool _isWinState;

    protected int _countCirclesPoints;
    protected int _countCrossesPoints;

    public int LIMIT_QUEUE_ID = 3;
    public int SLOTS_COUNT = 9;
    public int POINTS_FOR_WIN = 5;

    public List<SlotStates> Field => _slotStates;
    public Queue<int> QueueCircleID => _queueCirclesID;
    public Queue<int> QueueCrossID => _queueCrossesID;
    public bool isAIThinking => _isAIThinking;
    public int CountWinsCircle => _countCirclesPoints;
    public int CountCrossesPoints => _countCrossesPoints;
    public bool isGameState => _isWinState == false;

    public Model()
    {
        _queueCirclesID = new Queue<int>();
        _queueCrossesID = new Queue<int>();
        _slotStates = new List<SlotStates>();

        for (int i = 0; i < SLOTS_COUNT; i++)
            _slotStates.Add(SlotStates.Empty);
    }

    public void SetState(List<SlotStates> Field) => _slotStates = Field;

    public void SetIsAIThinking(bool isAIThinking) => _isAIThinking = isAIThinking;

    public void SetStateWin() => _isWinState = true;

    public void ResetFieldState()
    {
        _queueCirclesID = new Queue<int>();
        _queueCrossesID = new Queue<int>();
        _slotStates = new List<SlotStates>();

        for (int i = 0; i < SLOTS_COUNT; i++)
            _slotStates.Add(SlotStates.Empty);

        _isWinState = false;
    }

    public void AddCirclesPoint() => _countCirclesPoints++;

    public void AddCrossesPoint() => _countCrossesPoints++;

    public void ResetCounters()
    {
        _countCirclesPoints = 0;
        _countCrossesPoints = 0;
    }
}
