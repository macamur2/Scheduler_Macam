using Scheduler.Domain;
using System;
using Xunit;
//using Xunit;

namespace Scheduler.Tests.Unit_Testing
{
    public class SchedulerTests
    {
        #region Scheduler
        [Fact]
        public void Create_Schedule_Daily_Once()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Once,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                ConfigOnceTimeAt = new DateTime(2020, 01, 08, 14, 00, 00),
                DailyFrequencyOnceAtEnabled = true,
                DailyFrequencyOnceAtTime = new TimeSpan(02, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            Assert.True(schedulerObject != null);
        }

        [Fact]
        public void Check_Configuration_Enabled()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Once,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                ConfigOnceTimeAt = new DateTime(2020, 01, 08, 14, 00, 00),
                DailyFrequencyOnceAtEnabled = true,
                DailyFrequencyOnceAtTime = new TimeSpan(02, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            Assert.True(schedulerObject.ConfigEnabled);
        }

        [Fact]
        public void Check_Configuration_Is_Not_Enabled()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = false,
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<SchedulerException>(() => schedulerManager.CalculateNextDate());

            Assert.True(ex.Message.Equals("Config is not enabled"));
        }

        [Fact]
        public void Check_Next_Execution_Time_Is_Null()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Once,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                ConfigOnceTimeAt = new DateTime(2020, 01, 08, 14, 00, 00),
                DailyFrequencyOnceAtEnabled = true,
                DailyFrequencyOnceAtTime = new TimeSpan(02, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            Assert.Null(schedulerObject.OutputNextExecution);
        }

        [Fact]
        public void Check_Next_Execution_Time_Is_Not_Null()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Once,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                ConfigOnceTimeAt = new DateTime(2020, 01, 08, 14, 00, 00),
                DailyFrequencyOnceAtEnabled = true,
                DailyFrequencyOnceAtTime = new TimeSpan(02, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.NotNull(schedulerObject.OutputNextExecution);
        }

        [Fact]
        public void Check_Description_Is_Empty()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Once,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                ConfigOnceTimeAt = new DateTime(2020, 01, 08, 14, 00, 00),
                DailyFrequencyOnceAtEnabled = true,
                DailyFrequencyOnceAtTime = new TimeSpan(02, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);

            Assert.Empty(schedulerManager.GetDescription());
        }

        [Fact]
        public void Check_Description_Is_Not_Empty()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Once,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                ConfigOnceTimeAt = new DateTime(2020, 01, 08, 14, 00, 00),
                DailyFrequencyOnceAtEnabled = true,
                DailyFrequencyOnceAtTime = new TimeSpan(02, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.NotEmpty(schedulerManager.GetDescription());
        }

        [Fact]
        public void Daily_Recurring_Every_Two_Hours_And_Calculate_Next_Date_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(04, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.True(schedulerObject.OutputIterations.Length == 90);
        }

        [Fact]
        public void Daily_Recurring_Every_Two_Hours_And_Calculate_Next_Date_Is_Not_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(04, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.False(schedulerObject.OutputIterations.Length == 91);
        }

        [Fact]
        public void Daily_Recurring_Every_Two_Minutes_And_Calculate_Next_Date_Is_Correct()
        {
             Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Minutes,
                DailyFrequencyStartingAt = new TimeSpan(04, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.True(schedulerObject.OutputIterations.Length == 3630);
        }

        [Fact]
        public void Daily_Recurring_Every_Two_Minutes_And_Calculate_Next_Date_Is_Not_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Minutes,
                DailyFrequencyStartingAt = new TimeSpan(04, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.False(schedulerObject.OutputIterations.Length == 3629);
        }

        [Fact]
        public void Daily_Recurring_Every_Two_Seconds_And_Calculate_Next_Date_Is_Correct()
        {
           Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Second,
                DailyFrequencyStartingAt = new TimeSpan(04, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.True(schedulerObject.OutputIterations.Length == 216030);
        }

        [Fact]
        public void Daily_Recurring_Every_Two_Seconds_And_Calculate_Next_Date_Is_Not_Correct()
        {
             Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Second,
                DailyFrequencyStartingAt = new TimeSpan(04, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.False(schedulerObject.OutputIterations.Length == 216035);
        }

        [Fact]
        public void Weekly_Recurring_Every_Two_Weeks_Once_Two_Am_And_Calculate_Next_Date_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Weekly,
                WeeklyEvery = 2,
                WeeklyMonday = true,
                WeeklyThursday = true,
                WeeklyFriday = true,
                DailyFrequencyOnceAtEnabled = true,
                DailyFrequencyOnceAtTime = new TimeSpan(02, 00, 00),
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(04, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.True(schedulerObject.OutputIterations.Length == 7);
        }

        [Fact]
        public void Weekly_Recurring_Every_Two_Weeks_Once_Two_Am_And_Calculate_Next_Date_Is_Not_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Weekly,
                WeeklyEvery = 2,
                WeeklyMonday = false,
                WeeklyThursday = true,
                WeeklyFriday = true,
                DailyFrequencyOnceAtEnabled = true,
                DailyFrequencyOnceAtTime = new TimeSpan(02, 00, 00),
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(04, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.False(schedulerObject.OutputIterations.Length == 7);
        }

        [Fact]
        public void Weekly_Recurring_Every_Two_Weeks_Every_Two_Hours_And_Calculate_Next_Date_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Weekly,
                WeeklyEvery = 2,
                WeeklyMonday = true,
                WeeklyThursday = true,
                WeeklyFriday = true,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(04, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.True(schedulerObject.OutputIterations.Length == 21);
        }

        [Fact]
        public void Weekly_Recurring_Every_Two_Weeks_Every_Two_Hours_And_Calculate_Next_Date_Is_Not_Correct()
        {

            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Weekly,
                WeeklyEvery = 2,
                WeeklyMonday = false,
                WeeklyThursday = true,
                WeeklyFriday = true,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(04, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.False(schedulerObject.OutputIterations.Length == 21);
        }

        #endregion

        #region Manager
        [Fact]
        public void Create_Manager_And_Check_Is_Not_Null()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Once,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                ConfigOnceTimeAt = new DateTime(2020, 01, 08, 14, 00, 00),
                DailyFrequencyOnceAtEnabled = true,
                DailyFrequencyOnceAtTime = new TimeSpan(02, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            Assert.True(CreateSchedulerManager(schedulerObject) != null);
        }
        #endregion

        private static SchedulerManager CreateSchedulerManager(Domain.Scheduler schedulerObject)
        {
            SchedulerManager managerObject = new(schedulerObject);
            return managerObject;
        }
    }
}
