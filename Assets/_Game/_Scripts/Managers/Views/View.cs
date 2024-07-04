using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class View : MonoBehaviour
{
    protected Presenter _presenter;

    public virtual void Init(Presenter presenter)
    {
        _presenter = presenter;

        _presenter.OnTurnDone += DisplayField;
        _presenter.OnCircleWon += DisplayWinCircle;
        _presenter.OnCrossWon += DisplayWinCross;

        _presenter.FirstMoveDetermination();
    }

    public void OnDisable()
    {
        _presenter.OnTurnDone -= DisplayField;
        _presenter.OnCircleWon -= DisplayWinCircle;
        _presenter.OnCrossWon -= DisplayWinCross;
    }

    public abstract void DisplayField(List<SlotStates> Field);
    public abstract void DisplayWinCircle();
    public abstract void DisplayWinCross();
}
