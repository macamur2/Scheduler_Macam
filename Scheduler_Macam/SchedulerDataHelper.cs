﻿using System;
using Scheduler.Domain.Resources;

namespace Scheduler.Domain
{
    public class SchedulerDataHelper
    {
        #region Enumerations
        public enum TypeConfiguration
        {
            Once = 01,
            Recurring = 02
        }

        public enum OccursConfiguration
        {
            Daily = 01,
            Weekly = 02,
            Monthly = 03
        }
        #endregion
    }

}