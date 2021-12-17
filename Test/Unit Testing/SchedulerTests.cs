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
        public void Check_Once_Description_Is_Not_Empty()
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
        public void Check_Recurring_Description_Is_Not_Empty()
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

            Assert.NotEmpty(schedulerManager.GetDescription());
        }


        [Fact]
        public void Daily_Once_One_Day_Is_Correct()
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

            Assert.Equal(new DateTime(2020, 01, 08, 14, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.True(schedulerManager.GetDescription().Equals("Occurs Once, Schedule will be used on 08/01/2020 at 14:00 starting on 01/01/2020"));
        }

        [Fact]
        public void Daily_Once_One_Day_Is_Not_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Once,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                ConfigOnceTimeAt = new DateTime(2020, 01, 08, 14, 00, 00),
                DailyFrequencyOnceAtEnabled = true,
                DailyFrequencyOnceAtTime = new TimeSpan(01, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.Equal(new DateTime(2020, 01, 08, 14, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.True(schedulerManager.GetDescription().Equals("Occurs Once, Schedule will be used on 08/01/2020 at 14:00 starting on 01/01/2020"));
        }

        [Fact]
        public void Daily_Recurring_Every_Two_Hours_And_Calculate_Two_Days_Is_Correct()
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

            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 01, 6, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 01, 8, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 02, 4, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 01, 02, 6, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 01, 02, 8, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.True(schedulerManager.GetDescription().Equals("Occurs Recurring, Schedule will be used on 01/01/2020 at 04:00 starting on 01/01/2020"));
        }

        [Fact]
        public void Daily_Recurring_Every_Two_Hours_And_Calculate_Two_Days_Is_Not_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(02, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.NotEqual(new DateTime(2020, 01, 01, 4, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.NotEqual(new DateTime(2020, 01, 01, 6, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.NotEqual(new DateTime(2020, 01, 01, 8, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.NotEqual(new DateTime(2020, 01, 02, 4, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.NotEqual(new DateTime(2020, 01, 02, 6, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.NotEqual(new DateTime(2020, 01, 02, 8, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.False(schedulerManager.GetDescription().Equals("Occurs Once, Schedule will be used on 08/01/2020 at 14:00 starting on 01/01/2020"));
        }

        [Fact]
        public void Daily_Recurring_Every_Two_Minutes_And_Calculate_Recurring_Date_Is_Correct()
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

            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 2, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 4, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 6, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 8, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 10, 0), schedulerObject.OutputIterations[5]);
            Assert.True(schedulerManager.GetDescription().Equals("Occurs Recurring, Schedule will be used on 01/01/2020 at 04:00 starting on 01/01/2020"));
        }

        [Fact]
        public void Daily_Recurring_Every_Two_Minutes_And_Calculate_Recurring_Date_Is_Not_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Minutes,
                DailyFrequencyStartingAt = new TimeSpan(05, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(10, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.NotEqual(new DateTime(2020, 01, 01, 4, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.False(schedulerManager.GetDescription().Equals("Occurs Recurring, Schedule will be used on 01/01/2020 at 04:00 starting on 01/01/2020"));
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

            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 2), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 4), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 6), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 8), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 10), schedulerObject.OutputIterations[5]);
            Assert.True(schedulerManager.GetDescription().Equals("Occurs Recurring, Schedule will be used on 01/01/2020 at 04:00 starting on 01/01/2020"));
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
                DailyFrequencyStartingAt = new TimeSpan(04, 00, 02),
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 02),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.NotEqual(new DateTime(2020, 01, 01, 4, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.False(schedulerManager.GetDescription().Equals("Occurs Once, Schedule will be used on 01/01/2020 at 04:00 starting on 01/01/2020"));
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

            Assert.Equal(new DateTime(2020, 01, 02, 4, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 02, 6, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 02, 8, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 03, 4, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 01, 03, 6, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 01, 03, 8, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 01, 13, 4, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2020, 01, 13, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2020, 01, 13, 8, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2020, 01, 16, 4, 0, 0), schedulerObject.OutputIterations[9]);
            Assert.Equal(new DateTime(2020, 01, 16, 6, 0, 0), schedulerObject.OutputIterations[10]);
            Assert.Equal(new DateTime(2020, 01, 16, 8, 0, 0), schedulerObject.OutputIterations[11]);
            Assert.Equal(new DateTime(2020, 01, 17, 4, 0, 0), schedulerObject.OutputIterations[12]);

            Assert.True(schedulerManager.GetDescription().Equals("Occurs Recurring, Schedule will be used on 02/01/2020 at 04:00 starting on 01/01/2020"));
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
                LimitsStartDate = new DateTime(2020, 01, 04)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.NotEqual(new DateTime(2020, 01, 04, 4, 0, 0), schedulerObject.OutputNextExecution);
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

            Assert.Equal(new DateTime(2020, 01, 02, 2, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 03, 2, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 13, 2, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 16, 2, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 01, 17, 2, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 01, 27, 2, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 01, 30, 2, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.True(schedulerManager.GetDescription().Equals("Occurs Recurring, Schedule will be used on 02/01/2020 at 02:00 starting on 01/01/2020"));
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
                DailyFrequencyOnceAtTime = new TimeSpan(06, 00, 00),
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(05, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 02)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.NotEqual(new DateTime(2020, 01, 02, 2, 0, 0), schedulerObject.OutputNextExecution);
            Assert.True(schedulerManager.GetDescription().Equals("Occurs Recurring, Schedule will be used on 02/01/2020 at 06:00 starting on 02/01/2020"));
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
