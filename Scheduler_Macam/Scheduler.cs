using Scheduler.Domain.Resources;
using System;

namespace Scheduler.Domain
{
    /// <summary>
    /// Scheduler Class used in task operations.
    /// </summary>
    public class Scheduler
    {
        #region Properties
        public DateTime CurrentDate { get { return new DateTime(2020, 01, 01); } }
        public SchedulerDataHelper.TypeConfiguration? ConfigType { get; set; }
        public bool ConfigEnabled { get; set; }
        public DateTime? ConfigOnceTimeAt { get; set; }
        public SchedulerDataHelper.OccursConfiguration? ConfigOccurs { get; set; }

        // Monthly Configuration
        public bool MonthlyDayEnabled { get; set; }
        public bool MonthlyTheEnabled { get; set; }
        public int? MonthlyDayEveryDay { get; set; }
        public int? MonthlyDayEveryMonth { get; set; }
        public SchedulerDataHelper.MonthlyFrequency MonthlyTheFreqency { get; set; }
        public SchedulerDataHelper.MonthlyDay MonthlyTheDay { get; set; }
        public int? MonthlyTheEveryMonths { get; set; }

        // Weekly Configuration
        public int? WeeklyEvery { get; set; }
        public bool WeeklyMonday { get; set; }
        public bool WeeklyTuesday { get; set; }
        public bool WeeklyWednesday { get; set; }
        public bool WeeklyThursday { get; set; }
        public bool WeeklyFriday { get; set; }
        public bool WeeklySaturday { get; set; }
        public bool WeeklySunday { get; set; }

        // Daily Frequency
        public bool DailyFrequencyOnceAtEnabled { get; set; }
        public TimeSpan? DailyFrequencyOnceAtTime { get; set; }
        public bool DailyFrequencyEveryEnabled { get; set; }
        public double? DailyFrequencyEveryNumber { get; set; }
        public SchedulerDataHelper.DailyFreqTime? DailyFrequencyEveryTime { get; set; }
        public TimeSpan? DailyFrequencyStartingAt { get; set; }
        public TimeSpan? DailyFrequencyEndingAt { get; set; }

        // Limits
        public DateTime? LimitsStartDate { get; set; }
        public DateTime? LimitsEndDate { get; set; }

        // Output and Iterations
        public DateTime? OutputNextExecution { get; set; }
        public DateTime[] OutputIterations { get; set; }

        //Culture
        public string Language { get; set;}

        #endregion
    }

}