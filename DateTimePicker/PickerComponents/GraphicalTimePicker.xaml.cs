using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PeteEvans.PickerComponents
{
    /// <summary>
    ///     Interaction logic for Grapical Time Picker
    /// </summary>
    public partial class GraphicalTimePicker : UserControl
    {
        #region Constructor

        public GraphicalTimePicker()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Constants

        // Geometry etc
        private const int MinutesInHour = 60;
        private const double HourHandDegreesPerMinute = 0.5; // 360 / (12 * 60)
        private const double HourHandDegreesPerHour = 30; // 360 / 12
        private const double MinuteHandDegreesPerMinute = 6; // 360 / 60
        private const int DegreesInCircle = 360;
        private const double DegreesPerRadian = 180/Math.PI;
        private const int HoursPerRevolution = 12;

        // Mouse
        private const int MouseMovementLimit = 3;

        #endregion Constants

        #region Dependency Properties

        #region DisplayTime

        public static readonly DependencyProperty DisplayTimeProperty
            = DependencyProperty.Register("DisplayTime",
                typeof (DateTime),
                typeof (GraphicalTimePicker),
                new FrameworkPropertyMetadata(new DateTime(2015, 12, 25, 15, 31, 0),
                    OnDisplayTimePropertyChanged));

        public DateTime DisplayTime
        {
            get { return (DateTime) GetValue(DisplayTimeProperty); }
            set { SetValue(DisplayTimeProperty, value); }
        }

        private static void OnDisplayTimePropertyChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            // Get the new value
            var control = source as GraphicalTimePicker;
            var time = (DateTime) e.NewValue;

            // Set up the hands
            control.MoveHands(time);
        }

        #endregion DisplayTime

        #region AngleForHour

        public static readonly DependencyProperty AngleForHourProperty
            = DependencyProperty.Register("AngleForHour",
                typeof (double),
                typeof (GraphicalTimePicker),
                new FrameworkPropertyMetadata(0d));

        public double AngleForHour
        {
            get { return (double) GetValue(AngleForHourProperty); }
            set { SetValue(AngleForHourProperty, value); }
        }

        #endregion AngleForHour

        #region AngleForMinute

        public static readonly DependencyProperty AngleForMinuteProperty
            = DependencyProperty.Register("AngleForMinute",
                typeof (double),
                typeof (GraphicalTimePicker),
                new FrameworkPropertyMetadata(0d));

        public double AngleForMinute
        {
            get { return (double) GetValue(AngleForMinuteProperty); }
            set { SetValue(AngleForMinuteProperty, value); }
        }

        #endregion AngleForMinute

        public double hourAngle { get; private set; }

        #endregion Dependency Properties

        #region Private Methods

        #region Display Time Related Methods

        private void MoveHands(DateTime time)
        {
            // Set up the rotations for the minute and hour hand based on the TimeOfDay
            double totalMinutes = time.Minute + time.Hour*MinutesInHour;

            AngleForHour = totalMinutes*HourHandDegreesPerMinute;
            if (AngleForHour >= DegreesInCircle) AngleForHour -= DegreesInCircle;
            AngleForMinute = time.Minute*MinuteHandDegreesPerMinute;
        }

        #endregion Display Time Related Methods

        private void HourButtonClick(object sender, RoutedEventArgs e)
        {
            // Set the Display Time
            var sourceButton = sender as Button;
            var newHour = Convert.ToInt32(sourceButton.Content);

            DisplayTime = DisplayTime.AddHours(newHour - DisplayTime.Hour);
        }

        #region Mouse Click Handling

        private Point mouseDownPoint;

        #region Hours AM

        private void EllipseHoursAm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDownPoint = e.GetPosition(sender as IInputElement);

            // Use the down position to calculate the angle and hence required time
            var a = GetAngle(mouseDownPoint, EllipseHoursAm.Width/2, EllipseHoursAm.Height/2);

            var hour = (int) Math.Round(a/HourHandDegreesPerHour);
            if (hour < 1) hour += HoursPerRevolution;

            // Use right click to set afternoon
            if (e.ChangedButton == MouseButton.Right)
            {
                hour += HoursPerRevolution;
            }

            DisplayTime = DisplayTime.AddHours(hour - DisplayTime.Hour);
        }

        #endregion Hours AM

        #region Hours PM

        private void EllipseHoursPm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDownPoint = e.GetPosition(sender as IInputElement);

            // Use the down position to calculate the angle and hence required time
            var a = GetAngle(mouseDownPoint, EllipseHoursPm.Width/2, EllipseHoursPm.Height/2);

            var hour = (int) Math.Round(a/HourHandDegreesPerHour) + HoursPerRevolution;
            if (hour == 24) hour = 0;

            DisplayTime = DisplayTime.AddHours(hour - DisplayTime.Hour);
        }

        #endregion Hours PM

        #region Minutes

        private void EllipseMinutes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDownPoint = e.GetPosition(sender as IInputElement);

            // Use the down position to calculate the angle and hence required time
            var a = GetAngle(mouseDownPoint, EllipseMinutes.Width/2, EllipseMinutes.Height/2);

            var minutes = (int) Math.Round(a/MinuteHandDegreesPerMinute);

            DisplayTime = DisplayTime.AddMinutes(minutes - DisplayTime.Minute);

            // Now treat as a drag
            MinuteGrabMouseIsDown = true;
            Mouse.Capture(MinuteHandle);
        }

        #endregion Minutes

        #endregion Mouse Click Handling

        private double GetAngle(Point mouse, double centreX, double centreY)
        {
            var angle = Math.Atan2(mouse.X - centreX,
                centreY - mouse.Y)*DegreesPerRadian;

            if (angle < 0) angle += DegreesInCircle;

            return angle;
        }

        #endregion Private Methods

        #region Hour Hand Drag Handling

        private bool hourGrabMouseIsDown;

        private void HourGrab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            hourGrabMouseIsDown = true;
            Mouse.Capture(sender as UIElement);
        }

        private void HourGrab_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            hourGrabMouseIsDown = false;
        }

        private void HourGrab_MouseMove(object sender, MouseEventArgs e)
        {
            if (hourGrabMouseIsDown)
            {
                // Use positions relative to the hours display (centre is fixed with clock centre).
                var mousePosition = e.GetPosition(EllipseHoursAm);

                // Use the position to calculate the angle and hence required time
                var a = GetAngle(mousePosition, EllipseHoursAm.Width/2, EllipseHoursAm.Height/2);

                var hour = a/HourHandDegreesPerHour;

                var minutes = Math.Round(hour*60%60);
                hour = Math.Floor(hour);

                // The current hour will be in the range 0 to 11 (i.e. morning).
                // The current display time could be morning or afternoon and if the
                // display time is near midnight this could be a transition to another day.

                var dayOffset = 0;
                var displayHour = DisplayTime.Hour;

                if (displayHour < 3 && hour > 9)
                {
                    // Drag transition back a day
                    dayOffset = -1;
                    hour += HoursPerRevolution; // Adjust time to afternoon
                }
                else if (displayHour > 21 && hour < 3)
                {
                    // Drag transition forward a day
                    dayOffset = 1;
                }
                else if (displayHour > 9 && displayHour < 12
                         && hour < 3)
                {
                    // Drag transition between morning and afternoon
                    hour += HoursPerRevolution;
                }
                else if (displayHour >= 12)
                {
                    // Dragging in the afternoon

                    // Add 12 hours if not transition to am!
                    if (!(displayHour < 15 && hour > 9))
                    {
                        hour += HoursPerRevolution;
                    }
                }

                DisplayTime = DisplayTime.AddHours(hour - DisplayTime.Hour);
                DisplayTime = DisplayTime.AddMinutes(minutes - DisplayTime.Minute);
                if (dayOffset != 0)
                {
                    DisplayTime = DisplayTime.AddDays(dayOffset);
                }
            }
        }

        #endregion Hour Hand Drag Handling

        #region Minute Hand Drag Handling

        private bool MinuteGrabMouseIsDown;

        private void MinuteGrab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MinuteGrabMouseIsDown = true;
            Mouse.Capture(sender as UIElement);
        }

        private void MinuteGrab_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            MinuteGrabMouseIsDown = false;
        }

        private void MinuteGrab_MouseMove(object sender, MouseEventArgs e)
        {
            if (MinuteGrabMouseIsDown)
            {
                // Use positions relative to the hours display (centre is fixed with clock centre).
                var mousePosition = e.GetPosition(EllipseHoursAm);

                // Use the position to calculate the angle and hence required time
                var a = GetAngle(mousePosition, EllipseHoursAm.Width/2, EllipseHoursAm.Height/2);

                var minute = a/MinuteHandDegreesPerMinute;


                minute = Math.Round(minute);

                // The current Minute will be in the range 0 to 59
                // Work out whether to transition up or down an hour

                var displayMinute = DisplayTime.Minute;

                if (displayMinute < 15 && minute > 45)
                {
                    // Drag transition back an hour
                    minute = minute - MinutesInHour;
                }
                else if (displayMinute > 45 && minute < 15)
                {
                    // Drag transition forward an hour
                    minute = minute + MinutesInHour;
                }

                DisplayTime = DisplayTime.AddMinutes(minute - DisplayTime.Minute);
            }
        }

        #endregion Minute Hand Drag Handling
    }
}