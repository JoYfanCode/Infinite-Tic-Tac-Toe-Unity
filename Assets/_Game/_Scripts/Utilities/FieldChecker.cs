using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class FieldChecker
{
    public static bool Check(List<SlotStates> Field, SlotStates State)
    {
        if (Field[0] == State && Field[1] == State && Field[2] == State)
            return true;
        else if (Field[3] == State && Field[4] == State && Field[5] == State)
            return true;
        else if (Field[6] == State && Field[7] == State && Field[8] == State)
            return true;

        if (Field[0] == State && Field[3] == State && Field[6] == State)
            return true;
        else if (Field[1] == State && Field[4] == State && Field[7] == State)
            return true;
        else if (Field[2] == State && Field[5] == State && Field[8] == State)
            return true;

        if (Field[0] == State && Field[4] == State && Field[8] == State)
            return true;
        else if (Field[2] == State && Field[4] == State && Field[6] == State)
            return true;

        return false;
    }
}
