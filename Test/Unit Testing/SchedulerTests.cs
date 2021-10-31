using Scheduler.Domain;
using System;
using Xunit;

namespace Scheduler.Tests.Unit_Testing
{
    public class SchedulerTests
    {
        #region Scheduler
        [Fact]
        public void Create_Schedule_Daily_Once()
        {
            Assert.True(this.CreateSchedulerDailyOnce() != null);
        }

        [Fact]
        public void Create_Schedule_Daily_Recurring()
        {
            Assert.True(this.CreateSchedulerDailyRecurring() != null);
        }

        [Fact]
        public void Check_Configuration_Enabled()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyOnce();
            Assert.True(schedulerObject.ConfigEnabled = true);
        }

        [Fact]
        public void Check_Next_Execution_Time_Is_Null()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyOnce();
            Assert.Null(schedulerObject.OutputNextExecution);
        }

        [Fact]
        public void Check_Next_Execution_Time_Is_Not_Null()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyOnce();
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();
            Assert.NotNull(schedulerObject.OutputNextExecution);
        }

        [Fact]
        public void Check_Description_Is_Empty()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyOnce();
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            Assert.Empty(schedulerManager.GetDescription());
        }

        [Fact]
        public void Check_Description_Is_Not_Empty()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyOnce();
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();
            Assert.NotEmpty(schedulerManager.GetDescription());
        }


        [Fact]
        public void Daily_Recurring_Every_Two_Hours_And_Calculate_Next_Date_Is_Correct()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyRecurring();
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputIterations.Length == 90;
            Assert.True(isCorrectDateTime);
        }
        [Fact]
        public void Daily_Recurring_Every_Two_Hours_And_Calculate_Next_Date_Is_Not_Correct()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyRecurring();
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputIterations.Length == 91;
            Assert.False(isCorrectDateTime);
        }

        [Fact]
        public void Daily_Recurring_Every_Two_Minutes_And_Calculate_Next_Date_Is_Correct()
        {
            //ToDo: CHANGE IT
            IScheduler schedulerObject = this.CreateSchedulerDailyRecurring();
            schedulerObject.DailyFreqEveryEnabled = true;
            schedulerObject.DailyFreqEveryTime = SchedulerDataHelper.DailyFreqTime.Minutes;
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputIterations.Length == 3630;
            Assert.True(isCorrectDateTime);
        }
        [Fact]
        public void Daily_Recurring_Every_Two_Minutes_And_Calculate_Next_Date_Is_Not_Correct()
        {
            //ToDo: CHANGE IT
            IScheduler schedulerObject = this.CreateSchedulerDailyRecurring();
            schedulerObject.DailyFreqEveryEnabled = true;
            schedulerObject.DailyFreqEveryTime = SchedulerDataHelper.DailyFreqTime.Minutes;
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputIterations.Length == 3629;
            Assert.False(isCorrectDateTime);
        }

        [Fact]
        public void Daily_Recurring_Every_Two_Seconds_And_Calculate_Next_Date_Is_Correct()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyRecurring();
            schedulerObject.DailyFreqEveryEnabled = true;
            schedulerObject.DailyFreqEveryTime = SchedulerDataHelper.DailyFreqTime.Second;
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputIterations.Length == 216030;
            Assert.True(isCorrectDateTime);
        }

        [Fact]
        public void Daily_Recurring_Every_Two_Seconds_And_Calculate_Next_Date_Is_Not_Correct()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyRecurring();
            schedulerObject.DailyFreqEveryEnabled = true;
            schedulerObject.DailyFreqEveryTime = SchedulerDataHelper.DailyFreqTime.Second;
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputIterations.Length == 216035;
            Assert.False(isCorrectDateTime);
        }

        [Fact]
        public void Weekly_Recurring_Every_Two_Weeks_Once_Two_Am_And_Calculate_Next_Date_Is_Correct()
        {
            IScheduler schedulerObject = this.CreateSchedulerWeeklyRecurring();
            schedulerObject.ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring;
            schedulerObject.LimitsStartDate = new DateTime(2020, 01, 01);
            schedulerObject.WeeklyEvery = 2;
            schedulerObject.WeeklyMonday = true;
            schedulerObject.WeeklyThursday = true;
            schedulerObject.WeeklyFriday = true;
            schedulerObject.DailyFreqEveryEnabled = false;
            schedulerObject.DailyFreqOnceAtEnabled = true;
            schedulerObject.DailyFreqOnceAtTime = new TimeSpan(02, 00, 00);

            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputIterations.Length == 7;
            Assert.True(isCorrectDateTime);
        }

        [Fact]
        public void Weekly_Recurring_Every_Two_Weeks_Once_Two_Am_And_Calculate_Next_Date_Is_Not_Correct()
        {
            IScheduler schedulerObject = this.CreateSchedulerWeeklyRecurring();
            schedulerObject.ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring;
            schedulerObject.LimitsStartDate = new DateTime(2020, 01, 01);
            schedulerObject.WeeklyEvery = 2;
            schedulerObject.WeeklyMonday = false; /*Change this to false*/
            schedulerObject.WeeklyThursday = true;
            schedulerObject.WeeklyFriday = true;
            schedulerObject.DailyFreqEveryEnabled = false;
            schedulerObject.DailyFreqOnceAtEnabled = true;
            schedulerObject.DailyFreqOnceAtTime = new TimeSpan(02, 00, 00);

            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputIterations.Length == 7;
            Assert.False(isCorrectDateTime);
        }

        [Fact]
        public void Weekly_Recurring_Every_Two_Weeks_Every_Two_Hours_And_Calculate_Next_Date_Is_Correct()
        {
            IScheduler schedulerObject = this.CreateSchedulerWeeklyRecurring();
            schedulerObject.ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring;
            schedulerObject.LimitsStartDate = new DateTime(2020, 01, 01);
            schedulerObject.WeeklyEvery = 2;
            schedulerObject.WeeklyMonday = true;
            schedulerObject.WeeklyThursday = true;
            schedulerObject.WeeklyFriday = true;
            schedulerObject.DailyFreqEveryEnabled = true;
            schedulerObject.DailyFreqEveryNumber = 2;
            schedulerObject.DailyFreqEveryTime = SchedulerDataHelper.DailyFreqTime.Hours;

            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputIterations.Length == 21;
            Assert.True(isCorrectDateTime);
        }

        [Fact]
        public void Weekly_Recurring_Every_Two_Weeks_Every_Two_Hours_And_Calculate_Next_Date_Is_Not_Correct()
        {
            IScheduler schedulerObject = this.CreateSchedulerWeeklyRecurring();
            schedulerObject.ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring;
            schedulerObject.LimitsStartDate = new DateTime(2020, 01, 01);
            schedulerObject.WeeklyEvery = 2;
            schedulerObject.WeeklyMonday = false; /*Change this to false*/
            schedulerObject.WeeklyThursday = true;
            schedulerObject.WeeklyFriday = true;
            schedulerObject.DailyFreqEveryEnabled = true;
            schedulerObject.DailyFreqEveryNumber = 2;
            schedulerObject.DailyFreqEveryTime = SchedulerDataHelper.DailyFreqTime.Hours;

            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerManager.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputIterations.Length == 21;
            Assert.False(isCorrectDateTime);
        }

        #endregion

        #region  Incorrect Configuration
        [Fact]
        public void Check_Scheduler_Type_Configuration_Once_Without_Config_Once_Time_At()
        {
            IScheduler schedulerObject = this.CreateSchedulerWeeklyRecurring();
            schedulerObject.ConfigType = SchedulerDataHelper.TypeConfiguration.Once;
            schedulerObject.ConfigOnceTimeAt = null;

            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The field ConfigOnceTimeAt is empty and is neccesary to operate with the selected configuration. Please check it and try again."));
        }

        [Fact]
        public void Check_Scheduler_Type_Configuration_Recurring_Without_Config_Occurs()
        {
            IScheduler schedulerObject = this.CreateSchedulerWeeklyRecurring();
            schedulerObject.ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring;
            schedulerObject.ConfigOccurs = null;

            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The field ConfigOccurs is empty and is neccesary to operate with the selected configuration. Please check it and try again."));
        }

        [Fact]
        public void Check_Scheduler_Daily_Freq_Every_Enabled_Without_Daily_Freq_End_At()
        {
            IScheduler schedulerObject = this.CreateSchedulerWeeklyRecurring();
            schedulerObject.DailyFreqEveryEnabled = true;
            schedulerObject.DailyFreqEndAt = null;

            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The field DailyFreqEndAt is empty and is neccesary to operate with the selected configuration. Please check it and try again."));
        }

        [Fact]
        public void Check_Scheduler_Daily_Freq_Every_Enabled_Without_Daily_Freq_Start_At()
        {
            IScheduler schedulerObject = this.CreateSchedulerWeeklyRecurring();
            schedulerObject.DailyFreqEveryEnabled = true;
            schedulerObject.DailyFreqEndAt = new TimeSpan(02, 02, 02);
            schedulerObject.DailyFreqStartAt = null;

            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The field DailyFreqStartAt is empty and is neccesary to operate with the selected configuration. Please check it and try again."));
        }

        [Fact]
        public void Check_Scheduler_Daily_Freq_Every_Enabled_False_And_Daily_Freq__Once_At_Enabled_False()
        {
            IScheduler schedulerObject = this.CreateSchedulerWeeklyRecurring();
            schedulerObject.DailyFreqEveryEnabled = false;
            schedulerObject.DailyFreqOnceAtEnabled = false;

            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The fields DailyFreqEveryEnabled and DailyFreqOnceAtEnabled are empty, you must select only one to operate. Please check the configuration and try again."));
        }

        [Fact]
        public void Check_Scheduler_Daily_Freq_Every_Enabled_And_Daily_Freq__Once_At_Enabled()
        {
            IScheduler schedulerObject = this.CreateSchedulerWeeklyRecurring();
            schedulerObject.DailyFreqEveryEnabled = true;
            schedulerObject.DailyFreqOnceAtEnabled = true;

            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The fields DailyFreqEveryEnabled and DailyFreqOnceAtEnabled cannot be activated at same time, you must select only one to operate. Please check the configuration and try again."));
        }

        [Fact]
        public void Check_Scheduler_Daily_Freq_Once_At_Enabled_Without_Daily_Freq_Once_At_Time()
        {
            IScheduler schedulerObject = this.CreateSchedulerWeeklyRecurring();
            schedulerObject.DailyFreqEveryEnabled = false;
            schedulerObject.DailyFreqOnceAtEnabled = true;
            schedulerObject.DailyFreqOnceAtTime = null;

            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The field DailyFreqOnceAtTime is empty and is neccesary to operate with the selected configuration. Please check it and try again."));
        }

        [Fact]
        public void Check_Scheduler_Daily_Freq_Once_Every_Enabled_Without_Daily_Freq_Every_Time()
        {
            IScheduler schedulerObject = this.CreateSchedulerWeeklyRecurring();
            schedulerObject.DailyFreqEveryEnabled = true;
            schedulerObject.DailyFreqOnceAtEnabled = false;
            schedulerObject.DailyFreqEveryTime = null;

            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The field DailyFreqEveryTime is empty and is neccesary to operate with the selected configuration. Please check it and try again."));
        }

        [Fact]
        public void Check_Scheduler_Daily_Freq_Once_Every_Enabled_Without_Daily_Freq_Every_Number()
        {
            IScheduler schedulerObject = this.CreateSchedulerWeeklyRecurring();
            schedulerObject.DailyFreqEveryEnabled = true;
            schedulerObject.DailyFreqOnceAtEnabled = false;
            schedulerObject.DailyFreqEveryTime = SchedulerDataHelper.DailyFreqTime.Hours;
            schedulerObject.DailyFreqEveryNumber = null;

            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The field DailyFreqEveryNumber is empty and is neccesary to operate with the selected configuration. Please check it and try again."));
        }


        [Fact]
        public void Check_Scheduler_Daily_Freq_Once_Every_Enabled_And_Start_At_Grater_Than_End_At()
        {
            IScheduler schedulerObject = this.CreateSchedulerWeeklyRecurring();
            schedulerObject.DailyFreqEveryEnabled = true;
            schedulerObject.DailyFreqOnceAtEnabled = false;
            schedulerObject.DailyFreqEveryTime = SchedulerDataHelper.DailyFreqTime.Hours;
            schedulerObject.DailyFreqEveryNumber = 2;
            schedulerObject.DailyFreqStartAt = new TimeSpan(10, 30, 00);
            schedulerObject.DailyFreqEndAt = new TimeSpan(10, 00, 00);


            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The field DailyFreqStartAt cannot be greater than DailyFreqEndAt. Please check the configuration and try again."));
        }

        #endregion

        #region Input Format
        [Fact]
        public void Check_ConfigOccurs_Input_Is_Not_Valid()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyOnce();
            schedulerObject.ConfigOccurs = (SchedulerDataHelper.OccursConfiguration)10;
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);

            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The ConfigOccurs field has an incorrect value, check the configuration and try again."));
        }

        public void Check_ConfigOnceTimeAt_Input_Is_Not_Valid()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyOnce();
            schedulerObject.ConfigOnceTimeAt = new DateTime(2020, 13, 10); /*Cannot test DateTime*/
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The ConfigOnceTimeAt field has an incorrect value, check the configuration and try again."));
        }

        [Fact]
        public void Check_ConfigType_Input_Is_Not_Valid()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyOnce();
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerObject.ConfigType = (SchedulerDataHelper.TypeConfiguration)10;
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The ConfigType field has an incorrect value, check the configuration and try again."));
        }

        [Fact]
        public void Check_DailyFreqEveryNumber_Input_Is_Not_Valid()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyOnce();
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerObject.DailyFreqEveryNumber = -1;
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The DailyFreqEveryNumber field has an incorrect value, check the configuration and try again."));
        }

        [Fact]
        public void Check_DailyFreqEveryTime_Input_Is_Not_Valid()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyOnce();
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerObject.DailyFreqEveryTime = (SchedulerDataHelper.DailyFreqTime)10;
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The DailyFreqEveryTime field has an incorrect value, check the configuration and try again."));
        }

        public void Check_DailyFreqOnceAtTime_Input_Is_Not_Valid()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyOnce();
            SchedulerManager schedulerManager = this.CreateSchedulerManager(schedulerObject);
            schedulerObject.DailyFreqOnceAtTime = new TimeSpan(26,01,01);  /*Cannot test TimeSpan*/
            var ex = Assert.Throws<Exception>(() => schedulerManager.CalculateNextDate());
            Assert.True(ex.Message.Equals("The DailyFreqOnceAtTime field has an incorrect value, check the configuration and try again."));
        }
        #endregion


        #region Manager
        [Fact]
        public void Create_Manager_And_Check_Is_Not_Null()
        {
            IScheduler schedulerObject = this.CreateSchedulerDailyOnce();
            Assert.True(this.CreateSchedulerManager(schedulerObject) != null);
        }
        #endregion

        private IScheduler CreateSchedulerDailyOnce()
        {
            IScheduler schedulerObject = new Domain.Scheduler();
            schedulerObject.ConfigType = SchedulerDataHelper.TypeConfiguration.Once;
            schedulerObject.ConfigEnabled = true;
            schedulerObject.ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily;
            schedulerObject.ConfigOnceTimeAt = new DateTime(2020, 01, 08, 14, 00, 00);
            schedulerObject.DailyFreqOnceAtEnabled = true;
            schedulerObject.DailyFreqOnceAtTime = new TimeSpan(02,00,00);
            schedulerObject.LimitsStartDate = new DateTime(2020, 01, 01);
            return schedulerObject;
        }
        private IScheduler CreateSchedulerDailyRecurring()
        {
            IScheduler schedulerObject = new Domain.Scheduler();
            schedulerObject.ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring;
            schedulerObject.ConfigEnabled = true;
            schedulerObject.ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily;
            schedulerObject.LimitsStartDate = new DateTime(2020, 01, 01);
            schedulerObject.DailyFreqEveryEnabled = true;
            schedulerObject.DailyFreqEveryNumber = 2;
            schedulerObject.DailyFreqEveryTime = SchedulerDataHelper.DailyFreqTime.Hours;
            schedulerObject.DailyFreqStartAt = new TimeSpan(04, 00, 00);
            schedulerObject.DailyFreqEndAt = new TimeSpan(08, 00, 00);
            return schedulerObject;
        }

        private IScheduler CreateSchedulerWeeklyRecurring()
        {
            IScheduler schedulerObject = new Domain.Scheduler();
            schedulerObject.ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring;
            schedulerObject.ConfigEnabled = true;
            schedulerObject.ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Weekly;
            schedulerObject.LimitsStartDate = new DateTime(2020, 01, 01);
            schedulerObject.WeeklyEvery = 2;
            schedulerObject.WeeklyMonday = true;
            schedulerObject.WeeklyThursday = true;
            schedulerObject.WeeklyFriday = true;
            schedulerObject.DailyFreqEveryEnabled = true;
            schedulerObject.DailyFreqEveryNumber = 2;
            schedulerObject.DailyFreqEveryTime = SchedulerDataHelper.DailyFreqTime.Hours;
            schedulerObject.DailyFreqStartAt = new TimeSpan(04, 00, 00);
            schedulerObject.DailyFreqEndAt = new TimeSpan(08, 00, 00);
            return schedulerObject;
        }

        private SchedulerManager CreateSchedulerManager(IScheduler scheduler)
        {
            SchedulerManager managerObject = new SchedulerManager(scheduler);
            return managerObject;
        }
    }
}
