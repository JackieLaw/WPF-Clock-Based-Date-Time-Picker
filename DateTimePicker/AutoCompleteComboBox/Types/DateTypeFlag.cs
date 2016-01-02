using System;

namespace PeteEvans.AutoCompleteComboBox
{
    [Flags]
    public enum DateTypeFlags
    {
        None = 0,
        PossibleDayOfMonth = 1,
        DefiniteDayOfMonth = 2,
        DefiniteDayOfWeek = 4,
        PossibleMonth = 8,
        DefinitieMonth = 16,
        PossibleYear = 32,
        DefiniteYear = 64
    }
}