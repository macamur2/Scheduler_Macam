using Scheduler.Domain;
using System.Collections.Generic;
using System.Globalization;

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
                throw new SchedulerException(GetResourceLanguage("ErrorCulture"));
            }

            if(dictionaryResources == null)
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
                    {spanishLang, "Ocurre el {0} {1} de cada {2} meses cada {3} {4} entre {5} y {6} a partir de {7}"},
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
                    {spanishLang, "Ocurre {0}, el Scheduler se utilizará el {1} a {2} a partir de {3}"},
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
            };
        }
	}
}

