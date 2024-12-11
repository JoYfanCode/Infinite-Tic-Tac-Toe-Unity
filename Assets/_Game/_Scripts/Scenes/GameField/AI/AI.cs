using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AI
{
    public abstract int DoTurn(List<SlotStates> Field, Queue<int> queueCirclesID, Queue<int> queueCrossesID, SlotStates AIState);
}
