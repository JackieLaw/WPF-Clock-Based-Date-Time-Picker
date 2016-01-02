using System.Collections.Generic;
using PeteEvans.AutoCompleteComboBox;

namespace PeteEvans.PickerComponents
{
    public class AutoCompleteTimePicker : AutoCompletionComboBox
    {
        public AutoCompleteTimePicker()
        {
            SuggestionProvidersCollection
                = new List<ISuggestionProvider>
                {
                    new TimeSuggestionProvider(),
                    new RelativeTimeSuggestionProvider()
                };
        }

        protected override object ComposeSelectedValue(object newValue)
        {
            return newValue;
        }
    }
}