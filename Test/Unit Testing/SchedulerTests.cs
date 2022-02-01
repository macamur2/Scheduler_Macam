using Scheduler.Domain;
using System;
using System.Globalization;
using Xunit;

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

            string nextDateTime = new DateTime(2020, 01, 08).ToShortDateString();
            string startDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.Equal(new DateTime(2020, 01, 08, 14, 0, 0), schedulerObject.OutputNextExecution);
            Assert.True(schedulerManager.GetDescription()
                .Equals($"Occurs Once, Schedule will be used on {nextDateTime} at 14:00 starting on {startDateTime}"));
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

            string nextDateTime = new DateTime(2020, 01, 08).ToShortDateString();
            string startDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.Equal(new DateTime(2020, 01, 08, 14, 0, 0), schedulerObject.OutputNextExecution);
            Assert.True(schedulerManager.GetDescription()
                .Equals($"Occurs Once, Schedule will be used on {nextDateTime} at 14:00 starting on {startDateTime}"));
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
            schedulerManager.CalculateNextDate(6);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 01, 6, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 01, 8, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 02, 4, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 01, 02, 6, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 01, 02, 8, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.True(schedulerManager.GetDescription()
                .Equals($"Occurs Recurring, Schedule will be used on {sDateTime} at 04:00 starting on {sDateTime}"));
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
            schedulerManager.CalculateNextDate(6);

            string nextDateTime = new DateTime(2020, 01, 08).ToShortDateString();
            string startDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.NotEqual(new DateTime(2020, 01, 01, 4, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.NotEqual(new DateTime(2020, 01, 01, 6, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.NotEqual(new DateTime(2020, 01, 01, 8, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.NotEqual(new DateTime(2020, 01, 02, 4, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.NotEqual(new DateTime(2020, 01, 02, 6, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.NotEqual(new DateTime(2020, 01, 02, 8, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.False(schedulerManager.GetDescription()
                .Equals($"Occurs Once, Schedule will be used on {nextDateTime} at 14:00 starting on {startDateTime}"));
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
            schedulerManager.CalculateNextDate(6);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 2, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 4, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 6, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 8, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 10, 0), schedulerObject.OutputIterations[5]);
            Assert.True(schedulerManager.GetDescription()
                .Equals($"Occurs Recurring, Schedule will be used on {sDateTime} at 04:00 starting on {sDateTime}"));
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

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.NotEqual(new DateTime(2020, 01, 01, 4, 0, 0), schedulerObject.OutputNextExecution);
            Assert.False(schedulerManager.GetDescription()
                .Equals($"Occurs Recurring, Schedule will be used on {sDateTime} at 04:00 starting on {sDateTime}"));
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
            schedulerManager.CalculateNextDate(6);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 2), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 4), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 6), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 8), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 10), schedulerObject.OutputIterations[5]);
            Assert.True(schedulerManager.GetDescription()
                .Equals($"Occurs Recurring, Schedule will be used on {sDateTime} at 04:00 starting on {sDateTime}"));
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

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.NotEqual(new DateTime(2020, 01, 01, 4, 0, 0), schedulerObject.OutputNextExecution);
            Assert.False(schedulerManager.GetDescription()
                .Equals($"Occurs Once, Schedule will be used on {sDateTime} at 04:00 starting on {sDateTime}"));
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
            schedulerManager.CalculateNextDate(13);

            string nextDateTime = new DateTime(2020, 01, 02).ToShortDateString();
            string startDateTime = new DateTime(2020, 01, 01).ToShortDateString();

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

            Assert.True(schedulerManager.GetDescription()
                .Equals($"Occurs Recurring, Schedule will be used on {nextDateTime} at 04:00 starting on {startDateTime}"));
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
            schedulerManager.CalculateNextDate(7);

            string nextDateTime = new DateTime(2020, 01, 02).ToShortDateString();
            string startDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.Equal(new DateTime(2020, 01, 02, 2, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 03, 2, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 13, 2, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 16, 2, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 01, 17, 2, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 01, 27, 2, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 01, 30, 2, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.True(schedulerManager.GetDescription().Equals($"Occurs Recurring, Schedule will be used on {nextDateTime} at 02:00 starting on {startDateTime}"));
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

            string sDateTime = new DateTime(2020, 01, 02).ToShortDateString();

            Assert.NotEqual(new DateTime(2020, 01, 02, 2, 0, 0), schedulerObject.OutputNextExecution);
            Assert.True(schedulerManager.GetDescription().Equals($"Occurs Recurring, Schedule will be used on {sDateTime} at 06:00 starting on {sDateTime}"));
        }

        [Fact]
        public void Check_DailyFreqencyStartingAt_Is_Null()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyDayEnabled = true,
                MonthlyDayEveryDay = 8,
                MonthlyDayEveryMonth = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<SchedulerException>(() => schedulerManager.CalculateNextDate());

            Assert.True(ex.Message.Equals("The DailyFrequencyStartingAt must be filled"));
        }

        [Fact]
        public void Check_DailyFreqencyEndingAt_Is_Null()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyDayEnabled = true,
                MonthlyDayEveryDay = 8,
                MonthlyDayEveryMonth = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<SchedulerException>(() => schedulerManager.CalculateNextDate());

            Assert.True(ex.Message.Equals("The DailyFrequencyEndingAt must be filled"));
        }

        [Fact]
        public void Monthly_Recurring_Eight_Day_Three_Months_Daily_Every_One_Hour_Three_To_Six_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyDayEnabled = true,
                MonthlyDayEveryDay = 8,
                MonthlyDayEveryMonth = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            Assert.Equal(new DateTime(2020, 01, 08, 3, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 08, 4, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 08, 5, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 08, 6, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 04, 08, 3, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 04, 08, 4, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 04, 08, 5, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2020, 04, 08, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2020, 07, 08, 3, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2020, 07, 08, 4, 0, 0), schedulerObject.OutputIterations[9]);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the 8 day every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }

        [Fact]
        public void Monthly_Recurring_Eight_The_First_Thursday_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.First,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Thursday,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            Assert.Equal(new DateTime(2020, 01, 02, 3, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 02, 4, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 02, 5, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 02, 6, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 04, 02, 3, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 04, 02, 4, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 04, 02, 5, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2020, 04, 02, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2020, 07, 02, 3, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2020, 07, 02, 4, 0, 0), schedulerObject.OutputIterations[9]);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the First Thursday of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }

        [Fact]
        public void Monthly_Recurring_Eight_The_First_Thursday_Of_Every_Three_Months_Is_Not_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.First,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Thursday,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(02, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            Assert.NotEqual(new DateTime(2020, 01, 02, 3, 0, 0), schedulerObject.OutputNextExecution);
        }

        [Fact]
        public void Monthly_Recurring_Eight_The_First_Monday_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.First,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Monday,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            Assert.Equal(new DateTime(2021, 03, 01, 3, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2021, 03, 01, 4, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2021, 03, 01, 5, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2021, 03, 01, 6, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2022, 08, 01, 3, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2022, 08, 01, 4, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2022, 08, 01, 5, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2022, 08, 01, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2023, 05, 01, 3, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2023, 05, 01, 4, 0, 0), schedulerObject.OutputIterations[9]);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the First Monday of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }

        [Fact]
        public void Monthly_Recurring_Eight_The_First_Tuesday_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.First,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Tuesday,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            Assert.Equal(new DateTime(2020, 09, 01, 3, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 09, 01, 4, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 09, 01, 5, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 09, 01, 6, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 12, 01, 3, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 12, 01, 4, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 12, 01, 5, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2020, 12, 01, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2021, 03, 02, 3, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2021, 03, 02, 4, 0, 0), schedulerObject.OutputIterations[9]);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the First Tuesday of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }

        [Fact]
        public void Monthly_Recurring_Eight_The_First_Wednesday_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.First,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Wednesday,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            Assert.Equal(new DateTime(2020, 01, 01, 3, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 01, 5, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 01, 6, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 04, 01, 3, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 04, 01, 4, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 04, 01, 5, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2020, 04, 01, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2020, 07, 01, 3, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2020, 07, 01, 4, 0, 0), schedulerObject.OutputIterations[9]);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the First Wednesday of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }

        [Fact]
        public void Monthly_Recurring_Eight_The_First_Friday_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.First,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Friday,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            Assert.Equal(new DateTime(2020, 01, 03, 3, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 03, 4, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 03, 5, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 03, 6, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 04, 03, 3, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 04, 03, 4, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 04, 03, 5, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2020, 04, 03, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2020, 07, 03, 3, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2020, 07, 03, 4, 0, 0), schedulerObject.OutputIterations[9]);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the First Friday of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }

        [Fact]
        public void Monthly_Recurring_Eight_The_First_Saturday_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.First,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Saturday,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            Assert.Equal(new DateTime(2020, 01, 04, 3, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 04, 4, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 04, 5, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 04, 6, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 04, 04, 3, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 04, 04, 4, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 04, 04, 5, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2020, 04, 04, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2020, 07, 04, 3, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2020, 07, 04, 4, 0, 0), schedulerObject.OutputIterations[9]);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the First Saturday of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }

        [Fact]
        public void Monthly_Recurring_Eight_The_First_Sunday_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.First,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Sunday,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            Assert.Equal(new DateTime(2020, 01, 05, 3, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 05, 4, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 05, 5, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 05, 6, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 04, 05, 3, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 04, 05, 4, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 04, 05, 5, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2020, 04, 05, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2020, 07, 05, 3, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2020, 07, 05, 4, 0, 0), schedulerObject.OutputIterations[9]);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the First Sunday of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }

        [Fact]
        public void Monthly_Recurring_Eight_The_First_Day_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.First,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Day,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            Assert.True(schedulerObject.OutputIterations.Length == 10);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the First Day of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }

        [Fact]
        public void Monthly_Recurring_Eight_The_Second_Thursday_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.Second,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Thursday,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            Assert.Equal(new DateTime(2020, 01, 09, 3, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 09, 4, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 09, 5, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 09, 6, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 04, 09, 3, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 04, 09, 4, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 04, 09, 5, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2020, 04, 09, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2020, 07, 09, 3, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2020, 07, 09, 4, 0, 0), schedulerObject.OutputIterations[9]);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the Second Thursday of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }

        [Fact]
        public void Monthly_Recurring_Eight_The_Third_Thursday_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.Third,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Thursday,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            Assert.Equal(new DateTime(2020, 01, 16, 3, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 16, 4, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 16, 5, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 16, 6, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 04, 16, 3, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 04, 16, 4, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 04, 16, 5, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2020, 04, 16, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2020, 07, 16, 3, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2020, 07, 16, 4, 0, 0), schedulerObject.OutputIterations[9]);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the Third Thursday of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }

        [Fact]
        public void Monthly_Recurring_Eight_The_Fourth_Thursday_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.Fourth,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Thursday,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            Assert.Equal(new DateTime(2020, 01, 23, 3, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 23, 4, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 23, 5, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 23, 6, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 04, 23, 3, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 04, 23, 4, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 04, 23, 5, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2020, 04, 23, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2020, 07, 23, 3, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2020, 07, 23, 4, 0, 0), schedulerObject.OutputIterations[9]);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the Fourth Thursday of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }


        [Fact]
        public void Monthly_Recurring_Eight_The_Last_Thursday_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.Last,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Thursday,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            Assert.Equal(new DateTime(2020, 01, 30, 3, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 30, 4, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 30, 5, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 30, 6, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 04, 30, 3, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 04, 30, 4, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 04, 30, 5, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2020, 04, 30, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2020, 07, 30, 3, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2020, 07, 30, 4, 0, 0), schedulerObject.OutputIterations[9]);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the Last Thursday of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }

        [Fact]
        public void Monthly_Recurring_Eight_The_First_Weekend_Day_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.First,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.WeekendDay,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            Assert.Equal(new DateTime(2020, 01, 04, 3, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 04, 4, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 04, 5, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 04, 6, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 01, 05, 3, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 01, 05, 4, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 01, 05, 5, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2020, 01, 05, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2020, 04, 04, 3, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2020, 04, 04, 4, 0, 0), schedulerObject.OutputIterations[9]);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the First WeekendDay of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }

        [Fact]
        public void Monthly_Recurring_Eight_The_First_Weekday_Day_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.First,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Weekday,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(14);

            Assert.Equal(new DateTime(2020, 01, 01, 3, 0, 0), schedulerObject.OutputIterations[0]);
            Assert.Equal(new DateTime(2020, 01, 01, 4, 0, 0), schedulerObject.OutputIterations[1]);
            Assert.Equal(new DateTime(2020, 01, 01, 5, 0, 0), schedulerObject.OutputIterations[2]);
            Assert.Equal(new DateTime(2020, 01, 01, 6, 0, 0), schedulerObject.OutputIterations[3]);
            Assert.Equal(new DateTime(2020, 01, 02, 3, 0, 0), schedulerObject.OutputIterations[4]);
            Assert.Equal(new DateTime(2020, 01, 02, 4, 0, 0), schedulerObject.OutputIterations[5]);
            Assert.Equal(new DateTime(2020, 01, 02, 5, 0, 0), schedulerObject.OutputIterations[6]);
            Assert.Equal(new DateTime(2020, 01, 02, 6, 0, 0), schedulerObject.OutputIterations[7]);
            Assert.Equal(new DateTime(2020, 01, 03, 3, 0, 0), schedulerObject.OutputIterations[8]);
            Assert.Equal(new DateTime(2020, 01, 03, 4, 0, 0), schedulerObject.OutputIterations[9]);
            Assert.Equal(new DateTime(2020, 01, 03, 5, 0, 0), schedulerObject.OutputIterations[10]);
            Assert.Equal(new DateTime(2020, 01, 03, 6, 0, 0), schedulerObject.OutputIterations[11]);
            Assert.Equal(new DateTime(2020, 04, 01, 3, 0, 0), schedulerObject.OutputIterations[12]);
            Assert.Equal(new DateTime(2020, 04, 01, 4, 0, 0), schedulerObject.OutputIterations[13]);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription().Equals($"Occurs the First Weekday of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
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

        #region Language
        [Fact]
        public void Language_Spanish_Check_Config_Not_Enabled()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = false,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(04, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01),
                Language = "es-ES"
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<SchedulerException>(() => schedulerManager.CalculateNextDate());

            Assert.True(ex.Message.Equals("Config no habilitada"));
        }

        [Fact]
        public void Language_Spanish_Check_Monthly_Daily_Frequency_Starting_At_Is_Null()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 2,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = null,
                DailyFrequencyEndingAt = new TimeSpan(08, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01),
                Language = "es-ES"
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<SchedulerException>(() => schedulerManager.CalculateNextDate());

            Assert.True(ex.Message.Equals("El campo DailyFrequencyStartingAt no puede estar vacío."));
        }

        [Fact]
        public void Language_Spanish_Check_Once_Daily_Next_Execution_Time_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Once,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                ConfigOnceTimeAt = new DateTime(2020, 01, 08, 14, 00, 00),
                DailyFrequencyOnceAtEnabled = true,
                DailyFrequencyOnceAtTime = new TimeSpan(02, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01),
                Language = "es-ES"
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();

            string nextDateTime = new DateTime(2020, 01, 08).ToShortDateString();
            string startDateTime = new DateTime(2020, 01, 01).ToShortDateString();
            string nextTimeSpan = new TimeSpan(14, 00, 00).ToString(@"hh\:mm");

            string description = schedulerManager.GetDescription();

            Assert.True(description.Equals($"Ocurre Una vez, el Scheduler se utilizará el {nextDateTime} a las {nextTimeSpan} a partir de {startDateTime}"));
        }

        [Fact]
        public void Language_English_Check_Recurring_Weekly_Next_Execution_Time_Is_Correct()
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
                LimitsStartDate = new DateTime(2020, 01, 01),
                Language = "en-GB"
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(13);

            string nextDateTime = new DateTime(2020, 01, 02).ToShortDateString();
            string startDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription()
                .Equals($"Occurs Recurring, Schedule will be used on {nextDateTime} at 04:00 starting on {startDateTime}"));
        }

        [Fact]
        public void Language_Not_Selected_Auto_Use_English_Check_Recurring_Weekly_Next_Execution_Time_Is_Correct()
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
            schedulerManager.CalculateNextDate(13);

            string nextDateTime = new DateTime(2020, 01, 02).ToShortDateString();
            string startDateTime = new DateTime(2020, 01, 01).ToShortDateString();

            Assert.True(schedulerManager.GetDescription()
                .Equals($"Occurs Recurring, Schedule will be used on {nextDateTime} at 04:00 starting on {startDateTime}"));
        }

        [Fact]
        public void Language_Culture_Not_Exists_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Once,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily,
                ConfigOnceTimeAt = new DateTime(2020, 01, 08, 14, 00, 00),
                DailyFrequencyOnceAtEnabled = true,
                DailyFrequencyOnceAtTime = new TimeSpan(02, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01),
                Language = "1234"
            };

            var ex = Assert.Throws<SchedulerException>(() => CreateSchedulerManager(schedulerObject));

            Assert.True(ex.Message.Equals("The introduced culture is not correct."));
        }

        [Fact]
        public void Language_Spanish_Monthly_Check_Recurring_Eight_The_Last_Thursday_Of_Every_Three_Months_Is_Correct()
        {
            Domain.Scheduler schedulerObject = new()
            {
                ConfigEnabled = true,
                ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring,
                ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Monthly,
                MonthlyTheEnabled = true,
                MonthlyTheFreqency = SchedulerDataHelper.MonthlyFrequency.Last,
                MonthlyTheDay = SchedulerDataHelper.MonthlyDay.Thursday,
                MonthlyTheEveryMonths = 3,
                DailyFrequencyEveryEnabled = true,
                DailyFrequencyEveryNumber = 1,
                DailyFrequencyEveryTime = SchedulerDataHelper.DailyFreqTime.Hours,
                DailyFrequencyStartingAt = new TimeSpan(03, 00, 00),
                DailyFrequencyEndingAt = new TimeSpan(06, 00, 00),
                LimitsStartDate = new DateTime(2020, 01, 01)
            };

            SchedulerManager schedulerManager = CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate(10);

            string sDateTime = new DateTime(2020, 01, 01).ToShortDateString();


            Assert.True(schedulerManager.GetDescription().Equals($"Ocurre el último Occurs the Last Thursday of every 3 months every 1 Hours between 03:00  and 06:00  starting on {sDateTime}"));
        }
        #endregion

        private static SchedulerManager CreateSchedulerManager(Domain.Scheduler schedulerObject)
        {
            SchedulerManager managerObject = new(schedulerObject);
            return managerObject;
        }
    }
}
