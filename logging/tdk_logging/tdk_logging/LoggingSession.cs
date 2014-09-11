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
using Toyota.Common.Utilities;

namespace Toyota.Common.Logging
{
    public class LoggingSession: IDisposable
    {
        private Queue<LoggingMessage> messageQueue;
        private List<LoggingSink> sinks;
        private LoggingSink defaultSink;

        private List<ILoggingSessionEventListener> listeners;

        public LoggingSession(string name, params LoggingSink[] sinks)
        {
            listeners = new List<ILoggingSessionEventListener>();
            messageQueue = new Queue<LoggingMessage>();
            AutoFlush = true;
            Name = name;
            EnableMultiSink = false;

            this.sinks = new List<LoggingSink>();
            if ((sinks != null) && (sinks.Length > 0))
            {
                foreach (LoggingSink logSink in sinks)
                {
                    this.sinks.Add(logSink);
                    if (logSink.IsDefault)
                    {
                        defaultSink = logSink;
                    }
                }

                if (defaultSink == null)
                {
                    defaultSink = this.sinks.First();
                }
            }            
        }

        public string Name { private set; get; }
        public bool AutoFlush { set; get; }
        public bool EnableMultiSink { set; get; }
        public bool HasSink
        {
            get {
                return (sinks != null) && (sinks.Count > 0);
            }
            
        }
        public IList<LoggingSink> Sinks
        {
            get
            {
                if (sinks != null)
                {
                    return sinks.AsReadOnly();
                }
                return null;
            }
        }
        public LoggingSink GetDefaultSink()
        {
            if (!sinks.IsNullOrEmpty())
            {
                foreach (LoggingSink sink in sinks)
                {
                    if (sink.IsDefault)
                    {
                        return sink;
                    }
                }
            }
            return null;
        }

        public void SetPrefetchIO(bool prefetchIO)
        {
            foreach (LoggingSink sk in sinks)
            {
                sk.PrefetchIO = prefetchIO;
            }
        }

        public virtual void WriteLine(params LoggingMessage[] messages) 
        {
            Write(messages);            
            Write(LoggingMessage.EMPTY);
        }
        public virtual void WriteLine(string message, LoggingSeverity severity)
        {
            WriteLine(new LoggingMessage() { Message = message, Severity = severity });
        }
        public virtual void WriteLine(string message)
        {
            WriteLine(message, LoggingSeverity.Info);
        }        
        public virtual void Write(string message, LoggingSeverity severity)
        {
            Write(new LoggingMessage() { Message = message, Severity = severity });
        }
        public virtual void Write(string message)
        {
            Write(message, LoggingSeverity.Info);
        }
        public virtual void Write(params LoggingMessage[] messages)
        {
            if ((messages == null) || (messages.Length == 0))
            {
                return;
            }

            if (AutoFlush)
            {
                bool messagePersisted = false;
                if (EnableMultiSink)
                {
                    foreach (LoggingSink sink in sinks)
                    {
                        if (!sink.Disabled)
                        {
                            sink.Write(messages);
                            messagePersisted = true;
                        }
                    }
                }
                else if (defaultSink != null)
                {
                    if (!defaultSink.Disabled)
                    {
                        defaultSink.Write(messages);
                        messagePersisted = true;
                    }
                }

                if (messagePersisted)
                {
                    NotifyEventListeners(new LoggingSessionEvent()
                    {
                        Messages = messages,
                        Session = this,
                        Type = LoggingSessionEventType.Messages_Persisted
                    });
                }
            }
            else
            {
                foreach (LoggingMessage msg in messages)
                {
                    messageQueue.Enqueue(msg);
                }
            }

            NotifyEventListeners(new LoggingSessionEvent()
            {
                Messages = messages,
                Session = this,
                Type = LoggingSessionEventType.Messages_Written
            });
        }

        public void AddEventListener(ILoggingSessionEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }
        public void RemoveEventListener(ILoggingSessionEventListener listener)
        {
            listeners.Remove(listener);
        }
        public IList<ILoggingSessionEventListener> GetEventListeners()
        {
            if (listeners.Count > 0)
            {
                return listeners.AsReadOnly();
            }
            return null;
        }
        protected void NotifyEventListeners(LoggingSessionEvent evt)
        {            
            IList<ILoggingSessionEventListener> _listeners;
            lock (listeners)
            {
                _listeners = listeners.AsReadOnly();
            }
            if (_listeners != null)
            {
                foreach (ILoggingSessionEventListener listener in _listeners)
                {
                    listener.LoggingSessionChanged(evt);
                }
            }
        }

        public virtual IList<LoggingMessage> Pull()
        {
            LoggingSink sink = GetDefaultSink();
            if (!sink.IsNull())
            {
                return sink.Pull();
            }
            return null;
        }
        public virtual IList<LoggingMessage> Pull(int pageIndex, int pageSize)
        {
            LoggingSink sink = GetDefaultSink();
            if (!sink.IsNull())
            {
                return sink.Pull(pageIndex, pageSize);
            }
            return null;
        }

        public void Flush()
        {
            if (sinks != null)
            {
                if (messageQueue.Count > 0)
                {
                    LoggingMessage logMessage;
                    IList<LoggingMessage> writtenMessages = new List<LoggingMessage>();
                    while (messageQueue.Count > 0)
                    {
                        logMessage = messageQueue.Dequeue();
                        writtenMessages.Add(logMessage);
                        foreach (LoggingSink sink in sinks)
                        {
                            sink.Write(logMessage);
                        }
                    }

                    if (writtenMessages.Count > 0)
                    {
                        NotifyEventListeners(new LoggingSessionEvent()
                        {
                            Messages = writtenMessages.ToArray(),
                            Session = this,
                            Type = LoggingSessionEventType.Messages_Persisted
                        });    
                    }                    
                }
            }
        }
        public void Close()
        {
            Flush();
            if (sinks != null)
            {
                foreach (LoggingSink sink in sinks)
                {
                    sink.Close();
                }
            }            
        }

        public void Dispose()
        {
            listeners.Clear();
        }
    }
}
