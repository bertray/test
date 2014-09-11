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
    public abstract class LoggingSink
    {
        public LoggingSink(string name)
        {
            Name = name;
            IsDefault = false;
            Disabled = false;
            PrefetchIO = false;
        }

        public string Name { private set; get; }
        public bool IsDefault { set; get; }
        public bool Disabled { set; get; }
        public string SessionName { set; get; }

        public virtual bool PrefetchIO { set; get; }

        public void WriteLine(params LoggingMessage[] messages)
        {
            Write(messages);
            Write(LoggingMessage.EMPTY);
        }
        
        public abstract void Write(params LoggingMessage[] messages);
        public abstract void Close();

        public abstract IList<LoggingMessage> Pull();
        public abstract IList<LoggingMessage> Pull(int pageIndex, int pageSize); 
    }
}
