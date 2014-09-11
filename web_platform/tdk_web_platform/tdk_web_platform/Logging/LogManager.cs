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
using Toyota.Common.Logging.Sink;

namespace Toyota.Common.Web.Platform
{
    public class LogManager: LoggingManager
    {
        private LogManager() 
        {
            LogSettings settings = ApplicationSettings.Instance.Logging;
            if (settings.Enabled)
            {
                AddSink(settings.InternalSinkName, typeof(TextFileLoggingSink), true, settings.FolderLocation);
            }            
        }

        private static LogManager instance = new LogManager();
        public static LogManager Instance
        {
            get { return instance; }
        }

        public LoggingSession SystemSession 
        {
            get
            {
                return CreateSession("System", true);
            }
        }
    }
}
