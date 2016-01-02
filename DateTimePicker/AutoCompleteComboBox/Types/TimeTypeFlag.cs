using System;

namespace PeteEvans.AutoCompleteComboBox
{
    [Flags]
    public enum TimeTypeFlags
    {
        None = 0,
        PossibleHour = 1,
        DefiniteHour = 2,
        PossibleMinute = 4,
        DefinitieMinute = 8
    }
}