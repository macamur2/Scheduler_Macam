using Scheduler.Domain.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Scheduler.Domain
{
    /// <summary>
    /// Aux class used to perform different actions with IScheduler received as parameter.
    /// </summary>
    public class SchedulerManager
    {
        private readonly IScheduler scheduler;
        private readonly static DateTime LIMIT_END_DATE = new(2020, 01, 30);

        public SchedulerManager(IScheduler Scheduler)
        {
            this.scheduler = Scheduler ?? throw new Exception(Global.Error_SchedulerNull);
        }

        #region Calculate Date / Description Methods
        /// <summary>
        /// Method used to calculate the OutputNextExecution.
        /// </summary>
        public void CalculateNextDate()
        {
            // Calculate Next Execution
            DateTime? outputDateTime = this.CalculateNextExecutionTime();

            if (outputDateTime.HasValue == false)
            {
                throw new Exception(Global.Error_OutputDateTimeNull);
            }
            this.scheduler.OutputNextExecution = outputDateTime.Value;
        }

        /// <summary>
        /// Method used to get the description based on the OutputNextExecution and Scheduler Info/Config
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            if (this.scheduler.OutputNextExecution.HasValue == false)
            {
                return string.Empty;
            }
            return this.GetDescriptionNextExecutionTime(
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
            try
            {
                this.ValidateInput();

                DateTime? nextDateTime;
                if (this.scheduler.ConfigEnabled)
                {
                    // Configuration Once
                    if (this.scheduler.ConfigType == SchedulerDataHelper.TypeConfiguration.Once)
                    {
                        this.ValidateConfiguration(false);
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
                    throw new Exception("Config is not enabled");
                }
                return nextDateTime;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the list of Ocurrences, deppending of scheduler ConfigOccurs (Daily / Weekly)
        /// </summary>
        /// <returns></returns>
        internal DateTime[] GetListOfOcurrences()
        {


            if (this.scheduler.ConfigOccurs == SchedulerDataHelper.OccursConfiguration.Daily)
            {
                this.ValidateConfiguration(false);
                return this.CalculateDailyOcurrencesList();
            }
            else if (this.scheduler.ConfigOccurs == SchedulerDataHelper.OccursConfiguration.Weekly)
            {
                this.ValidateConfiguration(true);
                return this.CalculateWeeklyOcurrencesList();
            }
            return null;
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

            DateTime limitEndDate = LIMIT_END_DATE + this.scheduler.DailyFreqEndAt.Value; // To not overflow, it is necessary to establish end limit. "this.scheduler.LimitsEndDate + endAt.Value"
            DateTime currentIterationDateTime = this.scheduler.CurrentDate + this.scheduler.DailyFreqStartAt.Value; //Actual Iteration Value
            DateTime oldCurrentIterationDateTime; //Previous DateTime Value

            bool firstElementAdded = false;

            while (currentIterationDateTime <= limitEndDate)
            {
                // Assign the currentIterationDateTime value as the oldIterationValue (previous value)
                oldCurrentIterationDateTime = currentIterationDateTime;

                // Check if current iteration date time is allowed 
                if (this.IsDateTimeWeeklyDayAllowed(currentIterationDateTime))
                {
                    if (this.scheduler.DailyFreqOnceAtEnabled)
                    {
                        currentIterationDateTime =
                            this.CalculateOnceNextCurrentIteration(currentIterationDateTime, ocurrencesList, limitEndDate);
                    }
                    else
                    {
                        //// Add first element to ocurrences list
                        if (firstElementAdded == false)
                        {
                            ocurrencesList.Add(currentIterationDateTime);
                            firstElementAdded = true;
                        }
                        // Calculate and set next iteration date time as current iteration
                        currentIterationDateTime =
                            this.CalculateEveryNextCurrentIteration(currentIterationDateTime, ocurrencesList, limitEndDate);
                    }
                }
                else
                {
                    // Check if OldCurrentIterationDateTime is Sunday.
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
            DateTime limitEndDate = LIMIT_END_DATE + this.scheduler.DailyFreqEndAt.Value; /*this.scheduler.LimitsEndDate*/ //To not overflow, it is necessary to establish end limit.
            DateTime currentIterationDateTime = this.scheduler.CurrentDate + this.scheduler.DailyFreqStartAt.Value;

            // Add first element to ocurrences list
            ocurrencesList.Add(currentIterationDateTime);
            while (currentIterationDateTime <= limitEndDate)
            {
                if (this.scheduler.DailyFreqOnceAtEnabled)
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
            return null;
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

            CurrentIteration = CurrentIteration.Date + this.scheduler.DailyFreqOnceAtTime.Value;
            nextCurrentIterationDateTime = CurrentIteration.AddDays(1);

            //Add the current iteration if less or equel Limit End Date
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
            var startAt = this.scheduler.DailyFreqStartAt;
            var endAt = this.scheduler.DailyFreqEndAt;
            DateTime currentDayStartLimit = CurrentIteration.Date + startAt.Value;
            DateTime currentDayEndLimit = CurrentIteration.Date + endAt.Value;

            // Assign next current iteration as current iteration plus Daily Number (Hours, Minutes, Seconds)
            DateTime nextCurrentIterationDateTime =
                AddFreqTimeToDateTime(CurrentIteration, this.scheduler.DailyFreqEveryTime.Value, this.scheduler.DailyFreqEveryNumber.Value);

            // Check if currentDate  is greather or equal next current iteration.
            // If so, assign the current day start and end limit (ex. 04:00:00 / 08:00:00)
            if (CurrentIteration.Date >= nextCurrentIterationDateTime.Date)
            {
                currentDayStartLimit = CurrentIteration.Date + startAt.Value;
                currentDayEndLimit = CurrentIteration.Date + endAt.Value;
            }

            // Check if the next iteration is in between the limits. Add to list if true.
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

        #region Validations
        private void ValidateConfiguration(bool IsWeekly)
        {
            if (this.scheduler.ConfigType.Value == SchedulerDataHelper.TypeConfiguration.Once &&
                this.scheduler.ConfigOnceTimeAt.HasValue == false)
            {
                throw new Exception(string.Format(
                    Global.Error_EmptyInput, nameof(this.scheduler.ConfigOnceTimeAt)));
            }

            if (this.scheduler.DailyFreqEveryEnabled &&
                this.scheduler.DailyFreqStartAt.HasValue == false)
            {
                throw new Exception(string.Format(
                    Global.Error_EmptyInput, nameof(this.scheduler.DailyFreqStartAt)));
            }

            if (this.scheduler.DailyFreqEveryEnabled == false &&
                this.scheduler.DailyFreqOnceAtEnabled == false)
            {
                throw new Exception(
                    string.Format(Global.Error_EmptyInputBoth, nameof(this.scheduler.DailyFreqEveryEnabled),
                    nameof(this.scheduler.DailyFreqOnceAtEnabled)));
            }

            if (this.scheduler.DailyFreqEveryEnabled &&
                this.scheduler.DailyFreqEndAt.HasValue == false)
            {
                throw new Exception(string.Format(
                    Global.Error_EmptyInput, nameof(this.scheduler.DailyFreqEndAt)));
            }

            if (this.scheduler.DailyFreqEveryEnabled &&
                this.scheduler.DailyFreqOnceAtEnabled)
            {
                throw new Exception(
                    string.Format(Global.Error_DailyInputBoth, nameof(this.scheduler.DailyFreqEveryEnabled),
                    nameof(this.scheduler.DailyFreqOnceAtEnabled)));
            }

            if (this.scheduler.DailyFreqOnceAtEnabled &&
                this.scheduler.DailyFreqOnceAtTime == null)
            {
                throw new Exception(string.Format(Global.Error_EmptyInput, nameof(this.scheduler.DailyFreqOnceAtTime)));
            }

            if (this.scheduler.DailyFreqEveryEnabled)
            {
                if (this.scheduler.DailyFreqEveryTime.HasValue == false)
                {
                    throw new Exception(string.Format(
                    Global.Error_EmptyInput, nameof(this.scheduler.DailyFreqEveryTime)));
                }

                if (this.scheduler.DailyFreqEveryNumber.HasValue == false)
                {
                    throw new Exception(string.Format(
                    Global.Error_EmptyInput, nameof(this.scheduler.DailyFreqEveryNumber)));
                }

                if (this.scheduler.DailyFreqStartAt.HasValue &&
                    this.scheduler.DailyFreqEndAt.HasValue &&
                    this.scheduler.DailyFreqStartAt.Value > this.scheduler.DailyFreqEndAt)
                {
                    throw new Exception(
                    string.Format(Global.Error_DateTimeGreater, nameof(this.scheduler.DailyFreqStartAt),
                    nameof(this.scheduler.DailyFreqEndAt)));
                }
            }

            if (IsWeekly)
            {
                if (this.scheduler.WeeklyEvery.HasValue == false)
                {
                    throw new Exception(string.Format(
                        Global.Error_EmptyInput, nameof(this.scheduler.WeeklyEvery)));
                }
            }

        }

        private static bool CheckDateTime(DateTime InputDateTime)
        {
            return DateTime.TryParse(InputDateTime.ToString(), out _);
        }

        private static bool CheckTimeSpan(TimeSpan InputTimeSpan)
        {
            return TimeSpan.TryParse(InputTimeSpan.ToString(), out _);
        }

        private void ValidateInput()
        {
            if (this.scheduler.ConfigType.Value == SchedulerDataHelper.TypeConfiguration.Recurring &&
              this.scheduler.ConfigOccurs.HasValue == false)
            {
                throw new Exception(string.Format(
                    Global.Error_EmptyInput, nameof(this.scheduler.ConfigOccurs)));
            }

            if (Enum.IsDefined(typeof(SchedulerDataHelper.OccursConfiguration), this.scheduler.ConfigOccurs) == false)
            {
                throw new Exception(string.Format(
                    Global.Error_IncorrectValue, nameof(this.scheduler.ConfigOccurs)));
            }

            if (this.scheduler.ConfigOnceTimeAt.HasValue &&
                CheckDateTime(this.scheduler.ConfigOnceTimeAt.Value) == false)
            {
                throw new Exception(string.Format(
                    Global.Error_IncorrectValue, nameof(this.scheduler.ConfigOnceTimeAt)));
            }

            if (this.scheduler.ConfigType.HasValue &&
                Enum.IsDefined(typeof(SchedulerDataHelper.TypeConfiguration), this.scheduler.ConfigType.Value) == false)
            {
                throw new Exception(string.Format(Global.Error_IncorrectValue, nameof(this.scheduler.ConfigType)));
            }

            if (this.scheduler.DailyFreqEndAt.HasValue &&
                CheckTimeSpan(this.scheduler.DailyFreqEndAt.Value) == false)
            {
                throw new Exception(string.Format(Global.Error_IncorrectValue, nameof(this.scheduler.DailyFreqEndAt)));
            }

            if (this.scheduler.DailyFreqEveryNumber.HasValue &&
                this.scheduler.DailyFreqEveryNumber.Value <= 0)
            {
                throw new Exception(string.Format(Global.Error_IncorrectValue, nameof(this.scheduler.DailyFreqEveryNumber)));
            }

            if (this.scheduler.DailyFreqEveryTime.HasValue &&
                Enum.IsDefined(typeof(SchedulerDataHelper.DailyFreqTime), this.scheduler.DailyFreqEveryTime.Value) == false)
            {
                throw new Exception(string.Format(Global.Error_IncorrectValue, nameof(this.scheduler.DailyFreqEveryTime)));
            }

            if (this.scheduler.DailyFreqOnceAtTime.HasValue &&
                CheckTimeSpan(this.scheduler.DailyFreqOnceAtTime.Value) == false)
            {
                throw new Exception(string.Format(Global.Error_IncorrectValue, nameof(this.scheduler.DailyFreqOnceAtTime)));
            }

            if (this.scheduler.DailyFreqStartAt.HasValue &&
                CheckTimeSpan(this.scheduler.DailyFreqStartAt.Value) == false)
            {
                throw new Exception(string.Format(Global.Error_IncorrectValue, nameof(this.scheduler.DailyFreqStartAt)));
            }

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
