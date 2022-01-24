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

            if(this.scheduler.ConfigOccurs == SchedulerDataHelper.OccursConfiguration.Monthly)
            {
                return this.GetDescriptionMonthlyNextExecutionTime();
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
        private DateTime[] CalculateMonthlyOcurrencesList(int limitOcurrences)
        {
            if(this.scheduler.DailyFrequencyStartingAt.HasValue == false)
            {
                throw new SchedulerException("The DailyFrequencyStartingAt must be filled");
            }
            List<DateTime> ocurrencesList = new();
            DateTime currentIterationDateTime = this.scheduler.CurrentDate + this.scheduler.DailyFrequencyStartingAt.Value;
            DateTime oldCurrentIterationDateTime;

            bool firstElementAdded = false;
            int offset = 0;

            while (offset < limitOcurrences)
            {
                oldCurrentIterationDateTime = currentIterationDateTime;

                if (this.scheduler.MonthlyDayEnabled)
                {
                    CalculateOcurrenceListMonthlyDayEnabled(ocurrencesList, ref currentIterationDateTime, oldCurrentIterationDateTime, ref firstElementAdded, ref offset);
                }
                else
                {
                    CalculateOcurrenceListMonthlyTheEnabled(limitOcurrences, ocurrencesList, ref currentIterationDateTime, ref firstElementAdded, ref offset);
                }
            }
            return ocurrencesList.ToArray();
        }

        /// <summary>
        /// Get list of int days based on SchedulerDataHelper.MonthlyDay configuration.
        /// </summary>
        /// <param name="monthlyDay"></param>
        /// <returns></returns>
        private static DayOfWeek[] GetMonthlyDayConfigurationDayOfWeek(SchedulerDataHelper.MonthlyDay monthlyDay)
        {
            List<DayOfWeek> outputDaysOfWeek = new();

            switch (monthlyDay)
            {
                case SchedulerDataHelper.MonthlyDay.Day:
                    Random randomVar = new();
                    var randomWeekDay = randomVar.Next(0, 6);
                    outputDaysOfWeek.Add((DayOfWeek)randomWeekDay);
                    break;
                case SchedulerDataHelper.MonthlyDay.Monday:
                    outputDaysOfWeek.Add(DayOfWeek.Monday);
                    break;
                case SchedulerDataHelper.MonthlyDay.Tuesday:
                    outputDaysOfWeek.Add(DayOfWeek.Tuesday);
                    break;
                case SchedulerDataHelper.MonthlyDay.Wednesday:
                    outputDaysOfWeek.Add(DayOfWeek.Wednesday);
                    break;
                case SchedulerDataHelper.MonthlyDay.Thursday:
                    outputDaysOfWeek.Add(DayOfWeek.Thursday);
                    break;
                case SchedulerDataHelper.MonthlyDay.Friday:
                    outputDaysOfWeek.Add(DayOfWeek.Friday);
                    break;
                case SchedulerDataHelper.MonthlyDay.Saturday:
                    outputDaysOfWeek.Add(DayOfWeek.Saturday);
                    break;
                case SchedulerDataHelper.MonthlyDay.Sunday:
                    outputDaysOfWeek.Add(DayOfWeek.Sunday);
                    break;
                case SchedulerDataHelper.MonthlyDay.Weekday:
                    for(int index = 1; index <= 5; index++)
                    {
                        outputDaysOfWeek.Add((DayOfWeek)index);
                    }
                    break;
                case SchedulerDataHelper.MonthlyDay.WeekendDay:
                    outputDaysOfWeek.Add(DayOfWeek.Saturday);
                    outputDaysOfWeek.Add(DayOfWeek.Sunday);
                    break;
            }
            return outputDaysOfWeek.ToArray();
        }

        /// <summary>
        /// Calculate the OcurrenceList when TheConfig is enabled
        /// </summary>
        /// <param name="limitOcurrences"></param>
        /// <param name="ocurrencesList"></param>
        /// <param name="currentIterationDateTime"></param>
        /// <param name="firstElementAdded"></param>
        /// <param name="offset"></param>
        private void CalculateOcurrenceListMonthlyTheEnabled(int limitOcurrences, List<DateTime> ocurrencesList, ref DateTime currentIterationDateTime, ref bool firstElementAdded, ref int offset)
        {
            if (this.scheduler.MonthlyTheEveryMonths.HasValue)
            {
                DateTime[] calculateDateTime = GetValidDaysOfWeekToMonthlyCalculation(currentIterationDateTime, this.scheduler.MonthlyTheFreqency,
               GetMonthlyDayConfigurationDayOfWeek(this.scheduler.MonthlyTheDay));

                foreach (DateTime eachDateTime in calculateDateTime)
                {
                    if (currentIterationDateTime.Date != eachDateTime.Date)
                    {
                        currentIterationDateTime = eachDateTime;
                    }

                    while (currentIterationDateTime.Date == eachDateTime.Date && offset < limitOcurrences)
                    {
                        if (!firstElementAdded)
                        {
                            ocurrencesList.Add(currentIterationDateTime);
                            firstElementAdded = true;
                            offset++;
                        }

                        if (this.scheduler.DailyFrequencyOnceAtEnabled)
                        {
                            currentIterationDateTime = this.CalculateOnceNextCurrentIteration(currentIterationDateTime, ocurrencesList);
                            offset++;
                        }
                        else
                        {
                            var actualCountOcurrenceList = ocurrencesList.Count;
                            currentIterationDateTime =
                                this.CalculateEveryNextCurrentIteration(currentIterationDateTime, ocurrencesList);
                            if (ocurrencesList.Count != actualCountOcurrenceList)
                            {
                                offset++;
                            }
                        }
                    }
                }
                DateTime addedMonthsDateTime = currentIterationDateTime.AddDays(-1).AddMonths(this.scheduler.MonthlyTheEveryMonths.Value);
                currentIterationDateTime = new DateTime(addedMonthsDateTime.Year, addedMonthsDateTime.Month, 1) + this.scheduler.DailyFrequencyStartingAt.Value;
                firstElementAdded = false;
            }
        }

        /// <summary>
        /// Calculate the OcurrenceList when MonthlyDay is enabled
        /// </summary>
        /// <param name="ocurrencesList"></param>
        /// <param name="currentIterationDateTime"></param>
        /// <param name="oldCurrentIterationDateTime"></param>
        /// <param name="firstElementAdded"></param>
        /// <param name="offset"></param>
        private void CalculateOcurrenceListMonthlyDayEnabled(List<DateTime> ocurrencesList, ref DateTime currentIterationDateTime, DateTime oldCurrentIterationDateTime, ref bool firstElementAdded, ref int offset)
        {
            this.AddMonthIfNeeded(
                currentIterationDateTime,
                this.scheduler.MonthlyDayEveryDay.Value,
                this.scheduler.DailyFrequencyStartingAt.Value);

            if (currentIterationDateTime.Day < this.scheduler.MonthlyDayEveryDay)
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
                    offset++;
                }

                currentIterationDateTime = GetCurrentIterationDateTimeMonthsIteration(currentIterationDateTime);
                if (oldCurrentIterationDateTime != currentIterationDateTime)
                {
                    ocurrencesList.Add(currentIterationDateTime);
                    offset++;
                }

                if (this.scheduler.DailyFrequencyOnceAtEnabled)
                {
                    currentIterationDateTime = this.CalculateOnceNextCurrentIteration(currentIterationDateTime, ocurrencesList);
                    offset++;
                }
                else
                {
                    var actualCountOcurrenceList = ocurrencesList.Count;
                    currentIterationDateTime =
                        this.CalculateEveryNextCurrentIteration(currentIterationDateTime, ocurrencesList);
                    if (ocurrencesList.Count != actualCountOcurrenceList)
                    {
                        offset++;
                    }
                }
            }
        }

        /// <summary>
        /// Method used to get the Valid Days of week to calculate the monthly ocurrence list
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="monthlyFrequency"></param>
        /// <param name="daysOfWeekToObtain"></param>
        /// <returns></returns>
        private static DateTime[] GetValidDaysOfWeekToMonthlyCalculation(DateTime startDateTime, SchedulerDataHelper.MonthlyFrequency monthlyFrequency, DayOfWeek[] daysOfWeekToObtain)
        {
            DateTime[] listDateTime = DateTimeExtensions.GetDaysOfWeek(startDateTime, monthlyFrequency);
            List<DateTime> outputDateTime = new();

            if (daysOfWeekToObtain.Any() && listDateTime.Any())
            {
                List<DateTime> datesOfWeek = new();
                bool isEndOfWeek = false;
                bool isEndOfMonth = false;

                while (!isEndOfWeek && !isEndOfMonth)
                {
                    foreach (DateTime eachDateTime in listDateTime)
                    {
                        if (eachDateTime.DayOfWeek == DayOfWeek.Sunday)
                        {
                            datesOfWeek.Add(eachDateTime);
                            isEndOfWeek = true;
                            break;
                        }
                        datesOfWeek.Add(eachDateTime);

                        if(eachDateTime.Month != eachDateTime.AddDays(1).Month)
                        {
                            isEndOfMonth = true;
                        }
                    }
                }

                foreach (DayOfWeek eachDayOfWeek in daysOfWeekToObtain)
                {
                    outputDateTime.AddRange(datesOfWeek.Where(D => D.DayOfWeek == eachDayOfWeek).ToList());
                }
            }
            return outputDateTime.ToArray();
        } 

        /// <summary>
        /// Method used in ocurrenceList calculation when MonthDay is enabled. Return a new CurrentIteration plus one month and startingAt rightly configurated.
        /// </summary>
        /// <param name="currentIterationDateTime"></param>
        /// <returns></returns>
        private DateTime GetCurrentIterationDateTimeMonthsIteration(DateTime currentIterationDateTime)
        {
            if(this.scheduler.DailyFrequencyEndingAt.HasValue == false)
            {
                throw new SchedulerException("The DailyFrequencyEndingAt must be filled");
            }
            DateTime currentDayEndLimit = currentIterationDateTime.Date + this.scheduler.DailyFrequencyEndingAt.Value;

            if (currentIterationDateTime >= currentDayEndLimit)
            {
                currentIterationDateTime = AddMonthsDayIteration(currentIterationDateTime);
            }

            return currentIterationDateTime;
        }

        /// <summary>
        /// Add a month and return a new Datetime to CurrentIteration. Used when calculating the ocurrenceList.
        /// </summary>
        /// <param name="currentIterationDateTime"></param>
        /// <returns></returns>
        private DateTime AddMonthsDayIteration(DateTime currentIterationDateTime)
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
        /// <param name="currentIteration"></param>
        /// <param name="ocurrenceList"></param>
        /// <param name="ocurrenceList"></param>
        /// <returns></returns>
        private DateTime CalculateOnceNextCurrentIteration(DateTime currentIteration, List<DateTime> ocurrenceList)
        {
            DateTime nextCurrentIterationDateTime;
            currentIteration = currentIteration.Date + this.scheduler.DailyFrequencyOnceAtTime.Value;
            nextCurrentIterationDateTime = currentIteration.AddDays(1);

            AddCurrentIterationOnceToList(currentIteration, ocurrenceList);

            return nextCurrentIterationDateTime;
        }

        /// <summary>
        /// Method used to calculate the NextCurrentIterationDateTime who is neccesary to calculate the Daily/Weekly Ocurrences
        /// </summary>
        /// <param name="currentIteration"></param>
        /// <param name="ocurrenceList"></param>
        /// <param name="ocurrenceList"></param>
        /// <returns></returns>
        private DateTime CalculateEveryNextCurrentIteration(DateTime currentIteration, List<DateTime> ocurrenceList)
        {
            var startAt = this.scheduler.DailyFrequencyStartingAt;
            var endAt = this.scheduler.DailyFrequencyEndingAt;
            DateTime currentDayStartLimit = currentIteration.Date + startAt.Value;
            DateTime currentDayEndLimit = currentIteration.Date + endAt.Value;

            DateTime nextCurrentIterationDateTime =
                AddFreqTimeToDateTime(currentIteration, this.scheduler.DailyFrequencyEveryTime.Value, this.scheduler.DailyFrequencyEveryNumber.Value);

            if (currentIteration.Date >= nextCurrentIterationDateTime.Date)
            {
                currentDayStartLimit = currentIteration.Date + startAt.Value;
                currentDayEndLimit = currentIteration.Date + endAt.Value;
            }

            if (nextCurrentIterationDateTime >= currentDayStartLimit &&
                nextCurrentIterationDateTime <= currentDayEndLimit)
            {
                ocurrenceList.Add(nextCurrentIterationDateTime);
            }
            return nextCurrentIterationDateTime;
        }

        /// <summary>
        /// Add the Current Iteration to the list if less or equal Limit End Date.
        /// **MAYBE NOT NECCESARY**
        /// </summary>
        /// <param name="currentIteration"></param>
        /// <param name="ocurrenceList"></param>
        /// <param name="ocurrenceList"></param>
        private static void AddCurrentIterationOnceToList(DateTime currentIteration, List<DateTime> ocurrenceList)
        {
            ocurrenceList.Add(currentIteration);
        }

        /// <summary>
        /// Method neccesary to add the number of days in weekly configuration. 
        /// Check if DateTimeToCheck is Sunday and add EveryWeeks Value if true.
        /// </summary>
        /// <param name="dateTimeToCheck"></param>
        /// <param name="dateTimeToSet"></param>
        /// <param name="everyWeeks"></param>
        /// <returns></returns>
        private static DateTime CheckSundayAddWeeks(DateTime dateTimeToCheck, DateTime dateTimeToSet, int everyWeeks)
        {
            DateTime returnDate = dateTimeToSet;
            if (dateTimeToCheck.DayOfWeek == DayOfWeek.Sunday)
            {
                var numberOfDaysToAdd = 7 * everyWeeks;
                returnDate = dateTimeToSet.AddDays(numberOfDaysToAdd - 7);
            }
            return returnDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateInput"></param>
        /// <param name="dailyFreqTime"></param>
        /// <param name="numberToAdd"></param>
        /// <returns></returns>
        private static DateTime AddFreqTimeToDateTime(DateTime dateInput, SchedulerDataHelper.DailyFreqTime dailyFreqTime, double numberToAdd)
        {
            var returnDate = dailyFreqTime switch
            {
                SchedulerDataHelper.DailyFreqTime.Hours => dateInput.AddHours(Convert.ToDouble(numberToAdd)),
                SchedulerDataHelper.DailyFreqTime.Minutes => dateInput.AddMinutes(Convert.ToDouble(numberToAdd)),
                SchedulerDataHelper.DailyFreqTime.Second => dateInput.AddSeconds(Convert.ToDouble(numberToAdd)),
                // Use hours as default value
                _ => dateInput.AddHours(Convert.ToDouble(numberToAdd)),
            };
            return returnDate;
        }

        /// <summary>
        /// Check is DateTime is allowed given the scheduler config.
        /// True: Allowed
        /// False: Now allowed
        /// </summary>
        /// <param name="dateTimeToCheck"></param>
        /// <returns></returns>
        private bool IsDateTimeWeeklyDayAllowed(DateTime dateTimeToCheck)
        {
            DayOfWeek dayOfWeek = dateTimeToCheck.DayOfWeek;
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
        /// <param name="nextDateTime"></param>
        /// <param name="limitDateTime"></param>
        /// <param name="typeConfig"></param>
        /// <returns></returns>
        internal static string GetDescriptionNextExecutionTime(DateTime nextDateTime, DateTime limitDateTime, SchedulerDataHelper.TypeConfiguration typeConfig)
        {
            return string.Format(
                    Global.Description_SchedulerNextExecution,
                    GetStringTypeConfiguration(typeConfig),
                    nextDateTime.ToShortDateString(),
                    nextDateTime.ToString("HH:mm"),
                    limitDateTime.ToShortDateString());
        }

        /// <summary>
        /// Method based on the Scheduler info and configuration, get the description of next execution time. Used in monthly Configuration.
        /// </summary>
        /// <returns></returns>
        private string GetDescriptionMonthlyNextExecutionTime()
        {
            if (this.scheduler.MonthlyDayEnabled)
            {
                
                return string.Format(
                Global.Description_SchedulerNextExecutionMonthlyDay,
                this.scheduler.MonthlyDayEveryDay,
                this.scheduler.MonthlyDayEveryMonth,
                this.scheduler.DailyFrequencyEveryNumber,
                this.scheduler.DailyFrequencyEveryTime,
                DateTime.Today.Add(this.scheduler.DailyFrequencyStartingAt.Value).ToString("hh:mm tt"),
                DateTime.Today.Add(this.scheduler.DailyFrequencyEndingAt.Value).ToString("hh:mm tt"),
                this.scheduler.LimitsStartDate.Value.ToShortDateString());
            }
            else  if (this.scheduler.MonthlyTheEnabled)
            {
                return string.Format(
                Global.Description_SchedulerNextExecutionMonthlyEvery,
                this.scheduler.MonthlyTheFreqency,
                this.scheduler.MonthlyTheDay,
                this.scheduler.MonthlyTheEveryMonths,
                this.scheduler.DailyFrequencyEveryNumber,
                this.scheduler.DailyFrequencyEveryTime,
                DateTime.Today.Add(this.scheduler.DailyFrequencyStartingAt.Value).ToString("hh:mm tt"),
                DateTime.Today.Add(this.scheduler.DailyFrequencyEndingAt.Value).ToString("hh:mm tt"),
                this.scheduler.LimitsStartDate.Value.ToShortDateString());
            }
            else
            {
                return string.Empty;
            }
            
        }

        /// <summary>
        /// Method that obtains the string given a configuration type (TypeConfiguration).
        /// </summary>
        /// <param name="typeConfig"></param>
        /// <returns></returns>
        internal static string GetStringTypeConfiguration(SchedulerDataHelper.TypeConfiguration typeConfig)
        {
            switch (typeConfig)
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
