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
using Toyota.Common.Task;
using Toyota.Common.Task.External;

namespace Toyota.Common.Task.External
{
    class ExternalBackgroundTaskModel: ExternalBackgroundTask
    {        
        public string SubmitterUsername { set; get; }
        public string Parameter { set; get; }
        public byte? Interval { set; get; }
        public byte? PeriodicTypeValue { set; get; }

        public string ExecutionDays { set; get; }
        public string ExecutionMonths { set; get; }
        public int? ExecutionCount { set; get; }

        public long? RangeTimeInTicks { set; get; }
        public long? StartDateInTicks { set; get; }
        public long? EndDateInTicks { set; get; }

        public DateTime? ExecutionStartTime { set; get; }
        public DateTime? ExecutionFinishTime { set; get; }
    }
}
