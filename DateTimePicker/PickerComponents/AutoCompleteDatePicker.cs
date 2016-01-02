using System;
using System.Collections.Generic;
using PeteEvans.AutoCompleteComboBox;

namespace PeteEvans.PickerComponents
{
    public class AutoCompleteDatePicker : AutoCompletionComboBox
    {
        public AutoCompleteDatePicker()
        {
            SuggestionProvidersCollection
                = new List<ISuggestionProvider>
                {
                    new DateSuggestionProvider(),
                    new RelativeDateSuggestionProvider()
                };
        }

        protected override object ComposeSelectedValue(object newValue)
        {
            var newDate = ((DateTime) newValue).Date;
            var current = (DateTime) SelectedValue;
            return newDate.AddHours(current.Hour).AddMinutes(current.Minute);
        }
    }
}