using System.Collections.Generic;

public abstract class Model
{
    protected View _view;

    protected List<SlotStates> _field;
    protected Queue<int> _queueCirclesID;
    protected Queue<int> _queueCrossesID;

    protected List<int> _turnsList;

    protected bool _isAIThinking;

    protected bool _isWinCircle;
    protected bool _isWinCross;

    protected int _countWinsCircle;
    protected int _countWinsCross;
    protected int _countTurns;

    public int LIMIT_QUEUE_ID = 3;
    public int SLOTS_COUNT = 9;

    public List<SlotStates> SlotsStates => _field;
    public Queue<int> QueueCircleID => _queueCirclesID;
    public Queue<int> QueueCrossID => _queueCrossesID;
    public List<int> TurnsList => _turnsList;
    public bool isAIThinking => _isAIThinking;
    public int CountWinsCircle => _countWinsCircle;
    public int CountWinsCross => _countWinsCross;
    public int CountTurns => _countTurns;
    public bool isGameOn => _isWinCircle == false && _isWinCross == false;

    public Model(View view)
    {
        _view = view;

        _turnsList = new List<int>();
        _queueCirclesID = new Queue<int>();
        _queueCrossesID = new Queue<int>();
        _field = new List<SlotStates>();

        for (int i = 0; i < SLOTS_COUNT; i++)
            _field.Add(SlotStates.Empty);
    }

    public void SetState(List<SlotStates> Field)
    {
        _field = Field;
        _view.DisplayField(Field, CountTurns);
    }

    public void SetIsAIThinking(bool isAIThinking)
    {
        _isAIThinking = isAIThinking;
    }

    public void SetStateWin(SlotStates State)
    {
        if (State == SlotStates.Circle)
        {
            _countWinsCircle++;
            _isWinCircle = true;
            _isWinCross = false;
            _view.DisplayWinCircle(CountWinsCircle);
        }
        else if (State == SlotStates.Cross)
        {
            _countWinsCross++;
            _isWinCross = true;
            _isWinCircle = false;
            _view.DisplayWinCross(CountWinsCross);
        }
        else if (State == SlotStates.Empty)
        {
            _isWinCircle = false;
            _isWinCross = false;
        }

        _turnsList.Add(_countTurns);
        _turnsList.Sort();
        _view.UpdateAverageText(TurnsList);
    }

    public void PlusTurn()
    {
        _countTurns++;
    }

    public void ResetTurns()
    {
        _countTurns = 0;
    }

    public void ClearField()
    {
        _queueCirclesID = new Queue<int>();
        _queueCrossesID = new Queue<int>();
        _field = new List<SlotStates>();

        for (int i = 0; i < SLOTS_COUNT; i++)
            _field.Add(SlotStates.Empty);

        _isWinCircle = false;
        _isWinCross = false;

        _view.DisplayField(_field, CountTurns);
    }
}
