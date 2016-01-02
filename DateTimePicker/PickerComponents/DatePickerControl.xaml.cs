using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PeteEvans.PickerComponents
{
    /// <summary>
    ///     Interaction logic for BaseDatePickerControl.xaml
    /// </summary>
    public partial class DatePickerControl : UserControl
    {
        #region Constants

        private const int MouseMovementLimit = 3;

        #endregion Constants

        #region Constructor

        public DatePickerControl()
        {
            InitializeComponent();
        }

        #endregion Constructor

        private void Control_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        #region Dependency Properties

        #region ControlSize

        public static readonly DependencyProperty ControlSizeProperty
            = DependencyProperty.Register("ControlSize",
                typeof (double),
                typeof (DatePickerControl),
                new FrameworkPropertyMetadata(default(double),
                    OnControlSizePropertyChanged));

        public double ControlSize
        {
            get { return (double) GetValue(ControlSizeProperty); }
            set { SetValue(ControlSizeProperty, value); }
        }

        private static void OnControlSizePropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as DatePickerControl;
            var size = (double) e.NewValue;

            // Set the size
            control.OuterCircle.Height = size;
            control.OuterCircle.Width = size;
        }

        #endregion ControlSize

        #region HighlightSize

        public static readonly DependencyProperty HighlightSizeProperty
            = DependencyProperty.Register("HighlightSize",
                typeof (double),
                typeof (DatePickerControl),
                new FrameworkPropertyMetadata(default(double),
                    OnHighlightSizePropertyChanged));

        public double HighlightSize
        {
            get { return (double) GetValue(HighlightSizeProperty); }
            set { SetValue(HighlightSizeProperty, value); }
        }

        private static void OnHighlightSizePropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as DatePickerControl;
            var size = (double) e.NewValue;

            // Set the size
            control.HighlightCircle.Height = size;
            control.HighlightCircle.Width = size;
        }

        #endregion HighlightSize

        #region SelectedSize

        public static readonly DependencyProperty SelectedSizeProperty
            = DependencyProperty.Register("SelectedSize",
                typeof (double),
                typeof (DatePickerControl),
                new FrameworkPropertyMetadata(default(double),
                    OnSelectedSizePropertyChanged));

        public double SelectedSize
        {
            get { return (double) GetValue(SelectedSizeProperty); }
            set { SetValue(SelectedSizeProperty, value); }
        }

        private static void OnSelectedSizePropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as DatePickerControl;
            var size = (double) e.NewValue;

            // Set the size
            control.SelectedCircle.Height = size;
            control.SelectedCircle.Width = size;
        }

        #endregion SelectedSize

        #region DisplayTime

        public static readonly DependencyProperty DisplayTimeProperty
            = DependencyProperty.Register("DisplayTime",
                typeof (DateTime),
                typeof (DatePickerControl),
                new FrameworkPropertyMetadata(new DateTime(2015, 12, 25, 15, 31, 0)));

        public DateTime DisplayTime
        {
            get { return (DateTime) GetValue(DisplayTimeProperty); }
            set { SetValue(DisplayTimeProperty, value); }
        }

        #endregion DisplayTime

        #region RequiredDate

        public static readonly DependencyProperty RequiredDateProperty
            = DependencyProperty.Register("RequiredDate",
                typeof (DateTime),
                typeof (DatePickerControl),
                new FrameworkPropertyMetadata(new DateTime(2015, 12, 25, 15, 31, 0)));

        public DateTime RequiredDate
        {
            get { return (DateTime) GetValue(RequiredDateProperty); }
            set { SetValue(RequiredDateProperty, value); }
        }

        #endregion RequiredDate

        #region SetDay

        public static readonly DependencyProperty SetDayProperty
            = DependencyProperty.Register("SetDay",
                typeof (bool),
                typeof (DatePickerControl),
                new FrameworkPropertyMetadata(true));

        public bool SetDay
        {
            get { return (bool) GetValue(SetDayProperty); }
            set { SetValue(SetDayProperty, value); }
        }

        #endregion SetDay

        #region Text

        public static readonly DependencyProperty TextProperty
            = DependencyProperty.Register("Text",
                typeof (string),
                typeof (DatePickerControl),
                new FrameworkPropertyMetadata(default(string),
                    OnTextPropertyChanged));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextPropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as DatePickerControl;
            var text = (string) e.NewValue;

            // Set the text
            control.TextValue.Text = text;
        }

        #endregion Text

        #region IsSelected

        public static readonly DependencyProperty IsSelectedProperty
            = DependencyProperty.Register("IsSelected",
                typeof (bool),
                typeof (DatePickerControl),
                new FrameworkPropertyMetadata(default(bool),
                    OnIsSelectedPropertyChanged));

        public bool IsSelected
        {
            get { return (bool) GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        private static void OnIsSelectedPropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as DatePickerControl;
            var isSelected = (bool) e.NewValue;

            // Set the visibility
            control.SelectedCircle.Visibility = isSelected ? Visibility.Visible : Visibility.Hidden;
        }

        #endregion IsSelected

        #region IsHighlighted

        public static readonly DependencyProperty IsHighlightedProperty
            = DependencyProperty.Register("IsHighlighted",
                typeof (bool),
                typeof (DatePickerControl),
                new FrameworkPropertyMetadata(default(bool),
                    OnIsHighlightedPropertyChanged));

        public bool IsHighlighted
        {
            get { return (bool) GetValue(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        private static void OnIsHighlightedPropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as DatePickerControl;
            var isHighlighted = (bool) e.NewValue;

            // Set the visibility
            control.HighlightCircle.Visibility = isHighlighted ? Visibility.Visible : Visibility.Hidden;
        }

        #endregion IsHighlighted

        #region IsValid

        public static readonly DependencyProperty IsValidProperty
            = DependencyProperty.Register("IsValid",
                typeof (bool),
                typeof (DatePickerControl),
                new FrameworkPropertyMetadata(default(bool),
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
            var control = source as DatePickerControl;
            var isValid = (bool) e.NewValue;

            // Set the visibility
            control.Visibility = isValid ? Visibility.Visible : Visibility.Hidden;
        }

        #endregion IsValid

        #endregion Dependency Properties

        #region Event Handling

        private bool mouseDown;

        private void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsValid) return;
            mouseDown = true;
        }

        private void Control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsValid || !mouseDown) return;

            var tempTime = DisplayTime;
            tempTime = tempTime.AddYears(RequiredDate.Year - tempTime.Year);
            tempTime = tempTime.AddMonths(RequiredDate.Month - tempTime.Month);
            if (SetDay)
            {
                tempTime = tempTime.AddDays(RequiredDate.Day - tempTime.Day);
            }
            DisplayTime = tempTime;
            mouseDown = false;
        }

        #endregion Event Handling
    }
}