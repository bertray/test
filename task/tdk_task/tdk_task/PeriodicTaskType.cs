///
/// <author>lufty.abdillah@gmail.com</author>
/// <summary>
/// Toyota .Net Development Kit
/// Copyright (c) Toyota Motor Manufacturing Indonesia, All Right Reserved.
/// </summary>
/// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toyota.Common.Task
{
    public enum PeriodicTaskType
    {
        DAILY,
        WEEKLY,
        MONTHLY,
        YEARLY,
        /// <summary>
        /// Fallback type.
        /// All Periodic Task spesified as UNDEFINED will be treated as Immediate Task.
        /// </summary>
        UNDEFINED 
    }
}
