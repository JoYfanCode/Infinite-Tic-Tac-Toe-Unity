using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class View : MonoBehaviour
{
    protected Presenter _presenter;

    public void Init(Presenter presenter)
    {
        _presenter = presenter;
    }

    public abstract void DisplayField(List<SlotStates> fieldList);
    public abstract void DisplayYouWin();
    public abstract void DisplayYouLose();
}
