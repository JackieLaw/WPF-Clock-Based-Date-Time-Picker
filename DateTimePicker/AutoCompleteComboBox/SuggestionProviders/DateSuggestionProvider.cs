using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PeteEvans.Extensions;

namespace PeteEvans.AutoCompleteComboBox
{
    /// <summary>
    ///     Provide Data Suggestions
    /// </summary>
    public class DateSuggestionProvider : ISuggestionProvider
    {
        public IEnumerable<Suggestion> GetFirstSuggestion(string searchText, object currentValue)
        {
            var currentDate = currentValue is DateTime ? (DateTime) currentValue : DateTime.Now;
            var newSuggestions = new List<Suggestion>();

            // Ignore if first character is + or -
            if (searchText.Length >= 1)
            {
                var firstCharacter = searchText[0];
                if (firstCharacter == '+' || firstCharacter == '-')
                {
                    return newSuggestions;
                }
            }
            // Find the parts in the date.
            var dateParts = Regex.Split(searchText.ToLower(),
                @"[^a-zA-Z0-9]+").ToList();

            // Remove empty parts
            foreach (var part in dateParts.ToList())
            {
                if (string.IsNullOrWhiteSpace(part)) dateParts.Remove(part);
            }

            switch (dateParts.Count)
            {
                default:
                    return newSuggestions;

                case 1:
                    var part = new DatePartParser(dateParts[0], 1, currentDate);
                    GetSinglePartSuggestions(part, newSuggestions, currentDate);
                    break;

                case 2:
                {
                    var firstPart = new DatePartParser(dateParts[0], 1, currentDate);
                    var secondPart = new DatePartParser(dateParts[1], 2, currentDate);
                    GetTwoPartSuggestions(firstPart, secondPart, newSuggestions, currentDate);
                    break;
                }

                case 3:
                {
                    var firstPart = new DatePartParser(dateParts[0], 1, currentDate);
                    var secondPart = new DatePartParser(dateParts[1], 2, currentDate);
                    var thirdPart = new DatePartParser(dateParts[2], 3, currentDate);
                    GetThreePartSuggestions(firstPart, secondPart, thirdPart, newSuggestions, currentDate);
                }
                    break;
            }
            return newSuggestions;
        }

        public IEnumerable<Suggestion> GetNextSuggestion()
        {
            return null;
        }

        private void GetSinglePartSuggestions(DatePartParser part,
            List<Suggestion> newSuggestions,
            DateTime currentDate)
        {
            // Check Possible Day (Of Month)
            if ((part.DateType & (DateTypeFlags.PossibleDayOfMonth | DateTypeFlags.DefiniteDayOfMonth)) > 0)
            {
                // Possible or definite day of month
                var suggestedDate = currentDate.SetDayOfMonth(part.DayOfMonthValue);
                var probability = part.DayOfMonthProbability;
                if (currentDate != suggestedDate)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedDate.ToShortDateString(),
                            "",
                            probability,
                            suggestedDate
                            );
                    newSuggestions.Add(s);
                }
            }

            // Check Day of Week
            if ((part.DateType & DateTypeFlags.DefiniteDayOfWeek) > 0)
            {
                // Definite day of week

                // Next Day
                var suggestedDate = currentDate.NextDayOfWeek(part.DayOfWeekValue);
                var probability = 70;
                if (currentDate != suggestedDate)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedDate.ToShortDateString(),
                            "",
                            probability,
                            suggestedDate
                            );
                    newSuggestions.Add(s);
                }

                // Previous Day
                suggestedDate = currentDate.PreviousDayOfWeek(part.DayOfWeekValue);
                probability = 40;
                if (currentDate != suggestedDate)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedDate.ToShortDateString(),
                            "",
                            probability,
                            suggestedDate
                            );
                    newSuggestions.Add(s);
                }
            }


            // Check Possible Month
            if ((part.DateType & (DateTypeFlags.PossibleMonth | DateTypeFlags.DefinitieMonth)) > 0)
            {
                // Possible or definite month
                var suggestedDate = currentDate.SetMonth(part.MonthValue);
                var probability = part.MonthProbability;
                if (currentDate != suggestedDate)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedDate.ToShortDateString(),
                            "",
                            probability,
                            suggestedDate
                            );
                    newSuggestions.Add(s);
                }
            }

            // Check Possible Year
            if ((part.DateType & (DateTypeFlags.PossibleYear | DateTypeFlags.DefiniteYear)) > 0)
            {
                // Possible or definite year
                var suggestedDate = currentDate.SetYear(part.YearValue);
                var probability = part.YearProbability;
                if (currentDate != suggestedDate)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedDate.ToShortDateString(),
                            "",
                            probability,
                            suggestedDate
                            );
                    newSuggestions.Add(s);
                }
            }
        }

        private void GetTwoPartSuggestions(DatePartParser firstPart,
            DatePartParser secondPart,
            List<Suggestion> newSuggestions,
            DateTime currentDate)
        {
            // Check Possible Day (Of Month) and Month
            if ((firstPart.DateType & (DateTypeFlags.PossibleDayOfMonth | DateTypeFlags.DefiniteDayOfMonth)) > 0
                && (secondPart.DateType & (DateTypeFlags.PossibleMonth | DateTypeFlags.DefinitieMonth)) > 0)
            {
                var suggestedDate = currentDate.SetDayOfMonth(firstPart.DayOfMonthValue).SetMonth(secondPart.MonthValue);
                var probability = (firstPart.DayOfMonthProbability + secondPart.MonthProbability)/2;
                if (currentDate != suggestedDate)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedDate.ToShortDateString(),
                            "",
                            probability,
                            suggestedDate
                            );
                    newSuggestions.Add(s);
                }
            }

            // Check Possible Month and Day of Month
            if ((secondPart.DateType & (DateTypeFlags.PossibleDayOfMonth | DateTypeFlags.DefiniteDayOfMonth)) > 0
                && (firstPart.DateType & (DateTypeFlags.PossibleMonth | DateTypeFlags.DefinitieMonth)) > 0)
            {
                var suggestedDate = currentDate.SetDayOfMonth(secondPart.DayOfMonthValue).SetMonth(firstPart.MonthValue);
                var probability = (secondPart.DayOfMonthProbability + firstPart.MonthProbability)/2;
                if (currentDate != suggestedDate)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedDate.ToShortDateString(),
                            "",
                            probability,
                            suggestedDate
                            );
                    newSuggestions.Add(s);
                }
            }

            // Check Day of Week and Day (Of Month)
            if ((firstPart.DateType & DateTypeFlags.DefiniteDayOfWeek) > 0
                && (secondPart.DateType & (DateTypeFlags.PossibleDayOfMonth | DateTypeFlags.DefiniteDayOfMonth)) > 0)
            {
                var suggestedDate = currentDate.NextDate(firstPart.DayOfWeekValue, secondPart.DayOfMonthValue);
                var probability = 70;
                if (currentDate != suggestedDate)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedDate.ToShortDateString(),
                            "",
                            probability,
                            suggestedDate
                            );
                    newSuggestions.Add(s);
                }

                suggestedDate = currentDate.PreviousDate(firstPart.DayOfWeekValue, secondPart.DayOfMonthValue);
                probability = 60;
                if (currentDate != suggestedDate)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedDate.ToShortDateString(),
                            "",
                            probability,
                            suggestedDate
                            );
                    newSuggestions.Add(s);
                }
            }

            // Check Possible Month and year
            if ((firstPart.DateType & (DateTypeFlags.PossibleMonth | DateTypeFlags.DefinitieMonth)) > 0
                && (secondPart.DateType & (DateTypeFlags.PossibleYear | DateTypeFlags.DefiniteYear)) > 0)
            {
                var suggestedDate = currentDate.SetMonth(firstPart.MonthValue).SetYear(secondPart.YearValue);
                var probability = (firstPart.MonthProbability + secondPart.YearProbability)/2;
                if (currentDate != suggestedDate)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedDate.ToShortDateString(),
                            "",
                            probability,
                            suggestedDate
                            );
                    newSuggestions.Add(s);
                }
            }
        }

        private void GetThreePartSuggestions(DatePartParser firstPart,
            DatePartParser secondPart,
            DatePartParser thirdPart,
            List<Suggestion> newSuggestions,
            DateTime currentDate)
        {
            // Check Possible Day (Of Month) and Month and Year
            if ((firstPart.DateType & (DateTypeFlags.PossibleDayOfMonth | DateTypeFlags.DefiniteDayOfMonth)) > 0
                && (secondPart.DateType & (DateTypeFlags.PossibleMonth | DateTypeFlags.DefinitieMonth)) > 0
                && (thirdPart.DateType & (DateTypeFlags.PossibleYear | DateTypeFlags.DefiniteYear)) > 0)
            {
                var suggestedDate = currentDate
                    .SetDayOfMonth(firstPart.DayOfMonthValue)
                    .SetMonth(secondPart.MonthValue)
                    .SetYear(thirdPart.YearValue);
                var probability = (firstPart.DayOfMonthProbability
                                   + secondPart.MonthProbability
                                   + thirdPart.YearProbability)/3;
                if (currentDate != suggestedDate)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedDate.ToShortDateString(),
                            "",
                            probability,
                            suggestedDate
                            );
                    newSuggestions.Add(s);
                }
            }

            // Check Possible Month, Day Of Month and Year
            if ((secondPart.DateType & (DateTypeFlags.PossibleDayOfMonth | DateTypeFlags.DefiniteDayOfMonth)) > 0
                && (firstPart.DateType & (DateTypeFlags.PossibleMonth | DateTypeFlags.DefinitieMonth)) > 0
                && (thirdPart.DateType & (DateTypeFlags.PossibleYear | DateTypeFlags.DefiniteYear)) > 0)
            {
                var suggestedDate = currentDate
                    .SetDayOfMonth(secondPart.DayOfMonthValue)
                    .SetMonth(firstPart.MonthValue)
                    .SetYear(thirdPart.YearValue);
                var probability = (secondPart.DayOfMonthProbability
                                   + firstPart.MonthProbability
                                   + thirdPart.YearProbability)/3;
                if (currentDate != suggestedDate)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedDate.ToShortDateString(),
                            "",
                            probability,
                            suggestedDate
                            );
                    newSuggestions.Add(s);
                }
            }
        }
    }
}