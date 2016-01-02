using System;

namespace PeteEvans.AutoCompleteComboBox
{
    public class TimePartParser
    {
        public TimePartParser(string timePart, int pos, DateTime sourceDate)
        {
            // Check numeric parts
            int intVal;
            if (int.TryParse(timePart, out intVal))
            {
                if (intVal > 23 && intVal <= 59)
                {
                    TimeType = TimeTypeFlags.DefinitieMinute;
                    MinuteValue = intVal;
                    MinuteProbability = 100;
                    return;
                }

                if (intVal >= 0 && intVal <= 23)
                {
                    switch (pos)
                    {
                        case 1:
                            HourProbability = 70;
                            MinuteProbability = 30;
                            break;
                        default:
                            HourProbability = 20;
                            MinuteProbability = 90;
                            break;
                    }
                    HourValue = intVal;
                    MinuteValue = intVal;
                    TimeType = TimeTypeFlags.PossibleHour
                               | TimeTypeFlags.PossibleMinute;
                }
            }
        }

        public TimeTypeFlags TimeType { get; private set; }

        public int HourValue { get; private set; }
        public int HourProbability { get; private set; }

        public int MinuteValue { get; private set; }
        public int MinuteProbability { get; private set; }
    }
}