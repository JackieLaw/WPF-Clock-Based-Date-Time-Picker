using System;
using System.Windows;
using System.Windows.Controls;

namespace PeteEvans.Pickers
{
    /// <summary>
    ///     Interaction logic for NullableDateTimePicker.xaml
    /// </summary>
    public partial class NullableDateTimePicker : UserControl
    {
        #region Constructor

        public NullableDateTimePicker()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Dependency Properties

        #region SelectedValue

        public static readonly DependencyProperty SelectedValueProperty
            = DependencyProperty.Register("SelectedValue",
                typeof (DateTime?),
                typeof (NullableDateTimePicker),
                new FrameworkPropertyMetadata(null,
                    OnSelectedValuePropertyChanged));

        public DateTime? SelectedValue
        {
            get { return (DateTime?) GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        private static void OnSelectedValuePropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as NullableDateTimePicker;

            // Ensure the is valid and interim value matches.
            if (control.SelectedValue.HasValue)
            {
                control.IsValid = true;
                if (control.InterimValue != control.SelectedValue.Value)
                {
                    control.InterimValue = control.SelectedValue.Value;
                }
            }
            else
            {
                control.IsValid = false;
            }

            // Display the formatted string
            control.SetDateTimeText();
        }

        #endregion SelectedValue

        #region IsValid

        public static readonly DependencyProperty IsValidProperty
            = DependencyProperty.Register("IsValid",
                typeof (bool),
                typeof (NullableDateTimePicker),
                new FrameworkPropertyMetadata(false,
                    OnIsValidPropertyChanged));

        public bool IsValid
        {
            get { return (bool) GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        private static void OnIsValidPropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as NullableDateTimePicker;
            // Display the formatted string
            control.SetDateTimeText();
        }

        #endregion IsValid

        #region InterimValue

        public static readonly DependencyProperty InterimValueProperty
            = DependencyProperty.Register("InterimValue",
                typeof (DateTime),
                typeof (NullableDateTimePicker),
                new FrameworkPropertyMetadata(DateTime.Now.Date,
                    OnInterimValuePropertyChanged));

        public DateTime InterimValue
        {
            get { return (DateTime) GetValue(InterimValueProperty); }
            set { SetValue(InterimValueProperty, value); }
        }

        private static void OnInterimValuePropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as NullableDateTimePicker;
            // Set the new date time as valid on change
            control.IsValid = true;
        }

        #endregion InterimValue

        #region Format

        public static readonly DependencyProperty FormatProperty
            = DependencyProperty.Register("Format",
                typeof (string),
                typeof (NullableDateTimePicker),
                new FrameworkPropertyMetadata(string.Empty,
                    OnFormatPropertyChanged));

        public string Format
        {
            get { return (string) GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        private static void OnFormatPropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as NullableDateTimePicker;
            // Redisplay the formatted string
            if (control.IsValid)
            {
                control.TextBlock.Text = string.Format("{0" + (string) e.NewValue + "}", control.SelectedValue);
            }
        }

        #endregion Format

        #endregion Dependency Properties

        #region Private Methods

        private void SetDateTimeText()
        {
            if (IsValid)
            {
                TextBlock.Text = string.Format("{0" + Format + "}", SelectedValue);
            }
            else
            {
                TextBlock.Text = string.Empty;
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            PickerPopup.IsOpen = true;
            TextualTimePicker.Focus();
        }

        private void PickerPopup_Closed(object sender, EventArgs e)
        {
            // When closing default is to use the interim value as the new selected value.
            if (IsValid)
            {
                if (SelectedValue != InterimValue)
                {
                    SelectedValue = InterimValue;
                }
            }
            else
            {
                SelectedValue = null;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            PickerPopup.IsOpen = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Undo changes then close the popup
            if (SelectedValue.HasValue)
            {
                // There was a previously selected value so fix the interim values before close
                if (InterimValue != SelectedValue.Value)
                {
                    InterimValue = SelectedValue.Value;
                }
            }
            else
            {
                IsValid = false;
            }

            PickerPopup.IsOpen = false;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            IsValid = false;
        }

        #endregion Private Methods
    }
}