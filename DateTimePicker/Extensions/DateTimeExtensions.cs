using System;

namespace PeteEvans.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime SetDayOfMonth(this DateTime date, int dayOfMonth)
        {
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            if (dayOfMonth > 0 && dayOfMonth <= daysInMonth)
            {
                return date.AddDays(dayOfMonth - date.Day);
            }
            return date;
        }

        public static DateTime SetMonth(this DateTime date, int month)
        {
            if (month > 0 && month <= 12)
            {
                return date.AddMonths(month - date.Month);
            }
            return date;
        }

        public static DateTime SetYear(this DateTime date, int year)
        {
            if (year > 0 && year <= 9999)
            {
                return date.AddYears(year - date.Year);
            }
            return date;
        }

        public static DateTime SetHour(this DateTime time, int hour)
        {
            if (hour >= 0 && hour <= 23)
            {
                return time.AddHours(hour - time.Hour);
            }
            return time;
        }

        public static DateTime SetMinute(this DateTime time, int minute)
        {
            if (minute >= 0 && minute <= 59)
            {
                return time.AddMinutes(minute - time.Minute);
            }
            return time;
        }

        public static DateTime NextDayOfWeek(this DateTime date, DayOfWeek day)
        {
            var dayThatWeek = (int) day - (int) date.DayOfWeek;
            var nextDay = dayThatWeek > 0 ? dayThatWeek : dayThatWeek + 7;
            return date.AddDays(nextDay);
        }

        public static DateTime PreviousDayOfWeek(this DateTime date, DayOfWeek day)
        {
            var dayThatWeek = (int) day - (int) date.DayOfWeek;
            var previousDay = dayThatWeek < 0 ? dayThatWeek : dayThatWeek - 7;
            return date.AddDays(previousDay);
        }

        public static DateTime NextDate(this DateTime date, DayOfWeek dayOfWeek, int dayOfMonth)
        {
            if (dayOfMonth < 1 || dayOfMonth > 31
                || dayOfWeek < DayOfWeek.Sunday || dayOfWeek > DayOfWeek.Saturday)
            {
                return date;
            }
            var candidate = date;
            do
            {
                candidate = candidate.AddDays(1);
            } while (candidate.DayOfWeek != dayOfWeek || candidate.Day != dayOfMonth);

            return candidate;
        }

        public static DateTime PreviousDate(this DateTime date, DayOfWeek dayOfWeek, int dayOfMonth)
        {
            if (dayOfMonth < 1 || dayOfMonth > 31
                || dayOfWeek < DayOfWeek.Sunday || dayOfWeek > DayOfWeek.Saturday)
            {
                return date;
            }
            var candidate = date;
            do
            {
                candidate = candidate.AddDays(-1);
            } while (candidate.DayOfWeek != dayOfWeek || candidate.Day != dayOfMonth);

            return candidate;
        }
    }
}