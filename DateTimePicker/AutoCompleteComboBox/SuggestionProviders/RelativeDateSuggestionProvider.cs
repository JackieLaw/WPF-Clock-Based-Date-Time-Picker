using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PeteEvans.AutoCompleteComboBox
{
    /// <summary>
    ///     Provide Data Suggestions from a relative date
    /// </summary>
    public class RelativeDateSuggestionProvider : ISuggestionProvider
    {
        private readonly Regex dmyRegex;
        private readonly Regex simpleRegex;


        public RelativeDateSuggestionProvider()
        {
            dmyRegex = new Regex(@"(\d+)([dmy])");
            simpleRegex = new Regex(@"(\d+)");
        }

        public IEnumerable<Suggestion> GetFirstSuggestion(string searchText, object currentValue)
        {
            var currentDate = currentValue is DateTime ? (DateTime) currentValue : DateTime.Now;
            var newSuggestions = new List<Suggestion>();

            // Only attempt to work out a relative date if the search string start with + or -
            if (searchText.Length < 2) return newSuggestions;
            var firstCharacter = searchText[0];
            if (firstCharacter != '-' && firstCharacter != '+') return newSuggestions;

            // Find the parts in the date.
            var days = 0;
            var months = 0;
            var years = 0;
            var matchFound = false;
            int val;

            var relativeDateText = searchText.ToLower().Trim(firstCharacter);
            // Recognise parts with format like nnd nnm nny where nn is a number
            var matches = dmyRegex.Matches(relativeDateText);

            foreach (Match m in matches)
            {
                if (int.TryParse(m.Groups[1].Value, out val)
                    && val != 0)
                {
                    switch (m.Groups[2].Value)
                    {
                        case "d":
                            days = val;
                            break;
                        case "m":
                            months = val;
                            break;
                        case "y":
                            years = val;
                            break;
                    }
                    matchFound = true;
                }
            }

            if (!matchFound)
            {
                // Try a non specific match (assume days)
                var m = simpleRegex.Match(relativeDateText);
                if (int.TryParse(m.Groups[0].Value, out val)
                    && val != 0)
                {
                    days = val;
                    matchFound = true;
                }
            }

            if (matchFound)
            {
                if (firstCharacter == '-')
                {
                    days = -days;
                    months = -months;
                    years = -years;
                }
                var suggestedDate
                    = ((DateTime) currentValue)
                        .AddDays(days)
                        .AddMonths(months)
                        .AddYears(years);
                var s
                    = new Suggestion
                        (
                        "",
                        suggestedDate.ToShortDateString(),
                        "",
                        100,
                        suggestedDate
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