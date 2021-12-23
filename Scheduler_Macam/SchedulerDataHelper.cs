using System;
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
            Las = 05
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
}
