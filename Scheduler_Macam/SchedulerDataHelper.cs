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
        #endregion
    }

    public class SchedulerException : ApplicationException
    {
        public SchedulerException(string message)
            : base(message)
        { }

        public SchedulerException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
