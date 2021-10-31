﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Scheduler.Domain.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Global {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Global() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Scheduler.Domain.Resources.Global", typeof(Global).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Occurs {0}, Schedule will be used on {1} at {2} starting on {3}.
        /// </summary>
        internal static string Description_SchedulerNextExecution {
            get {
                return ResourceManager.GetString("Description_SchedulerNextExecution", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The fields {0} and {1} cannot be activated at same time, you must select only one to operate. Please check the configuration and try again..
        /// </summary>
        internal static string Error_DailyInputBoth {
            get {
                return ResourceManager.GetString("Error_DailyInputBoth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field {0} cannot be greater than {1}. Please check the configuration and try again..
        /// </summary>
        internal static string Error_DateTimeGreater {
            get {
                return ResourceManager.GetString("Error_DateTimeGreater", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field {0} is empty and is neccesary to operate with the selected configuration. Please check it and try again..
        /// </summary>
        internal static string Error_EmptyInput {
            get {
                return ResourceManager.GetString("Error_EmptyInput", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The fields {0} and {1} are empty, you must select only one to operate. Please check the configuration and try again..
        /// </summary>
        internal static string Error_EmptyInputBoth {
            get {
                return ResourceManager.GetString("Error_EmptyInputBoth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The {0} field has an incorrect value, check the configuration and try again..
        /// </summary>
        internal static string Error_IncorrectValue {
            get {
                return ResourceManager.GetString("Error_IncorrectValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is a problem calculating the Next Execution Time, please try again later..
        /// </summary>
        internal static string Error_OutputDateTimeNull {
            get {
                return ResourceManager.GetString("Error_OutputDateTimeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Scheduler can not be null. Ensure that the info is correct..
        /// </summary>
        internal static string Error_SchedulerNull {
            get {
                return ResourceManager.GetString("Error_SchedulerNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Once.
        /// </summary>
        internal static string TypeConfiguration_Once {
            get {
                return ResourceManager.GetString("TypeConfiguration_Once", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Every day.
        /// </summary>
        internal static string TypeConfiguration_Recurring {
            get {
                return ResourceManager.GetString("TypeConfiguration_Recurring", resourceCulture);
            }
        }
    }
}
