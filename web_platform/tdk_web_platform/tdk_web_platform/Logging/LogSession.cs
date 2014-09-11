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
using Toyota.Common.Logging;

namespace Toyota.Common.Web.Platform
{
    public class LogSession: LoggingSession
    {
        public LogSession(string name, params LoggingSink[] sinks) : base(name, sinks) { }

        public override void Write(params LoggingMessage[] messages)
        {
            if (ApplicationSettings.Instance.Logging.Enabled)
            {
                base.Write(messages);
            }            
        }

        public override void WriteLine(params LoggingMessage[] messages)
        {
            if (ApplicationSettings.Instance.Logging.Enabled)
            {
                base.WriteLine(messages);
            }               
        }
    }
}
