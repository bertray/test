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

namespace Toyota.Common.Logging
{
    public class LoggingSessionActionListener: ILoggingSessionEventListener
    {
        private Action<LoggingSessionEvent> action;

        public LoggingSessionActionListener(Action<LoggingSessionEvent> action)
        {
            this.action = action;
        }

        public void LoggingSessionChanged(LoggingSessionEvent evt)
        {
            if (action != null)
            {
                action.Invoke(evt);
            }
        }
    }
}
