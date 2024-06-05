using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AI
{
    public abstract int TakeTurn(ref List<SlotStates> SlotsStates, SlotStates AIState);
}
