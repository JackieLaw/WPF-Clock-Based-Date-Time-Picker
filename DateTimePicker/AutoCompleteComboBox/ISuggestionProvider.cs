using System.Collections.Generic;

namespace PeteEvans.AutoCompleteComboBox
{
    /// <summary>
    ///     Interface for providers of suggestions for an auto competion combo box.
    /// </summary>
    public interface ISuggestionProvider
    {
        IEnumerable<Suggestion> GetFirstSuggestion(string searchText, object currentValue);
        IEnumerable<Suggestion> GetNextSuggestion();
    }
}