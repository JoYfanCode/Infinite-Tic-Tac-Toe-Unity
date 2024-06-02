using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Model
{
    protected View _view;
    protected List<SlotStates> _slotsStates;
    protected bool _isWin;

    public const int SLOTS_COUNT = 9;

    public List<SlotStates> SlotsStates => _slotsStates;

    public Model(View view)
    {
        _view = view;
        _slotsStates = new List<SlotStates>();

        for (int i = 0; i < SLOTS_COUNT; i++)
            _slotsStates.Add(SlotStates.Empty);
    }

    public void SetState(List<SlotStates> slotsStates)
    {
        _slotsStates = slotsStates;
        _view.DisplayField(slotsStates);
    }
    
    public void SetStateResult(bool isWin)
    {
        _isWin = isWin;

        if (isWin)
            _view.DisplayYouWin();
    }
}
