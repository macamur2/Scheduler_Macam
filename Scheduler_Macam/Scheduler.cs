using Scheduler.Domain.Resources;
using System;

namespace Scheduler.Domain
{
    /// <summary>
    /// Scheduler Class used in task operations. Use IScheduler Interface.
    /// </summary>
    public class Scheduler : IScheduler
    {
        #region Fields
        private SchedulerDataHelper.TypeConfiguration? configType;
        private bool configEnabled;
        private DateTime? configOnceTimeAt;
        private SchedulerDataHelper.OccursConfiguration? configOccurs;

        private DateTime? limitsStartDate;
        private DateTime? limitsEndDate;

        private bool dailyFreqOnceAtEnabled;
        private TimeSpan? dailyFreqOnceAtTime;
        private bool dailyFreqEveryEnabled;
        private double? dailyFreqEveryNumber;
        private SchedulerDataHelper.DailyFreqTime? dailyFreqEveryTime;
        private TimeSpan? dailyFreqStartAt;
        private TimeSpan? dailyFreqEndAt;

        private DateTime[] outputIterations;

        private int? weeklyEvery;
        private bool weeklyMonday;
        private bool weeklyTuesday;
        private bool weeklyWednesday;
        private bool weeklyThursday;
        private bool weeklyFriday;
        private bool weeklySaturday;
        private bool weeklySunday;
        #endregion

        #region Properties
        public DateTime CurrentDate { get { return new DateTime(2020, 01, 01); } }
        public SchedulerDataHelper.TypeConfiguration? ConfigType { get { return this.configType; } set { this.configType = value; } }
        public bool ConfigEnabled { get { return this.configEnabled; } set { this.configEnabled = value; } }
        public DateTime? ConfigOnceTimeAt { get { return this.configOnceTimeAt; } set { this.configOnceTimeAt = value; } }
        public SchedulerDataHelper.OccursConfiguration? ConfigOccurs { get { return this.configOccurs; } set { this.configOccurs = value; } }

        // Weekly Configuration
        public int? WeeklyEvery { get { return this.weeklyEvery; } set { this.weeklyEvery = value; } }
        public bool WeeklyMonday { get { return this.weeklyMonday; } set { this.weeklyMonday = value; } }
        public bool WeeklyTuesday { get { return this.weeklyTuesday; } set { this.weeklyTuesday = value; } }
        public bool WeeklyWednesday { get { return this.weeklyWednesday; } set { this.weeklyWednesday = value; } }
        public bool WeeklyThursday { get { return this.weeklyThursday; } set { this.weeklyThursday = value; } }
        public bool WeeklyFriday { get { return this.weeklyFriday; } set { this.weeklyFriday = value; } }
        public bool WeeklySaturday { get { return this.weeklySaturday; } set { this.weeklySaturday = value; } }
        public bool WeeklySunday { get { return this.weeklySunday; } set { this.weeklySunday = value; } }

        // Daily Frequency
        public bool DailyFreqOnceAtEnabled { get { return this.dailyFreqOnceAtEnabled; } set { this.dailyFreqOnceAtEnabled = value; } }
        public TimeSpan? DailyFreqOnceAtTime { get { return this.dailyFreqOnceAtTime; } set { this.dailyFreqOnceAtTime = value; } }
        public bool DailyFreqEveryEnabled { get { return this.dailyFreqEveryEnabled; } set { this.dailyFreqEveryEnabled = value; } }
        public double? DailyFreqEveryNumber { get { return this.dailyFreqEveryNumber; } set { this.dailyFreqEveryNumber = value; } }
        public SchedulerDataHelper.DailyFreqTime? DailyFreqEveryTime { get { return this.dailyFreqEveryTime; } set { this.dailyFreqEveryTime = value; } }
        public TimeSpan? DailyFreqStartAt { get { return this.dailyFreqStartAt; } set { this.dailyFreqStartAt = value; } }
        public TimeSpan? DailyFreqEndAt { get { return this.dailyFreqEndAt; } set { this.dailyFreqEndAt = value; } }

        // Limits
        public DateTime? LimitsStartDate { get { return this.limitsStartDate; } set { this.limitsStartDate = value; } }
        public DateTime? LimitsEndDate { get { return this.limitsEndDate; } set { this.limitsEndDate = value; } }

        // Output and Iterations
        public DateTime? OutputNextExecution { get; set; }
        public DateTime[] OutputIterations { get { return this.outputIterations; } set { this.outputIterations = value; } }
        #endregion
    }

    /// <summary>
    /// IScheduler Interface.
    /// </summary>
    public interface IScheduler
    {
        DateTime CurrentDate { get; }
        SchedulerDataHelper.TypeConfiguration? ConfigType { get; set; }
        bool ConfigEnabled { get; set; }
        DateTime? ConfigOnceTimeAt { get; set; }
        SchedulerDataHelper.OccursConfiguration? ConfigOccurs { get; set; }
        DateTime? LimitsStartDate { get; set; }
        DateTime? LimitsEndDate { get; set; }
        DateTime? OutputNextExecution { get; set; }
        DateTime[] OutputIterations { get; set; }
        bool DailyFreqOnceAtEnabled { get; set; }
        TimeSpan? DailyFreqOnceAtTime { get; set; }
        bool DailyFreqEveryEnabled { get; set; }
        double? DailyFreqEveryNumber { get; set; }
        SchedulerDataHelper.DailyFreqTime? DailyFreqEveryTime { get; set; }
        TimeSpan? DailyFreqStartAt { get; set; }
        TimeSpan? DailyFreqEndAt { get; set; }
        int? WeeklyEvery { get; set; }
        bool WeeklyMonday { get; set; }
        bool WeeklyTuesday { get; set; }
        bool WeeklyWednesday { get; set; }
        bool WeeklyThursday { get; set; }
        bool WeeklyFriday { get; set; }
        bool WeeklySaturday { get; set; }
        bool WeeklySunday { get; set; }
    }
}