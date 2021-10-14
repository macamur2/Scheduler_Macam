using Scheduler.Domain;
using System;
using Xunit;

namespace Test
{
    public class SchedulerTests
    {
        #region Scheduler
        [Fact]
        public void Create_Scheduler()
        {
            Assert.True(this.CreateScheduler() != null);
        }

        [Fact]
        public void Create_Scheduler_And_Check_Configuration_Enabled()
        {
            IScheduler schedulerObject = this.CreateScheduler();
            Assert.True(schedulerObject.ConfigEnabled = true);
        }

        [Fact]
        public void Create_Scheduler_And_Check_Next_Execution_Time_Is_Null()
        {
            IScheduler schedulerObject = this.CreateScheduler();
            Assert.Null(schedulerObject.OutputNextExecution);
        }

        [Fact]
        public void Create_Scheduler_And_Check_Next_Execution_Time_Is_Not_Null()
        {
            IScheduler schedulerObject = this.CreateScheduler();
            schedulerObject.CalculateNextDate();
            Assert.NotNull(schedulerObject.OutputNextExecution);
        }

        [Fact]
        public void Create_Scheduler_And_Check_Description_Is_Empty()
        {
            IScheduler schedulerObject = this.CreateScheduler();
            Assert.Empty(schedulerObject.GetDescription());
        }

        [Fact]
        public void Create_Scheduler_And_Check_Description_Is_Not_Empty()
        {
            IScheduler schedulerObject = this.CreateScheduler();
            schedulerObject.CalculateNextDate();
            Assert.NotEmpty(schedulerObject.GetDescription());
        }

        [Fact]
        public void Create_Scheduler_And_Check_Once_First_Case_Is_Correct()
        {
            IScheduler schedulerObject = this.CreateScheduler();
            schedulerObject.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputNextExecution == new DateTime(2020,01,08, 14, 00, 00);
            Assert.True(isCorrectDateTime);
        }

        [Fact]
        public void Create_Scheduler_And_Check_Once_First_Case_Is_Not_Correct()
        {
            IScheduler schedulerObject = this.CreateScheduler();
            schedulerObject.ConfigDateTime = DateTime.Today;
            schedulerObject.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputNextExecution == new DateTime(2020, 01, 08, 14, 00, 00);
            Assert.False(isCorrectDateTime);
        }

        [Fact]
        public void Create_Scheduler_And_Check_Recurring_Second_Case_Is_Correct()
        {
            IScheduler schedulerObject = this.CreateScheduler();
            schedulerObject.ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring;
            schedulerObject.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputNextExecution == new DateTime(2020, 01, 05, 00, 00, 00);
            Assert.True(isCorrectDateTime);
        }

        [Fact]
        public void Create_Scheduler_And_Check_Recurring_Second_Case_Is_Not_Correct()
        {
            IScheduler schedulerObject = this.CreateScheduler();
            schedulerObject.ConfigDays = 4;
            schedulerObject.ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring;
            schedulerObject.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputNextExecution == new DateTime(2020, 01, 05, 00, 00, 00);
            Assert.False(isCorrectDateTime);
        }

        [Fact]
        public void Create_Scheduler_And_Check_Recurring_Second_Case_Is__Correct_Another_Date()
        {
            IScheduler schedulerObject = this.CreateScheduler();
            schedulerObject.ConfigDays = 4;
            schedulerObject.ConfigType = SchedulerDataHelper.TypeConfiguration.Recurring;
            schedulerObject.CalculateNextDate();
            bool isCorrectDateTime = schedulerObject.OutputNextExecution == new DateTime(2020, 01, 08, 00, 00, 00);
            Assert.True(isCorrectDateTime);
        }
        #endregion

        #region Manager
        [Fact]
        public void Create_Manager_And_Check_Is_Not_Null()
        {
            Assert.True(this.CreateSchedulerManager() != null);
        }
        #endregion



        private IScheduler CreateScheduler()
        {
            IScheduler schedulerObject = new Scheduler.Domain.Scheduler();
            schedulerObject.ConfigType = SchedulerDataHelper.TypeConfiguration.Once;
            schedulerObject.ConfigEnabled = true;
            schedulerObject.ConfigOccurs = SchedulerDataHelper.OccursConfiguration.Daily;
            schedulerObject.ConfigDays = 1;
            schedulerObject.ConfigDateTime = new DateTime(2020, 01, 08, 14, 00, 00);
            schedulerObject.LimitsStartDate = new DateTime(2020, 01, 01);
            return schedulerObject;
        }

        private SchedulerManager CreateSchedulerManager()
        {
            IScheduler schedulerObject = this.CreateScheduler();
            SchedulerManager managerObject = new SchedulerManager(schedulerObject);
            return managerObject;
        }
    }
}
