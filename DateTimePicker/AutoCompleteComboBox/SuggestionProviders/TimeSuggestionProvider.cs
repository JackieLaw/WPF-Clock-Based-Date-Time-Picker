using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PeteEvans.Extensions;

namespace PeteEvans.AutoCompleteComboBox
{
    /// <summary>
    ///     Provide Time Suggestions
    /// </summary>
    public class TimeSuggestionProvider : ISuggestionProvider
    {
        public IEnumerable<Suggestion> GetFirstSuggestion(string searchText, object currentValue)
        {
            var currentTime = currentValue is DateTime ? (DateTime) currentValue : DateTime.Now;
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
            // Find the parts in the time.
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
                    var part = new TimePartParser(dateParts[0], 1, currentTime);
                    GetSinglePartSuggestions(part, newSuggestions, currentTime);
                    break;

                case 2:
                {
                    var firstPart = new TimePartParser(dateParts[0], 1, currentTime);
                    var secondPart = new TimePartParser(dateParts[1], 2, currentTime);
                    GetTwoPartSuggestions(firstPart, secondPart, newSuggestions, currentTime);
                    break;
                }
            }
            return newSuggestions;
        }

        public IEnumerable<Suggestion> GetNextSuggestion()
        {
            return null;
        }

        private void GetSinglePartSuggestions(TimePartParser part,
            List<Suggestion> newSuggestions,
            DateTime currentTime)
        {
            // Check Possible Hour
            if ((part.TimeType & (TimeTypeFlags.PossibleHour | TimeTypeFlags.DefiniteHour)) > 0)
            {
                // Possible or definite hour
                var suggestedTime = currentTime.SetHour(part.HourValue);
                var probability = part.HourProbability;
                if (currentTime != suggestedTime)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedTime.ToShortTimeString(),
                            "",
                            probability,
                            suggestedTime
                            );
                    newSuggestions.Add(s);
                }
            }

            // Check Minute
            if ((part.TimeType & (TimeTypeFlags.PossibleMinute | TimeTypeFlags.DefinitieMinute)) > 0)
            {
                // Possible or definite minute

                var suggestedTime = currentTime.SetMinute(part.MinuteValue);
                var probability = part.MinuteProbability;
                if (currentTime != suggestedTime)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedTime.ToShortTimeString(),
                            "",
                            probability,
                            suggestedTime
                            );
                    newSuggestions.Add(s);
                }
            }
        }

        private void GetTwoPartSuggestions(TimePartParser firstPart,
            TimePartParser secondPart,
            List<Suggestion> newSuggestions,
            DateTime currentTime)
        {
            // Check Possible Hour and Minute
            if ((firstPart.TimeType & (TimeTypeFlags.PossibleHour | TimeTypeFlags.DefiniteHour)) > 0
                && (secondPart.TimeType & (TimeTypeFlags.PossibleMinute | TimeTypeFlags.DefinitieMinute)) > 0)
            {
                var suggestedTime = currentTime.SetHour(firstPart.HourValue).SetMinute(secondPart.MinuteValue);
                var probability = (firstPart.HourProbability + secondPart.MinuteProbability)/2;
                if (currentTime != suggestedTime)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedTime.ToShortTimeString(),
                            "",
                            probability,
                            suggestedTime
                            );
                    newSuggestions.Add(s);
                }
            }

            // Check Possible Minute and Hour (realy?)
            if ((firstPart.TimeType & (TimeTypeFlags.PossibleMinute | TimeTypeFlags.DefinitieMinute)) > 0
                && (secondPart.TimeType & (TimeTypeFlags.PossibleHour | TimeTypeFlags.DefiniteHour)) > 0)
            {
                var suggestedTime = currentTime.SetHour(secondPart.HourValue).SetMinute(firstPart.MinuteValue);
                var probability = (secondPart.HourProbability + firstPart.MinuteProbability)/2;
                if (currentTime != suggestedTime)
                {
                    var s
                        = new Suggestion
                            (
                            "",
                            suggestedTime.ToShortTimeString(),
                            "",
                            probability,
                            suggestedTime
                            );
                    newSuggestions.Add(s);
                }
            }
        }
    }
}