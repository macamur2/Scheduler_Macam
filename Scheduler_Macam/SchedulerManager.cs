using Scheduler.Domain.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler.Domain
{
    /// <summary>
    /// Aux class used to perform different actions with Scheduler received as parameter.
    /// </summary>
    public class SchedulerManager
    {
        private readonly Scheduler scheduler;

        public SchedulerManager(Scheduler Scheduler)
        {
            this.scheduler = Scheduler ?? throw new SchedulerException(Global.Error_SchedulerNull);
        }

        #region Calculate Date / Description Methods
        /// <summary>
        /// Method used to calculate the OutputNextExecution.
        /// </summary>
        public void CalculateNextDate(int limitOcurrences)
        {
            // Calculate Next Execution
            DateTime? outputDateTime = this.CalculateNextExecutionTime(limitOcurrences);

            if (!outputDateTime.HasValue)
            {
                throw new SchedulerException(Global.Error_OutputDateTimeNull);
            }
            this.scheduler.OutputNextExecution = outputDateTime.Value;
        }

        public void CalculateNextDate()
        {
            this.CalculateNextDate(1);
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
        internal DateTime? CalculateNextExecutionTime(int limitOcurrences)
        {
            DateTime? nextDateTime;
            if (this.scheduler.ConfigEnabled)
            {
                // Configuration Once
                if (this.scheduler.ConfigType == SchedulerDataHelper.TypeConfiguration.Once)
                {
                    nextDateTime = this.scheduler.ConfigOnceTimeAt;
                    if (nextDateTime.HasValue)
                    {
                        this.scheduler.OutputIterations = new DateTime[1] { nextDateTime.Value }.ToArray();
                    }
                }
                // Configuration Recurring
                else
                {
                    this.scheduler.OutputIterations = this.GetListOfOcurrences(limitOcurrences);
                    nextDateTime = this.GetListOfOcurrences(limitOcurrences).FirstOrDefault();
                }
            }
            else
            {
                //Mostrar mensaje de error
                throw new SchedulerException("Config is not enabled");
            }
            return nextDateTime;
        }

        internal DateTime? CalculateNextExecutionTime()
        {
            return CalculateNextExecutionTime(1);
        }

        /// <summary>
        /// Get the list of Ocurrences, deppending of scheduler ConfigOccurs (Daily / Weekly)
        /// </summary>
        /// <returns></returns>
        internal DateTime[] GetListOfOcurrences(int limitOcurrences)
        {
            List<DateTime> ocurrencesList = new();
            if (this.scheduler.ConfigOccurs == SchedulerDataHelper.OccursConfiguration.Daily)
            {
                ocurrencesList = this.CalculateDailyOcurrencesList(limitOcurrences).ToList();
            }
            else if (this.scheduler.ConfigOccurs == SchedulerDataHelper.OccursConfiguration.Weekly)
            {
                ocurrencesList =  this.CalculateWeeklyOcurrencesList(limitOcurrences).ToList();
            }
            else if (this.scheduler.ConfigOccurs == SchedulerDataHelper.OccursConfiguration.Monthly)
            {
                ocurrencesList = this.CalculateMonthlyOcurrencesList(limitOcurrences).ToList();
            }
            return ocurrencesList.ToArray();
        }
        #endregion

        private void AddMonthIfNeeded(DateTime currentIteration, int monthlyDay, TimeSpan frequencyStart)
        {
            if (currentIteration.Day > this.scheduler.MonthlyDayEveryDay)
            {
                DateTime addedMonthIteration = currentIteration.AddMonths(1);
                currentIteration = new DateTime(addedMonthIteration.Year, addedMonthIteration.Month, monthlyDay) + frequencyStart;
            }
        }
        #region Methods Ocurrences List
        /// <summary>
        /// Calculate de Monthly Ocurrences based on info given by the configuration.
        /// </summary>
        /// <param name="limitOcurrences"></param>
        /// <returns></returns>
        /// 
        //ToDo MCM: COMPLETE
        private DateTime[] CalculateMonthlyOcurrencesList(int limitOcurrences)
        {
            List<DateTime> ocurrencesList = new();
            DateTime currentIterationDateTime = this.scheduler.CurrentDate + this.scheduler.DailyFrequencyStartingAt.Value;
            DateTime oldCurrentIterationDateTime;

            bool firstElementAdded = false;
            int actualOcurrencesSize;
            int index = 0;

            while (index < limitOcurrences)
            {
                actualOcurrencesSize = ocurrencesList.Count;
                oldCurrentIterationDateTime = currentIterationDateTime;

                if (this.scheduler.MonthlyDayEnabled)
                {
                    this.AddMonthIfNeeded(
                        currentIterationDateTime,
                        this.scheduler.MonthlyDayEveryDay.Value,
                        this.scheduler.DailyFrequencyStartingAt.Value);

                    if(currentIterationDateTime.Day < this.scheduler.MonthlyDayEveryDay)
                    {
                        currentIterationDateTime =
                            new DateTime(
                                currentIterationDateTime.Year,
                                currentIterationDateTime.Month,
                                this.scheduler.MonthlyDayEveryDay.Value) +
                            this.scheduler.DailyFrequencyStartingAt.Value;
                    }
                    else
                    {
                        if (!firstElementAdded)
                        {
                            ocurrencesList.Add(currentIterationDateTime);
                            firstElementAdded = true;
                        }

                        currentIterationDateTime = GetCurrentIterationDateTimeMonthsIteration(currentIterationDateTime);
                        if (oldCurrentIterationDateTime != currentIterationDateTime)
                        {
                            ocurrencesList.Add(currentIterationDateTime);
                        }

                        if (this.scheduler.DailyFrequencyOnceAtEnabled)
                        {
                            currentIterationDateTime = this.CalculateOnceNextCurrentIteration(currentIterationDateTime, ocurrencesList);
                        }
                        else
                        {
                            currentIterationDateTime =
                                this.CalculateEveryNextCurrentIteration(currentIterationDateTime, ocurrencesList);
                        }
                    }
                }
                else
                {
                    /* ToDo MCM: obtener el valor de la frecuencia (Primero, segundo, tercero, cuarto o último)
                     Obtener el valor del día dependiendo + la frecuencia del día Lunes, Martes, Miércoles, etc...
                    Cada X número de meses*/
                    

                }

                if (ocurrencesList.Count != actualOcurrencesSize)
                {
                    index++;
                }
            }
            return ocurrencesList.ToArray();
        }

        private DateTime GetCurrentIterationDateTimeMonthsIteration(DateTime currentIterationDateTime)
        {
            DateTime currentDayEndLimit = currentIterationDateTime.Date + this.scheduler.DailyFrequencyEndingAt.Value;

            if (currentIterationDateTime >= currentDayEndLimit)
            {
                currentIterationDateTime = AddMonthsIteration(currentIterationDateTime);
            }

            return currentIterationDateTime;
        }

        private DateTime AddMonthsIteration(DateTime currentIterationDateTime)
        {
            DateTime sAddMonthIteration = currentIterationDateTime.AddMonths(this.scheduler.MonthlyDayEveryMonth.Value);
            currentIterationDateTime =
                new DateTime(
                    sAddMonthIteration.Year,
                    sAddMonthIteration.Month,
                    this.scheduler.MonthlyDayEveryDay.Value) +
                this.scheduler.DailyFrequencyStartingAt.Value;
            return currentIterationDateTime;
        }

        /// <summary>
        /// Calculate the Weekly Ocurrences based on info given by the configuration.
        /// </summary>
        /// <returns></returns>
        private DateTime[] CalculateWeeklyOcurrencesList(int limitOcurrences)
        {
            List<DateTime> ocurrencesList = new();
            DateTime currentIterationDateTime = this.scheduler.CurrentDate + this.scheduler.DailyFrequencyStartingAt.Value;
            DateTime oldCurrentIterationDateTime;

            bool firstElementAdded = false;
            int actualOcurrencesSize;
            int index = 0;

            while(index < limitOcurrences)
            {
                actualOcurrencesSize = ocurrencesList.Count;
                oldCurrentIterationDateTime = currentIterationDateTime;

                if (this.IsDateTimeWeeklyDayAllowed(currentIterationDateTime))
                {
                    if (this.scheduler.DailyFrequencyOnceAtEnabled)
                    {
                        currentIterationDateTime =
                            this.CalculateOnceNextCurrentIteration(currentIterationDateTime, ocurrencesList);
                    }
                    else
                    {
                        if (!firstElementAdded)
                        {
                            ocurrencesList.Add(currentIterationDateTime);
                            firstElementAdded = true;
                        }

                        currentIterationDateTime =
                            this.CalculateEveryNextCurrentIteration(currentIterationDateTime, ocurrencesList);
                    }
                }
                else
                {

                    currentIterationDateTime =
                        CheckSundayAddWeeks(oldCurrentIterationDateTime, currentIterationDateTime, this.scheduler.WeeklyEvery.Value);
                    currentIterationDateTime = currentIterationDateTime.AddDays(1);
                }

                if(ocurrencesList.Count != actualOcurrencesSize)
                {
                    index++;
                }
            }
            return ocurrencesList.ToArray();
        }

        /// <summary>
        /// Calculate the Daily Ocurrences based on info given by the configuration
        /// </summary>
        /// <returns></returns>
        private DateTime[] CalculateDailyOcurrencesList(int limitOcurrences)
        {
            List<DateTime> ocurrencesList = new();
            DateTime currentIterationDateTime = this.scheduler.CurrentDate + this.scheduler.DailyFrequencyStartingAt.Value;

            int actualOcurrencesSize;
            int index = 0;

            // Add first element to ocurrences list
            ocurrencesList.Add(currentIterationDateTime);

            while (index < limitOcurrences)
            {
                actualOcurrencesSize = ocurrencesList.Count;
                if (this.scheduler.DailyFrequencyOnceAtEnabled)
                {
                    // Set next iteration as current iteration
                    currentIterationDateTime = this.CalculateOnceNextCurrentIteration(currentIterationDateTime, ocurrencesList);
                }
                else
                {
                    // Set next iteration as current iteration
                    currentIterationDateTime = this.CalculateEveryNextCurrentIteration(currentIterationDateTime, ocurrencesList);
                }
                if (ocurrencesList.Count != actualOcurrencesSize)
                {
                    index++;
                }
            }
            return ocurrencesList.ToArray();
        }

        #endregion

        #region Aux. Ocurrence Methods

        /// <summary>
        /// Calculate the Next Current Iteration used in Once configuration.
        /// </summary>
        /// <param name="CurrentIteration"></param>
        /// <param name="OcurrenceList"></param>
        /// <param name="OcurrenceList"></param>
        /// <returns></returns>
        private DateTime CalculateOnceNextCurrentIteration(DateTime CurrentIteration, List<DateTime> OcurrenceList)
        {
            DateTime nextCurrentIterationDateTime;
            CurrentIteration = CurrentIteration.Date + this.scheduler.DailyFrequencyOnceAtTime.Value;
            nextCurrentIterationDateTime = CurrentIteration.AddDays(1);

            AddCurrentIterationOnceToList(CurrentIteration, OcurrenceList);

            return nextCurrentIterationDateTime;
        }

        /// <summary>
        /// Method used to calculate the NextCurrentIterationDateTime who is neccesary to calculate the Daily/Weekly Ocurrences
        /// </summary>
        /// <param name="CurrentIteration"></param>
        /// <param name="OcurrenceList"></param>
        /// <param name="OcurrenceList"></param>
        /// <returns></returns>
        private DateTime CalculateEveryNextCurrentIteration(DateTime CurrentIteration, List<DateTime> OcurrenceList)
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

            if (nextCurrentIterationDateTime >= currentDayStartLimit &&
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
        /// <param name="OcurrenceList"></param>
        private static void AddCurrentIterationOnceToList(DateTime CurrentIteration, List<DateTime> OcurrenceList)
        {
            OcurrenceList.Add(CurrentIteration);
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
