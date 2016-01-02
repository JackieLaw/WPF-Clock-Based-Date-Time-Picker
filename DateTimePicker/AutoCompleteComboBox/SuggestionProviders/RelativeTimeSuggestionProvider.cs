using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PeteEvans.AutoCompleteComboBox
{
    /// <summary>
    ///     Provide Time Suggestions from a relative time
    /// </summary>
    public class RelativeTimeSuggestionProvider : ISuggestionProvider
    {
        private readonly Regex dmyRegex;
        private readonly Regex simpleRegex;


        public RelativeTimeSuggestionProvider()
        {
            dmyRegex = new Regex(@"(\d+)([hm])");
            simpleRegex = new Regex(@"(\d+)");
        }

        public IEnumerable<Suggestion> GetFirstSuggestion(string searchText, object currentValue)
        {
            var currentDate = currentValue is DateTime ? (DateTime) currentValue : DateTime.Now;
            var newSuggestions = new List<Suggestion>();

            // Only attempt to work out a relative time if the search string start with + or -
            if (searchText.Length < 2) return newSuggestions;
            var firstCharacter = searchText[0];
            if (firstCharacter != '-' && firstCharacter != '+') return newSuggestions;

            // Find the parts in the time.
            var hours = 0;
            var minutes = 0;
            var matchFound = false;
            int val;

            var relativeTimeText = searchText.ToLower().Trim(firstCharacter);
            // Recognise parts with format like nnh nnm  where nn is a number
            var matches = dmyRegex.Matches(relativeTimeText);

            foreach (Match m in matches)
            {
                if (int.TryParse(m.Groups[1].Value, out val)
                    && val != 0)
                {
                    switch (m.Groups[2].Value)
                    {
                        case "h":
                            hours = val;
                            break;
                        case "m":
                            minutes = val;
                            break;
                    }
                    matchFound = true;
                }
            }

            if (!matchFound)
            {
                // Try a non specific match (assume minutes)
                var m = simpleRegex.Match(relativeTimeText);
                if (int.TryParse(m.Groups[0].Value, out val)
                    && val != 0)
                {
                    minutes = val;
                    matchFound = true;
                }
            }

            if (matchFound)
            {
                if (firstCharacter == '-')
                {
                    hours = -hours;
                    minutes = -minutes;
                }
                var suggestedTime
                    = ((DateTime) currentValue)
                        .AddHours(hours)
                        .AddMinutes(minutes);
                var s
                    = new Suggestion
                        (
                        "",
                        suggestedTime.ToShortTimeString(),
                        "",
                        100,
                        suggestedTime
                        );
                newSuggestions.Add(s);
            }


            return newSuggestions;
        }

        public IEnumerable<Suggestion> GetNextSuggestion()
        {
            return null;
        }
    }
}