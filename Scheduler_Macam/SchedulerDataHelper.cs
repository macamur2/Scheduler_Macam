using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Scheduler.Domain.Resources;

namespace Scheduler.Domain
{
    public class SchedulerDataHelper
    {
        #region Enumerations
        public enum TypeConfiguration
        {
            Once = 01,
            Recurring = 02
        }

        public enum OccursConfiguration
        {
            Daily = 01,
            Weekly = 02,
            Monthly = 03
        }

        public enum DailyFreqTime
        {
            Second = 01,
            Minutes = 02,
            Hours = 03
        }

        public enum MonthlyFrequency
        {
            First = 01,
            Second = 02,
            Third = 03,
            Fourth = 04,
            Last = 05
        }

        public enum MonthlyDay
        {
            Monday = 01,
            Tuesday = 02,
            Wednesday = 03,
            Thursday = 04,
            Friday = 05,
            Saturday = 06,
            Sunday = 07,
            Day = 08, //Any Week Day
            Weekday = 09, //Any Working Day
            WeekendDay = 10 //Saturday and Sunday
        }
        #endregion
    }

    public class SchedulerException : ApplicationException
    {
        public SchedulerException(string message)
            : base(message)
        { }
    }

    public static class DateTimeExtensions
    {
        public static DateTime[] GetDaysOfWeek(DateTime curDate, SchedulerDataHelper.MonthlyFrequency nthWeek)
        {
            List<DateTime> outputDayList = new();
            int startDaysToTake = 0;
            int lastDayToCheck = 0;
            int daysInMonth = DateTime.DaysInMonth(curDate.Year, curDate.Month);
            switch (nthWeek)
            {
                case SchedulerDataHelper.MonthlyFrequency.First:
                    startDaysToTake = 1;
                    lastDayToCheck = 7;
                    break;
                case SchedulerDataHelper.MonthlyFrequency.Second:
                    startDaysToTake = 8;
                    lastDayToCheck = 14;
                    break;
                case SchedulerDataHelper.MonthlyFrequency.Third:
                    startDaysToTake = 15;
                    lastDayToCheck = 21;
                    break;
                case SchedulerDataHelper.MonthlyFrequency.Fourth:
                    startDaysToTake = 22;
                    lastDayToCheck = 28;
                    break;
                case SchedulerDataHelper.MonthlyFrequency.Last:
                    startDaysToTake = 29;
                    lastDayToCheck = daysInMonth;
                    break;
            }

            while(startDaysToTake <= lastDayToCheck)
            {
                outputDayList.Add(new DateTime(curDate.Year, curDate.Month, startDaysToTake, curDate.Hour, curDate.Minute, curDate.Second));
                startDaysToTake++;
            }
            return outputDayList.ToArray();
        }
    }
}
