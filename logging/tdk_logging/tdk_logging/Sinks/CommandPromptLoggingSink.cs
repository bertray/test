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

namespace Toyota.Common.Logging.Sink
{
    public class CommandPromptLoggingSink: LoggingSink
    {        
        public CommandPromptLoggingSink(string name): base(name) { }

        public override void Write(params LoggingMessage[] messages)
        {
            if ((messages != null) && (messages.Length > 0))
            {
                ConsoleColor severityColor = ConsoleColor.Green;
                ConsoleColor messageColor = ConsoleColor.Gray;
                LoggingMessage emptyMessage = LoggingMessage.EMPTY;
                foreach (LoggingMessage msg in messages)
                {
                    if (emptyMessage.Message.Equals(msg.Message))
                    {
                        Console.WriteLine();
                        continue;
                    }

                    if (msg.Severity == LoggingSeverity.Error)
                    {
                        severityColor = ConsoleColor.Red;
                        messageColor = ConsoleColor.Red;
                    }
                    else if (msg.Severity == LoggingSeverity.Fatal)
                    {
                        severityColor = ConsoleColor.Red;
                        messageColor = ConsoleColor.Red;
                    }
                    else if (msg.Severity == LoggingSeverity.Warning)
                    {
                        severityColor = ConsoleColor.Yellow;
                        messageColor = ConsoleColor.Yellow;
                    }

                    Console.ForegroundColor = severityColor;
                    Console.Write("[" + Convert.ToString(msg.Severity) + "] ");
                    Console.ForegroundColor = messageColor;
                    Console.Write(msg.Message);
                    Console.ResetColor();                    
                }
            }
        }
        public override void Close() { }

        public override IList<LoggingMessage> Pull()
        {
            throw new NotImplementedException();
        }
        public override IList<LoggingMessage> Pull(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
