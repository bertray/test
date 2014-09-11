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
    public class PeriodicBackgroundTask : DelayedBackgroundTask        
    {
        public DateTime? StartDate { set; get; }
        public DateTime? EndDate { set; get; }
        public PeriodicTaskType PeriodicType 
        {
            get
            {
                if (Range != null)
                {
                    if (Range is DailyTaskRange)
                    {
                        return PeriodicTaskType.DAILY;
                    }
                    else if (Range is WeeklyTaskRange)
                    {
                        return PeriodicTaskType.WEEKLY;
                    }
                    else if (Range is MonthlyTaskRange)
                    {
                        return PeriodicTaskType.MONTHLY;
                    }
                    else if (Range is YearlyTaskRange)
                    {
                        return PeriodicTaskType.YEARLY;
                    }
                }
                return PeriodicTaskType.UNDEFINED;
            }
        }
    }
}
