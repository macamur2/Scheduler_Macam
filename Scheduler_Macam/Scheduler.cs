using Scheduler.Domain.Resources;
using System;

namespace Scheduler.Domain
{
    /// <summary>
    /// Schedule Class RELLENAR
    /// </summary>
    public class Scheduler : IScheduler
    {
        #region Fields

        private SchedulerDataHelper.TypeConfiguration configType;
        private bool configEnabled;
        private DateTime configDateTime;
        private SchedulerDataHelper.OccursConfiguration configOccurs;
        private int configEveryDay;

        private DateTime limitsStartDate;
        private DateTime limitsEndDate;

        private SchedulerManager schedulerManager;
        #endregion

        #region Properties
        public DateTime CurrentDate { get { return new DateTime(2020,01,04); /*DateTime.Now*/ } }
        public SchedulerDataHelper.TypeConfiguration ConfigType { get { return this.configType; } set { this.configType = value; } }
        public bool ConfigEnabled { get { return this.configEnabled; } set { this.configEnabled = value; } }
        public DateTime ConfigDateTime { get { return this.configDateTime; } set { this.configDateTime = value; } }
        public SchedulerDataHelper.OccursConfiguration ConfigOccurs { get { return this.configOccurs; } set { this.configOccurs = value; } }
        public int ConfigDays { get { return this.configEveryDay; } set { this.configEveryDay = value; } }
        public DateTime LimitsStartDate { get { return this.limitsStartDate; } set { this.limitsStartDate = value; } }
        public DateTime LimitsEndDate { get { return this.limitsEndDate; } set { this.limitsEndDate = value; } }
        public DateTime? OutputNextExecution { get; set; }
        #endregion

        #region Methods
        public SchedulerManager SchedulerManager
        {
            get
            {
                if (this.schedulerManager == null)
                {
                    this.schedulerManager = new SchedulerManager(this);
                }
                return this.schedulerManager;
            }
        }

        public void CalculateNextDate()
        {
            //Calculate Next Execution
            DateTime? outputDateTime = this.SchedulerManager.CalculateNextExecutionTime();
            if (outputDateTime.HasValue == false)
            {
                throw new Exception(Global.Error_OutputDateTimeNull);
            }
            this.OutputNextExecution = outputDateTime.Value;
        }

        public string GetDescription()
        {
            if(this.OutputNextExecution.HasValue == false)
            {
                return string.Empty;
            }
            return this.SchedulerManager.GetDescriptionNextExecutionTime(
                    this.OutputNextExecution.Value,
                    this.LimitsStartDate,
                    this.ConfigType
                    );
        }
        #endregion
    }

    public interface IScheduler
    {
        DateTime CurrentDate { get; }
        SchedulerDataHelper.TypeConfiguration ConfigType { get; set; }
        bool ConfigEnabled { get; set; }
        DateTime ConfigDateTime { get; set; }
        SchedulerDataHelper.OccursConfiguration ConfigOccurs { get; set; }
        int ConfigDays { get; set; }
        DateTime LimitsStartDate { get; set; }
        DateTime LimitsEndDate { get; set; }
        DateTime? OutputNextExecution { get; }
        void CalculateNextDate();
        string GetDescription();
    }
}