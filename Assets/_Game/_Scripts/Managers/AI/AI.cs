using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AI
{
    public abstract int DoTurn(ref List<SlotStates> Field, SlotStates AIState);
}
