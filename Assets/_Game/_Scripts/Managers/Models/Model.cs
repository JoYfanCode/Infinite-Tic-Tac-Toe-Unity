using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Model
{
    protected View _view;

    protected List<SlotStates> _slotsStates;
    protected Queue<int> _queueCirclesID;
    protected Queue<int> _queueCrossesID;

    protected bool _isWinCircle;
    protected bool _isWinCross;

    public int LIMIT_QUEUE_ID = 3;
    public int SLOTS_COUNT = 9;
    public bool IsWinCircle => _isWinCircle;
    public bool IsWinCross => _isWinCross;

    public List<SlotStates> SlotsStates => _slotsStates;
    public Queue<int> QueueCircleID => _queueCirclesID;
    public Queue<int> QueueCrossID => _queueCrossesID;

    public Model(View view)
    {
        _view = view;
        _queueCirclesID = new Queue<int>();
        _queueCrossesID = new Queue<int>();
        _slotsStates = new List<SlotStates>();

        for (int i = 0; i < SLOTS_COUNT; i++)
            _slotsStates.Add(SlotStates.Empty);
    }

    public void SetState(List<SlotStates> slotsStates)
    {
        _slotsStates = slotsStates;

        _view.DisplayField(slotsStates);
    }

    public void SetStateWinCircle(bool isWin)
    {
        _isWinCircle = isWin;

        if (isWin)
            _view.DisplayWinCircle();
    }

    public void SetStateWinCross(bool isWin)
    {
        _isWinCross = isWin;

        if (isWin)
            _view.DisplayWinCross();
    }

    public void SetStateWin(bool isWinCircle)
    {
        if (isWinCircle)
        {
            _view.DisplayWinCircle();
            _isWinCircle = true;
            _isWinCross = false;
        }
        else
        {
            _view.DisplayWinCross();
            _isWinCircle = false;
            _isWinCross = true;
        }
    }
}
