using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PeteEvans.AutoCompleteComboBox
{
    public class DatePartParser
    {
        private static readonly Dictionary<string, DayOfWeek> DayAbbreviations;
        private static readonly Dictionary<string, int> MonthAbbreviations;

        static DatePartParser()
        {
            var info = DateTimeFormatInfo.CurrentInfo;

            // Setup 3 letter abbreviations
            DayAbbreviations = new Dictionary<string, DayOfWeek>();
            DayOfWeek dayIndex = 0;
            foreach (var day in DateTimeFormatInfo.CurrentInfo.DayNames)
            {
                DayAbbreviations.Add(day.Substring(0, 3).ToLower(), dayIndex++);
            }

            MonthAbbreviations = new Dictionary<string, int>();
            var monthIndex = 1;
            foreach (var month in info.MonthNames)
            {
                if (month.Length >= 3)
                {
                    MonthAbbreviations.Add(month.Substring(0, 3).ToLower(), monthIndex++);
                }
            }
        }

        public DatePartParser(string datePart, int pos, DateTime sourceDate)
        {
            // Check day text
            if (datePart.Length >= 3
                && DayAbbreviations.Keys.Contains(datePart.Substring(0, 3)))
            {
                DateType = DateTypeFlags.DefiniteDayOfWeek;
                DayOfWeekValue = DayAbbreviations[datePart.Substring(0, 3)];
                DayOfWeekProbability = 100;
                return;
            }
            // Check month text
            if (datePart.Length >= 3
                && MonthAbbreviations.Keys.Contains(datePart.Substring(0, 3)))
            {
                DateType = DateTypeFlags.DefinitieMonth;
                MonthValue = MonthAbbreviations[datePart.Substring(0, 3)];
                MonthProbability = 100;
                return;
            }

            // Check numeric parts
            int intVal;
            var century = sourceDate.Year/100*100;
            if (int.TryParse(datePart, out intVal))
            {
                if (intVal > 31 && intVal <= 99)
                {
                    DateType = DateTypeFlags.DefiniteYear;
                    YearValue = intVal + century;
                    YearProbability = 90;
                    return;
                }
                if (intVal > 1000 && intVal < 9999)
                {
                    DateType = DateTypeFlags.DefiniteYear;
                    YearValue = intVal;
                    YearProbability = 100;
                    return;
                }
                if (intVal > 12 && intVal <= 31)
                {
                    switch (pos)
                    {
                        case 1:
                            DayOfMonthProbability = 70;
                            YearProbability = 50;
                            break;
                        case 2:
                            DayOfMonthProbability = 10;
                            YearProbability = 80;
                            break;
                        default:
                            DayOfMonthProbability = 10;
                            YearProbability = 90;
                            break;
                    }
                    YearValue = intVal + century;
                    DayOfMonthValue = intVal;
                    DateType = DateTypeFlags.PossibleDayOfMonth
                               | DateTypeFlags.PossibleYear;
                    return;
                }
                if (intVal > 0 && intVal <= 12)
                {
                    switch (pos)
                    {
                        case 1:
                            DayOfMonthProbability = 60;
                            MonthProbability = 50;
                            YearProbability = 40;
                            break;
                        case 2:
                            DayOfMonthProbability = 30;
                            MonthProbability = 70;
                            YearProbability = 30;
                            break;
                        default:
                            DayOfMonthProbability = 20;
                            MonthProbability = 20;
                            YearProbability = 75;
                            break;
                    }
                    YearValue = intVal + century;
                    MonthValue = intVal;
                    DayOfMonthValue = intVal;
                    DateType = DateTypeFlags.PossibleDayOfMonth
                               | DateTypeFlags.PossibleMonth
                               | DateTypeFlags.PossibleYear;
                }
            }
        }

        public DateTypeFlags DateType { get; private set; }

        public int DayOfMonthValue { get; private set; }
        public int DayOfMonthProbability { get; private set; }

        public DayOfWeek DayOfWeekValue { get; private set; }
        public int DayOfWeekProbability { get; private set; }

        public int MonthValue { get; private set; }
        public int MonthProbability { get; private set; }

        public int YearValue { get; private set; }
        public int YearProbability { get; private set; }
    }
}