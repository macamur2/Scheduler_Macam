using Scheduler.Domain.Resources;
using System;

namespace Scheduler.Domain
{
    /// <summary>
    /// Aux class used to perform different actions with IScheduler received as parameter.
    /// </summary>
    public class SchedulerManager
    {
        private IScheduler scheduler;
        public SchedulerManager(IScheduler Scheduler)
        {
            if (Scheduler == null)
            {
                throw new Exception(Global.Error_SchedulerNull);
            }

            this.scheduler = Scheduler;
        }

        #region Calculate Date / Description Methods
        /// <summary>
        /// Based on the Scheduler info and configuration, calculate the next execution time.
        /// </summary>
        /// <returns></returns>
        internal DateTime? CalculateNextExecutionTime()
        {
            DateTime? nextDateTime = null;

            if (this.scheduler.ConfigEnabled)
            {
                //Configuration Once
                if (this.scheduler.ConfigType == SchedulerDataHelper.TypeConfiguration.Once)
                {
                    nextDateTime = this.scheduler.ConfigDateTime;
                }
                //Configuration Recurring
                else
                {
                    if (this.scheduler.ConfigOccurs == SchedulerDataHelper.OccursConfiguration.Daily)
                    {
                        nextDateTime = this.scheduler.CurrentDate.AddDays(this.scheduler.ConfigDays).Date;
                    }
                    //ToDo: Other OccursConfiguration
                }
            }
            return nextDateTime;
        }

        /// <summary>
        /// Method who based on the Scheduler info and configuration, get the description of the next execution time.
        /// </summary>
        /// <param name="NextDateTime"></param>
        /// <param name="LimitDateTime"></param>
        /// <param name="TypeConfig"></param>
        /// <returns></returns>
        internal string GetDescriptionNextExecutionTime(DateTime NextDateTime, DateTime LimitDateTime, SchedulerDataHelper.TypeConfiguration TypeConfig)
        {
            string descriptionOut =
                string.Format(
                    Global.Description_SchedulerNextExecution,
                    this.GetStringTypeConfiguration(TypeConfig),
                    NextDateTime.ToShortDateString(),
                    NextDateTime.ToString("HH:mm"),
                    LimitDateTime.ToShortDateString()
                    );

            return descriptionOut;
        }

        /// <summary>
        /// Method that obtains the string given a configuration type (TypeConfiguration).
        /// </summary>
        /// <param name="TypeConfig"></param>
        /// <returns></returns>
        internal string GetStringTypeConfiguration(SchedulerDataHelper.TypeConfiguration TypeConfig)
        {
            switch (TypeConfig)
            {
                case SchedulerDataHelper.TypeConfiguration.Once:
                    return Global.TypeConfiguration_Once;
                case SchedulerDataHelper.TypeConfiguration.Recurring:
                    return Global.TypeConfiguration_Recurring;
                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}
