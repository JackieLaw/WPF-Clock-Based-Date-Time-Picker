using System;
using System.Windows;
using System.Windows.Controls;

namespace PeteEvans.Pickers
{
    /// <summary>
    ///     Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePicker : UserControl
    {
        #region Constructor

        public DateTimePicker()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Dependency Properties

        #region SelectedValue

        public static readonly DependencyProperty SelectedValueProperty
            = DependencyProperty.Register("SelectedValue",
                typeof (DateTime),
                typeof (DateTimePicker),
                new FrameworkPropertyMetadata(new DateTime(2000, 1, 1, 0, 0, 0),
                    OnSelectedValuePropertyChanged));

        public DateTime SelectedValue
        {
            get { return (DateTime) GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        private static void OnSelectedValuePropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as DateTimePicker;

            // Ensure the interim value matches.
            if (control.InterimValue != control.SelectedValue)
            {
                control.InterimValue = control.SelectedValue;
            }

            // Display the formatted string
            control.SetDateTimeText();
        }

        #endregion SelectedValue

        #region InterimValue

        public static readonly DependencyProperty InterimValueProperty
            = DependencyProperty.Register("InterimValue",
                typeof (DateTime),
                typeof (DateTimePicker),
                new FrameworkPropertyMetadata(new DateTime(2000, 1, 1, 0, 0, 0)));

        public DateTime InterimValue
        {
            get { return (DateTime) GetValue(InterimValueProperty); }
            set { SetValue(InterimValueProperty, value); }
        }

        #endregion InterimValue

        #region Format

        public static readonly DependencyProperty FormatProperty
            = DependencyProperty.Register("Format",
                typeof (string),
                typeof (DateTimePicker),
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
            var control = source as DateTimePicker;
            // Redisplay the formatted string
            control.TextBlock.Text = string.Format("{0" + (string) e.NewValue + "}", control.SelectedValue);
        }

        #endregion Format

        #endregion Dependency Properties

        #region Private Methods

        private void SetDateTimeText()
        {
            TextBlock.Text = string.Format("{0" + Format + "}", SelectedValue);
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            PickerPopup.IsOpen = true;
            TextualTimePicker.Focus();
        }

        private void PickerPopup_Closed(object sender, EventArgs e)
        {
            // When closing default is to use the interim value as selected.
            if (SelectedValue != InterimValue)
            {
                SelectedValue = InterimValue;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            PickerPopup.IsOpen = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Undo changes then close the popup
            if (InterimValue != SelectedValue)
            {
                InterimValue = SelectedValue;
            }
            PickerPopup.IsOpen = false;
        }

        #endregion Private Methods
    }
}