using System.Collections.Generic;

public static class FieldChecker
{
    public static bool Check(IReadOnlyList<SlotStates> Field, SlotStates State, out List<int> WinIndexesSlots)
    {
        WinIndexesSlots = new List<int>();

        if (Field[0] == State && Field[1] == State && Field[2] == State)
        {
            WinIndexesSlots.Add(0);
            WinIndexesSlots.Add(1);
            WinIndexesSlots.Add(2);
            return true;
        }
        else if (Field[3] == State && Field[4] == State && Field[5] == State)
        {
            WinIndexesSlots.Add(3);
            WinIndexesSlots.Add(4);
            WinIndexesSlots.Add(5);
            return true;
        }
        else if (Field[6] == State && Field[7] == State && Field[8] == State)
        {
            WinIndexesSlots.Add(6);
            WinIndexesSlots.Add(7);
            WinIndexesSlots.Add(8);
            return true;
        }

        if (Field[0] == State && Field[3] == State && Field[6] == State)
        {
            WinIndexesSlots.Add(0);
            WinIndexesSlots.Add(3);
            WinIndexesSlots.Add(6);
            return true;
        }
        else if (Field[1] == State && Field[4] == State && Field[7] == State)
        {
            WinIndexesSlots.Add(1);
            WinIndexesSlots.Add(4);
            WinIndexesSlots.Add(7);
            return true;
        }
        else if (Field[2] == State && Field[5] == State && Field[8] == State)
        {
            WinIndexesSlots.Add(2);
            WinIndexesSlots.Add(5);
            WinIndexesSlots.Add(8);
            return true;
        }

        if (Field[0] == State && Field[4] == State && Field[8] == State)
        {
            WinIndexesSlots.Add(0);
            WinIndexesSlots.Add(4);
            WinIndexesSlots.Add(8);
            return true;
        }
        else if (Field[2] == State && Field[4] == State && Field[6] == State)
        {
            WinIndexesSlots.Add(2);
            WinIndexesSlots.Add(4);
            WinIndexesSlots.Add(6);
            return true;
        }

        return false;
    }

    public static bool Check(IReadOnlyList<SlotStates> Field, SlotStates State)
    {
        if (Field[0] == State && Field[1] == State && Field[2] == State)
        {
            return true;
        }
        else if (Field[3] == State && Field[4] == State && Field[5] == State)
        {
            return true;
        }
        else if (Field[6] == State && Field[7] == State && Field[8] == State)
        {
            return true;
        }

        if (Field[0] == State && Field[3] == State && Field[6] == State)
        {
            return true;
        }
        else if (Field[1] == State && Field[4] == State && Field[7] == State)
        {
            return true;
        }
        else if (Field[2] == State && Field[5] == State && Field[8] == State)
        {
            return true;
        }

        if (Field[0] == State && Field[4] == State && Field[8] == State)
        {
            return true;
        }
        else if (Field[2] == State && Field[4] == State && Field[6] == State)
        {
            return true;
        }

        return false;
    }
}
