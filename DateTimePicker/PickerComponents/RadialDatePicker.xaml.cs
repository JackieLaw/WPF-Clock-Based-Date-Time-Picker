using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PeteEvans.PickerComponents
{
    /// <summary>
    ///     Interaction logic for RadialDatePicker.xaml
    /// </summary>
    public partial class RadialDatePicker : UserControl
    {
        #region Constants

        // Mouse
        private const int MouseMovementLimit = 3;

        #endregion Constants

        #region Constructor

        public RadialDatePicker()
        {
            InitializeComponent();

            // Create the array of dates for the month
            CreateDates();
        }

        #endregion Constructor

        #region Dependency Properties

        #region DisplayTime

        public static readonly DependencyProperty DisplayTimeProperty
            = DependencyProperty.Register("DisplayTime",
                typeof (DateTime),
                typeof (RadialDatePicker),
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
            var control = source as RadialDatePicker;
            var time = (DateTime) e.NewValue;

            // Set up the picker
            control.SetupDatesFor(time.Date);
        }

        #endregion DisplayTime

        #endregion Dependency Properties

        #region Private Mehtods

        private List<object> DaysThisMonth;
        private List<object> DaysNextMonth;

        private List<object> MonthsThisYear;
        private List<object> MonthsNextYear;

        private List<object> Year;

        private void CreateDates()
        {
            // Days This Month
            DaysThisMonth = new List<object>();
            DaysThisMonth.Add(new StringObject {Content = "Jan"});
            var d = Enumerable.Range(1, 31).ToArray();
            var dateInMonth = 1;
            foreach (var day in d)
            {
                DaysThisMonth
                    .Add(
                        new DatePickerControlSettings
                        {
                            Text = dateInMonth.ToString(),
                            Date = new DateTime(2015, 12, dateInMonth++)
                        });
            }
            DaysThisMonthItemsControl.ItemsSource = DaysThisMonth;

            // Day Next Month
            DaysNextMonth = new List<object>();
            DaysNextMonth.Add(new StringObject {Content = "Feb"});
            dateInMonth = 1;
            foreach (var day in d)
            {
                DaysNextMonth
                    .Add(
                        new DatePickerControlSettings
                        {
                            Text = dateInMonth.ToString(),
                            Date = new DateTime(2015, 12, dateInMonth++)
                        });
            }
            DaysNextMonthItemsControl.ItemsSource = DaysNextMonth;

            // Months This Year
            MonthsThisYear = new List<object>();
            MonthsThisYear.Add(new StringObject {Content = "15"});
            var m = Enumerable.Range(1, 12).ToArray();
            var monthInYear = 1;
            foreach (var month in m)
            {
                var initialDate = new DateTime(2000, monthInYear++, 1);
                MonthsThisYear
                    .Add(
                        new DatePickerControlSettings
                        {
                            Text = initialDate.ToString("MMM"),
                            Date = initialDate
                        });
            }
            MonthsThisYearItemsControl.ItemsSource = MonthsThisYear;

            // Months Next Year
            MonthsNextYear = new List<object>();
            MonthsNextYear.Add(new StringObject {Content = "16"});
            monthInYear = 1;
            foreach (var month in m)
            {
                var initialDate = new DateTime(2000, monthInYear++, 1);
                MonthsNextYear
                    .Add(
                        new DatePickerControlSettings
                        {
                            Text = initialDate.ToString("MMM"),
                            Date = initialDate
                        });
            }
            MonthsNextYearItemsControl.ItemsSource = MonthsNextYear;

            // Years
            Year = new List<object>();
            var y = Enumerable.Range(1, 3).ToArray();
            foreach (var year in y)
            {
                var initialDate = new DateTime(year, 1, 1);
                Year.Add(
                    new DatePickerControlSettings
                    {
                        Text = initialDate.ToString("yy"),
                        Date = initialDate
                    });
            }

            // Bind the named year controls to the year list
            ThisYearPicker.DataContext = Year[0];
            NextYearPicker.DataContext = Year[1];
            LastYearPicker.DataContext = Year[2];

            SetupDatesFor(DisplayTime.Date);
        }

        private void SetupDatesFor(DateTime date)
        {
            var daysInCurrentMonth = DateTime.DaysInMonth(date.Year, date.Month);

            // Days This Month
            DaysThisMonth.OfType<StringObject>().First().Content = date.ToString("MMM");
            foreach (var entry in DaysThisMonth.OfType<DatePickerControlSettings>())
            {
                if (entry.Date.Day <= daysInCurrentMonth)
                {
                    entry.Date = new DateTime(date.Year, date.Month, entry.Date.Day);
                    entry.IsValid = true;
                    entry.IsSelected = entry.Date == date.Date;
                    entry.IsHighlighted = entry.Date == DateTime.Now.Date;
                }
                else
                {
                    entry.IsValid = false;
                }
            }

            // Days Next Month
            var dateNextMonth = date.AddMonths(1);
            DaysNextMonth.OfType<StringObject>().First().Content = dateNextMonth.ToString("MMM");
            var daysInNextMonth = DateTime.DaysInMonth(dateNextMonth.Year, dateNextMonth.Month);
            foreach (var entry in DaysNextMonth.OfType<DatePickerControlSettings>())
            {
                if (entry.Date.Day <= daysInNextMonth)
                {
                    entry.Date = new DateTime(dateNextMonth.Year,
                        dateNextMonth.Month,
                        entry.Date.Day);
                    entry.IsValid = true;
                    entry.IsSelected = false;
                    entry.IsHighlighted = entry.Date == DateTime.Now.Date;
                }
                else
                {
                    entry.IsValid = false;
                }
            }

            // Months This Year
            MonthsThisYear.OfType<StringObject>().First().Content = date.ToString("yy");
            foreach (var entry in MonthsThisYear.OfType<DatePickerControlSettings>())
            {
                entry.Date = new DateTime(date.Year, entry.Date.Month, 1);
                entry.IsValid = true;
                entry.IsSelected = entry.Date.Month == date.Date.Month
                                   && entry.Date.Year == date.Date.Year;
                entry.IsHighlighted = entry.Date.Month == DateTime.Now.Date.Month
                                      && entry.Date.Year == DateTime.Now.Date.Year;
            }

            // Months Next Year
            var dateNextYear = date.AddYears(1);
            MonthsNextYear.OfType<StringObject>().First().Content = dateNextYear.ToString("yy");
            foreach (var entry in MonthsNextYear.OfType<DatePickerControlSettings>())
            {
                entry.Date = new DateTime(dateNextYear.Year, entry.Date.Month, 1);
                entry.IsValid = true;
                entry.IsSelected = false;
                entry.IsHighlighted = entry.Date.Month == DateTime.Now.Date.Month
                                      && entry.Date.Year == DateTime.Now.Date.Year;
            }

            // Years
            var requiredYears = Enumerable.Range(date.Date.Year - 1, 3).ToArray();
            var yearIndex = 1;
            foreach (var entry in Year.OfType<DatePickerControlSettings>())
            {
                entry.Date = new DateTime(requiredYears[yearIndex], date.Month, 1);
                entry.Text = entry.Date.ToString("yy");
                yearIndex = ++yearIndex%3;
                entry.IsValid = true;
                entry.IsSelected = entry.Date.Year == date.Date.Year;
                entry.IsHighlighted = entry.Date.Year == DateTime.Now.Date.Year;
            }
        }

        #endregion Private Mehtods
    }
}