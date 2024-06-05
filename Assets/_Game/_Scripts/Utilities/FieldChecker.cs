using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class FieldChecker
{
    public static bool Check(List<SlotStates> SlotsStates, SlotStates State)
    {
        if (SlotsStates[0] == State && SlotsStates[1] == State && SlotsStates[2] == State)
            return true;
        else if (SlotsStates[3] == State && SlotsStates[4] == State && SlotsStates[5] == State)
            return true;
        else if (SlotsStates[6] == State && SlotsStates[7] == State && SlotsStates[8] == State)
            return true;

        if (SlotsStates[0] == State && SlotsStates[3] == State && SlotsStates[6] == State)
            return true;
        else if (SlotsStates[1] == State && SlotsStates[4] == State && SlotsStates[7] == State)
            return true;
        else if (SlotsStates[2] == State && SlotsStates[5] == State && SlotsStates[8] == State)
            return true;

        if (SlotsStates[0] == State && SlotsStates[4] == State && SlotsStates[8] == State)
            return true;
        else if (SlotsStates[2] == State && SlotsStates[4] == State && SlotsStates[6] == State)
            return true;

        return false;
    }
}
