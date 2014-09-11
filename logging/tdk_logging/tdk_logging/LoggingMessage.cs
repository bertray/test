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
    public class LoggingMessage
    {
        public LoggingMessage() : this(LoggingSeverity.Info, string.Empty) { }
        public LoggingMessage(string message) : this(LoggingSeverity.Info, message, false) { }
        public LoggingMessage(string message, bool excludeHeadStamp): this(LoggingSeverity.Info, message, excludeHeadStamp){ }
        public LoggingMessage(LoggingSeverity severity, string message): this(severity, message, false) { }
        public LoggingMessage(LoggingSeverity severity, string message, bool excludeHeadStamp)
        {
            Severity = severity;
            Message = message;
            Date = DateTime.Now;
            ExcludeHeadStamp = excludeHeadStamp;
            Id = Guid.NewGuid().ToString();
        }

        public string Id { set; get; }
        public string Message { set; get; }
        public DateTime Date { set; get; }
        public LoggingSeverity Severity { set; get; }
        public bool ExcludeHeadStamp { set; get; }

        public static LoggingMessage EMPTY 
        {
            get
            {
                return new LoggingMessage() {
                    Message = "\n", 
                    Severity = LoggingSeverity.Info,
                    ExcludeHeadStamp = true
                };
            }
        }

        public string ToString(string sessionName)
        {
            string text = string.Empty;
            if (!string.IsNullOrEmpty(Message))
            {
                if (ExcludeHeadStamp)
                {
                    text = Message;
                }
                else
                {
                    if (!string.IsNullOrEmpty(sessionName))
                    {
                        text = string.Format("[{0}] [{1}] {2}", sessionName, Date.ToString("dd/MM/yyyy hh:mm:ss"), Message);
                    }
                    else
                    {
                        text = string.Format("[{0}] {1}", Date.ToString("ddd MMM yyyy, hh:mm:ss"), Message);
                    }
                }
            }
            return text;
        }

        public override string ToString()
        {
            return ToString(null);
        }
    }
}
