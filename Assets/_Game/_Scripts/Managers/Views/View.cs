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
        _presenter.OnTurnDone += ChangeTurnState;
        _presenter.OnCircleWon += DisplayWinCircle;
        _presenter.OnCrossWon += DisplayWinCross;
        _presenter.OnRestartedGame += ClearDisplayWin;

        _presenter.FirstMoveDetermination();
    }

    public virtual void OnDisable()
    {
        _presenter.OnTurnDone -= DisplayField;
        _presenter.OnTurnDone -= ChangeTurnState;
        _presenter.OnCircleWon -= DisplayWinCircle;
        _presenter.OnCrossWon -= DisplayWinCross;
        _presenter.OnRestartedGame -= ClearDisplayWin;
    }

    public abstract void DisplayField(List<SlotStates> Field, int CountTurns);
    public abstract void DisplayWinCircle(int countWins);
    public abstract void DisplayWinCross(int countWins);
    public abstract void ClearDisplayWin();
    public abstract void SetTurnState(SlotStates state);
    public abstract void ChangeTurnState(List<SlotStates> Field, int CountTurns);
}
