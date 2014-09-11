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
    public class BackgroundTaskMonitorEvent
    {
        public IBackgroundTaskMonitor Monitor { set; get; }
        public BackgroundTaskMonitorEventType Type { set; get; }
    }
}
