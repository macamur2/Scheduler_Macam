using Scheduler.Domain.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Scheduler.Domain
{
    /// <summary>
    /// Aux class used to perform different actions with Scheduler received as parameter.
    /// </summary>
    public class SchedulerManager
    {
        private readonly Scheduler scheduler;
        private readonly static DateTime LIMIT_END_DATE = new(2020, 01, 30);

        public SchedulerManager(Scheduler Scheduler)
        {
            this.scheduler = Scheduler ?? throw new SchedulerException(Global.Error_SchedulerNull);
        }

        #region Calculate Date / Description Methods
        /// <summary>
        /// Method used to calculate the OutputNextExecution.
        /// </summary>
        public void CalculateNextDate()
        {
            // Calculate Next Execution
            DateTime? outputDateTime = this.CalculateNextExecutionTime();

            if (!outputDateTime.HasValue)
            {
                throw new SchedulerException(Global.Error_OutputDateTimeNull);
            }
            this.scheduler.OutputNextExecution = outputDateTime.Value;
        }

        /// <summary>
        /// Method used to get the description based on the OutputNextExecution and Scheduler Info/Config
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            if (!this.scheduler.OutputNextExecution.HasValue)
            {
                return string.Empty;
            }
            return GetDescriptionNextExecutionTime(
                    this.scheduler.OutputNextExecution.Value,
                    this.scheduler.LimitsStartDate.Value,
                    this.scheduler.ConfigType.Value
                    );
        }
        #endregion

        #region Calculate Next Execution / Calculate Next Ocurrences
        /// <summary>
        /// Based on the Scheduler info and configuration, calculate the next execution time.
        /// if recurrence exist, get the first element of the recurrence list.
        /// </summary>
        /// <returns></returns>
        internal DateTime? CalculateNextExecutionTime()
        {
            DateTime? nextDateTime;
            if (this.scheduler.ConfigEnabled)
            {
                // Configuration Once
                if (this.scheduler.ConfigType == SchedulerDataHelper.TypeConfiguration.Once)
                {
                    nextDateTime = this.scheduler.ConfigOnceTimeAt;
                }
                // Configuration Recurring
                else
                {
                    this.scheduler.OutputIterations = this.GetListOfOcurrences();
                    nextDateTime = this.GetListOfOcurrences().FirstOrDefault();
                }
            }
            else
            {
                //Mostrar mensaje de error
                throw new SchedulerException("Config is not enabled");
            }
            return nextDateTime;
        }

        /// <summary>
        /// Get the list of Ocurrences, deppending of scheduler ConfigOccurs (Daily / Weekly)
        /// </summary>
        /// <returns></returns>
        internal DateTime[] GetListOfOcurrences()
        {
            if (this.scheduler.ConfigOccurs == SchedulerDataHelper.OccursConfiguration.Daily)
            {
                return this.CalculateDailyOcurrencesList();
            }
            else if (this.scheduler.ConfigOccurs == SchedulerDataHelper.OccursConfiguration.Weekly)
            {
                return this.CalculateWeeklyOcurrencesList();
            }

            return Array.Empty<DateTime>();
        }
        #endregion

        #region Methods Ocurrences List
        /// <summary>
        /// Calculate the weeklyOcurrencesList based on info given by the configuration.
        /// </summary>
        /// <returns></returns>
        private DateTime[] CalculateWeeklyOcurrencesList()
        {
            List<DateTime> ocurrencesList = new();

            DateTime limitEndDate = LIMIT_END_DATE + this.scheduler.DailyFrequencyEndingAt.Value; 
            DateTime currentIterationDateTime = this.scheduler.CurrentDate + this.scheduler.DailyFrequencyStartingAt.Value;
            DateTime oldCurrentIterationDateTime;

            bool firstElementAdded = false;

            while (currentIterationDateTime <= limitEndDate)
            {
                oldCurrentIterationDateTime = currentIterationDateTime;

                if (this.IsDateTimeWeeklyDayAllowed(currentIterationDateTime))
                {
                    if (this.scheduler.DailyFrequencyOnceAtEnabled)
                    {
                        currentIterationDateTime =
                            this.CalculateOnceNextCurrentIteration(currentIterationDateTime, ocurrencesList, limitEndDate);
                    }
                    else
                    {
                        if (!firstElementAdded)
                        {
                            ocurrencesList.Add(currentIterationDateTime);
                            firstElementAdded = true;
                        }

                        currentIterationDateTime =
                            this.CalculateEveryNextCurrentIteration(currentIterationDateTime, ocurrencesList, limitEndDate);
                    }
                }
                else
                {

                    currentIterationDateTime =
                        CheckSundayAddWeeks(oldCurrentIterationDateTime, currentIterationDateTime, this.scheduler.WeeklyEvery.Value);
                    currentIterationDateTime = currentIterationDateTime.AddDays(1);
                }
            }
            return ocurrencesList.ToArray();
        }

        /// <summary>
        /// Calculate the dailyOcurrencesList based on info given by the configuration
        /// </summary>
        /// <returns></returns>
        private DateTime[] CalculateDailyOcurrencesList()
        {

            List<DateTime> ocurrencesList = new();
            DateTime limitEndDate = LIMIT_END_DATE + this.scheduler.DailyFrequencyEndingAt.Value;
            DateTime currentIterationDateTime = this.scheduler.CurrentDate + this.scheduler.DailyFrequencyStartingAt.Value;

            // Add first element to ocurrences list
            ocurrencesList.Add(currentIterationDateTime);
            while (currentIterationDateTime <= limitEndDate)
            {
                if (this.scheduler.DailyFrequencyOnceAtEnabled)
                {
                    // Set next iteration as current iteration
                    currentIterationDateTime = this.CalculateOnceNextCurrentIteration(currentIterationDateTime, ocurrencesList, limitEndDate);
                }
                else
                {
                    // Set next iteration as current iteration
                    currentIterationDateTime = this.CalculateEveryNextCurrentIteration(currentIterationDateTime, ocurrencesList, limitEndDate);
                }
            }

            if (ocurrencesList.Any())
            {
                return ocurrencesList.ToArray();
            }
            return Array.Empty<DateTime>();
        }

        #endregion

        #region Aux. Ocurrence Methods

        /// <summary>
        /// Calculate the Next Current Iteration used in Once configuration.
        /// </summary>
        /// <param name="CurrentIteration"></param>
        /// <param name="OcurrenceList"></param>
        /// <param name="LimitEndDate"></param>
        /// <returns></returns>
        private DateTime CalculateOnceNextCurrentIteration(DateTime CurrentIteration, List<DateTime> OcurrenceList, DateTime LimitEndDate)
        {
            DateTime nextCurrentIterationDateTime;
            CurrentIteration = CurrentIteration.Date + this.scheduler.DailyFrequencyOnceAtTime.Value;
            nextCurrentIterationDateTime = CurrentIteration.AddDays(1);

            AddCurrentIterationOnceToList(CurrentIteration, OcurrenceList, LimitEndDate);

            return nextCurrentIterationDateTime;
        }

        /// <summary>
        /// Method used to calculate the NextCurrentIterationDateTime who is neccesary to calculate the Daily/Weekly Ocurrences
        /// </summary>
        /// <param name="CurrentIteration"></param>
        /// <param name="OcurrenceList"></param>
        /// <param name="LimitEndDate"></param>
        /// <returns></returns>
        private DateTime CalculateEveryNextCurrentIteration(DateTime CurrentIteration, List<DateTime> OcurrenceList, DateTime LimitEndDate)
        {
            var startAt = this.scheduler.DailyFrequencyStartingAt;
            var endAt = this.scheduler.DailyFrequencyEndingAt;
            DateTime currentDayStartLimit = CurrentIteration.Date + startAt.Value;
            DateTime currentDayEndLimit = CurrentIteration.Date + endAt.Value;

            DateTime nextCurrentIterationDateTime =
                AddFreqTimeToDateTime(CurrentIteration, this.scheduler.DailyFrequencyEveryTime.Value, this.scheduler.DailyFrequencyEveryNumber.Value);

            if (CurrentIteration.Date >= nextCurrentIterationDateTime.Date)
            {
                currentDayStartLimit = CurrentIteration.Date + startAt.Value;
                currentDayEndLimit = CurrentIteration.Date + endAt.Value;
            }

            if (nextCurrentIterationDateTime <= LimitEndDate &&
                nextCurrentIterationDateTime >= currentDayStartLimit &&
                nextCurrentIterationDateTime <= currentDayEndLimit)
            {
                OcurrenceList.Add(nextCurrentIterationDateTime);
            }
            return nextCurrentIterationDateTime;
        }

        /// <summary>
        /// Add the Current Iteration to the list if less or equal Limit End Date.
        /// **MAYBE NOT NECCESARY**
        /// </summary>
        /// <param name="CurrentIteration"></param>
        /// <param name="OcurrenceList"></param>
        /// <param name="LimitEndDate"></param>
        private static void AddCurrentIterationOnceToList(DateTime CurrentIteration, List<DateTime> OcurrenceList, DateTime LimitEndDate)
        {
            if (CurrentIteration <= LimitEndDate)
            {
                OcurrenceList.Add(CurrentIteration);
            }
        }

        /// <summary>
        /// Method neccesary to add the number of days in weekly configuration. 
        /// Check if DateTimeToCheck is Sunday and add EveryWeeks Value if true.
        /// </summary>
        /// <param name="DateTimeToCheck"></param>
        /// <param name="DateTimeToSet"></param>
        /// <param name="EveryWeeks"></param>
        /// <returns></returns>
        private static DateTime CheckSundayAddWeeks(DateTime DateTimeToCheck, DateTime DateTimeToSet, int EveryWeeks)
        {
            DateTime returnDate = DateTimeToSet;
            if (DateTimeToCheck.DayOfWeek == DayOfWeek.Sunday)
            {
                var numberOfDaysToAdd = 7 * EveryWeeks;
                returnDate = DateTimeToSet.AddDays(numberOfDaysToAdd - 7);
            }
            return returnDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="DailyFreqTime"></param>
        /// <param name="NumberToAdd"></param>
        /// <returns></returns>
        private static DateTime AddFreqTimeToDateTime(DateTime Date, SchedulerDataHelper.DailyFreqTime DailyFreqTime, double NumberToAdd)
        {
            var returnDate = DailyFreqTime switch
            {
                SchedulerDataHelper.DailyFreqTime.Hours => Date.AddHours(Convert.ToDouble(NumberToAdd)),
                SchedulerDataHelper.DailyFreqTime.Minutes => Date.AddMinutes(Convert.ToDouble(NumberToAdd)),
                SchedulerDataHelper.DailyFreqTime.Second => Date.AddSeconds(Convert.ToDouble(NumberToAdd)),
                // Use hours as default value
                _ => Date.AddHours(Convert.ToDouble(NumberToAdd)),
            };
            return returnDate;
        }

        /// <summary>
        /// Check is DateTime is allowed given the scheduler config.
        /// True: Allowed
        /// False: Now allowed
        /// </summary>
        /// <param name="DateTimeToCheck"></param>
        /// <returns></returns>
        private bool IsDateTimeWeeklyDayAllowed(DateTime DateTimeToCheck)
        {
            DayOfWeek dayOfWeek = DateTimeToCheck.DayOfWeek;
            if (this.scheduler.WeeklyMonday && dayOfWeek == DayOfWeek.Monday) { return true; }
            if (this.scheduler.WeeklyTuesday && dayOfWeek == DayOfWeek.Tuesday) { return true; }
            if (this.scheduler.WeeklyWednesday && dayOfWeek == DayOfWeek.Wednesday) { return true; }
            if (this.scheduler.WeeklyThursday && dayOfWeek == DayOfWeek.Thursday) { return true; }
            if (this.scheduler.WeeklyFriday && dayOfWeek == DayOfWeek.Friday) { return true; }
            if (this.scheduler.WeeklySaturday && dayOfWeek == DayOfWeek.Saturday) { return true; }
            if (this.scheduler.WeeklySunday && dayOfWeek == DayOfWeek.Sunday) { return true; }
            return false;
        }
        #endregion

        #region Methods Description / String
        /// <summary>
        /// Method who based on the Scheduler info and configuration, get the description of the next execution time.
        /// </summary>
        /// <param name="NextDateTime"></param>
        /// <param name="LimitDateTime"></param>
        /// <param name="TypeConfig"></param>
        /// <returns></returns>
        internal static string GetDescriptionNextExecutionTime(DateTime NextDateTime, DateTime LimitDateTime, SchedulerDataHelper.TypeConfiguration TypeConfig)
        {
            string descriptionOut =
                string.Format(
                    Global.Description_SchedulerNextExecution,
                    GetStringTypeConfiguration(TypeConfig),
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
        internal static string GetStringTypeConfiguration(SchedulerDataHelper.TypeConfiguration TypeConfig)
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
