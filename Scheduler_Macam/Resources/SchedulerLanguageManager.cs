using Scheduler.Domain;
using System.Collections.Generic;
using System.Globalization;
using static Scheduler.Domain.SchedulerDataHelper;

namespace Scheduler_Macam.Resources
{
	public static class SchedulerLanguageManager
	{
		private static Dictionary<string, Dictionary<string, string>> dictionaryResources;
		private static CultureInfo actualCulture;

        private const string spanishLang = "es-ES";
        private const string englishLang = "en-GB";

        public static void Initialize(string LanguageInfo)
        {
            try
            {
                actualCulture = CultureInfo.GetCultureInfo(LanguageInfo);
            }
            catch (CultureNotFoundException)
            {
                actualCulture = CultureInfo.GetCultureInfo("en-GB");
                InitializeDictionaryResources();
                throw new SchedulerException(GetResourceLanguage("Error_Culture"));
            }

            if (dictionaryResources == null)
            {
                InitializeDictionaryResources();
            }
        }

        public static string GetResourceLanguage(string Code)
        {
            string outputString;
            try
            {
                outputString = dictionaryResources[Code][actualCulture.Name];
            }
            catch (KeyNotFoundException)
            {
                outputString = dictionaryResources[Code][englishLang];
            }
            return outputString;
        }

        private static void InitializeDictionaryResources()
        {
            dictionaryResources = new Dictionary<string, Dictionary<string, string>>() 
            {
                {"Error_Culture", new Dictionary<string, string>()
                {
                    {spanishLang, "La cultura introducida no es correcta."},
                    {englishLang, "The introduced culture is not correct."}
                }
                },
                {"Error_SchedulerNull", new Dictionary<string, string>()
                {
                    {spanishLang, "El Scheduler no puede ser nulo. Asegúrese de que la información sea correcta."},
                    {englishLang, "The Scheduler can not be null. Ensure that the info is correct."}
                }
                },
                {"Error_OutputDateTimeNull", new Dictionary<string, string>()
                {
                    {spanishLang, "Hay un problema al calcular el próximo tiempo de ejecución, inténtelo de nuevo más tarde."},
                    {englishLang, "There is a problem calculating the Next Execution Time, please try again later."}
                }
                },
                {"Error_CalculatingOcurrenceList", new Dictionary<string, string>()
                {
                    {spanishLang, "Hay un problema al calcular las ocurrencias, revise la configuración e inténtelo de nuevo más tarde."},
                    {englishLang, "There is a problem calculating the Next Ocurrences, check the configuration and try again later."}
                }
                },
                {"Error_IncorrectValue", new Dictionary<string, string>()
                {
                    {spanishLang, "El campo {0} tiene un valor incorrecto, compruebe la configuración e inténtelo de nuevo."},
                    {englishLang, "The {0} field has an incorrect value, check the configuration and try again."}
                }
                },
                {"Error_EmptyInputBoth", new Dictionary<string, string>()
                {
                    {spanishLang, "Los campos {0} y {1} están vacíos, debe seleccionar solo uno para operar. Compruebe la configuración e inténtelo de nuevo."},
                    {englishLang, "The fields {0} and {1} are empty, you must select only one to operate. Please check the configuration and try again."}
                }
                },
                {"Error_EmptyInput", new Dictionary<string, string>()
                {
                    {spanishLang, "El campo {0} está vacío y es necesario para operar con la configuración seleccionada. Por favor, compruébalo e inténtalo de nuevo."},
                    {englishLang, "The field {0} is empty and is neccesary to operate with the selected configuration. Please check it and try again."}
                }
                },
                {"Error_DateTimeGreater", new Dictionary<string, string>()
                {
                    {spanishLang, "El campo {0} no puede ser mayor que {1}. Compruebe la configuración e inténtelo de nuevo."},
                    {englishLang, "The field {0} cannot be greater than {1}. Please check the configuration and try again."}
                }
                },
                {"Error_DailyInputBoth", new Dictionary<string, string>()
                {
                    {spanishLang, "Los campos {0} y {1} no se pueden activar al mismo tiempo, debe seleccionar solo uno para operar. Compruebe la configuración e inténtelo de nuevo."},
                    {englishLang, "The fields {0} and {1} cannot be activated at same time, you must select only one to operate. Please check the configuration and try again."}
                }
                },
                {"Description_SchedulerNextExecutionMonthlyEvery", new Dictionary<string, string>()
                {
                    {spanishLang, "Ocurre el {0} {1} cada {2} meses, cada {3} {4} entre {5} y las {6} comenzando el {7}"},
                    {englishLang, "Occurs the {0} {1} of every {2} months every {3} {4} between {5} and {6} starting on {7}"}
                }
                },
                {"Description_SchedulerNextExecutionMonthlyDay", new Dictionary<string, string>()
                {
                    {spanishLang, "Ocurre el día {0} cada {1} meses cada {2} {3} entre {4} y {5} a partir de {6}"},
                    {englishLang, "Occurs the {0} day every {1} months every {2} {3} between {4} and {5} starting on {6}"}
                }
                },
                {"Description_SchedulerNextExecution", new Dictionary<string, string>()
                {
                    {spanishLang, "Ocurre {0}, el Scheduler se utilizará el {1} a las {2} a partir de {3}"},
                    {englishLang, "Occurs {0}, Schedule will be used on {1} at {2} starting on {3}"}
                }
                },
                {"Config_NotEnabled", new Dictionary<string, string>()
                {
                    {spanishLang, "Config no habilitada"},
                    {englishLang, "Config is not enabled"}
                }
                },
                {"Error_DailyFrequencyStartingAtEmpty", new Dictionary<string, string>()
                {
                    {spanishLang, "El campo DailyFrequencyStartingAt no puede estar vacío."},
                    {englishLang, "The DailyFrequencyStartingAt must be filled"}
                }
                },
                {"Error_DailyFrequencyEndingAtEmpty", new Dictionary<string, string>()
                {
                    {spanishLang, "El campo DailyFrequencyEndingAt no puede estar vacío."},
                    {englishLang, "The DailyFrequencyEndingAt must be filled"}
                }
                },
                {"TypeConfiguration_Once", new Dictionary<string, string>()
                {
                    {spanishLang, "Una vez"},
                    {englishLang, "Once"}
                }
                },
                {"TypeConfiguration_Recurring", new Dictionary<string, string>()
                {
                    {spanishLang, "Recurrente"},
                    {englishLang, "Recurring"}
                }
                },
                {"OccursConfiguration_Daily", new Dictionary<string, string>()
                {
                    {spanishLang, "Diariamente"},
                    {englishLang, "Daily"}
                }
                },
                {"OccursConfiguration_Weekly", new Dictionary<string, string>()
                {
                    {spanishLang, "Semanalmente"},
                    {englishLang, "Weekly"}
                }
                },
                {"OccursConfiguration_Monthly", new Dictionary<string, string>()
                {
                    {spanishLang, "Mensualmente"},
                    {englishLang, "Monthly"}
                }
                },
                {"DailyFreqTime_Second", new Dictionary<string, string>()
                {
                    {spanishLang, "Segundos"},
                    {englishLang, "Second"}
                }
                },
                {"DailyFreqTime_Minutes", new Dictionary<string, string>()
                {
                    {spanishLang, "Minutos"},
                    {englishLang, "Minutes"}
                }
                },
                {"DailyFreqTime_Hours", new Dictionary<string, string>()
                {
                    {spanishLang, "Horas"},
                    {englishLang, "Hours"}
                }
                },
                {"MonthlyFrequency_First", new Dictionary<string, string>()
                {
                    {spanishLang, "Primero"},
                    {englishLang, "First"}
                }
                },
                {"MonthlyFrequency_Second", new Dictionary<string, string>()
                {
                    {spanishLang, "Segundo"},
                    {englishLang, "Second"}
                }
                },
                {"MonthlyFrequency_Third", new Dictionary<string, string>()
                {
                    {spanishLang, "Tercero"},
                    {englishLang, "Third"}
                }
                },
                {"MonthlyFrequency_Fourth", new Dictionary<string, string>()
                {
                    {spanishLang, "Cuarto"},
                    {englishLang, "Fourth"}
                }
                },
                {"MonthlyFrequency_Last", new Dictionary<string, string>()
                {
                    {spanishLang, "Último"},
                    {englishLang, "Last"}
                }
                },
                {"MonthlyDay_Monday", new Dictionary<string, string>()
                {
                    {spanishLang, "Lunes"},
                    {englishLang, "Monday"}
                }
                },
                {"MonthlyDay_Tuesday", new Dictionary<string, string>()
                {
                    {spanishLang, "Martes"},
                    {englishLang, "Tuesday"}
                }
                },
                {"MonthlyDay_Wednesday", new Dictionary<string, string>()
                {
                    {spanishLang, "Miércoles"},
                    {englishLang, "Wednesday"}
                }
                },
                {"MonthlyDay_Thursday", new Dictionary<string, string>()
                {
                    {spanishLang, "Jueves"},
                    {englishLang, "Thursday"}
                }
                },
                {"MonthlyDay_Friday", new Dictionary<string, string>()
                {
                    {spanishLang, "Viernes"},
                    {englishLang, "Friday"}
                }
                },
                {"MonthlyDay_Saturday", new Dictionary<string, string>()
                {
                    {spanishLang, "Sábado"},
                    {englishLang, "Saturday"}
                }
                },
                {"MonthlyDay_Sunday", new Dictionary<string, string>()
                {
                    {spanishLang, "Domingo"},
                    {englishLang, "Sunday"}
                }
                },
                {"MonthlyDay_Day", new Dictionary<string, string>()
                {
                    {spanishLang, "Día"},
                    {englishLang, "Day"}
                }
                },
                {"MonthlyDay_Weekday", new Dictionary<string, string>()
                {
                    {spanishLang, "Semana Laboral"},
                    {englishLang, "Weekday"}
                }
                },
                {"MonthlyDay_WeekendDay", new Dictionary<string, string>()
                {
                    {spanishLang, "Fin de Semana"},
                    {englishLang, "Weekend Day"}
                }
                },
            };
        }

        public static string GetTypeConfigurationDescription(TypeConfiguration? type)
        {
            if (type.HasValue)
            {
                return GetResourceLanguage("TypeConfiguration_" + type.ToString());
            }
            return string.Empty;
        }

        public static string GetDailyFreqTimeDescription(DailyFreqTime? type)
        {
            if (type.HasValue)
            {
                return GetResourceLanguage("DailyFreqTime_" + type.ToString());
            }
            return string.Empty;
        }

        public static string GetMonthlyFrequencyDescription(MonthlyFrequency? type)
        {
            if (type.HasValue)
            {
                return GetResourceLanguage("MonthlyFrequency_" + type.ToString());
            }
            return string.Empty;
        }
        public static string GetMonthlyDayDescription(MonthlyDay? type)
        {
            if (type.HasValue)
            {
                return GetResourceLanguage("MonthlyDay_" + type.ToString());
            }
            return string.Empty;
        }

    }
}

