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
    public class LoggingManager
    {
        private IDictionary<string, LoggingSinkType> sinks;

        public LoggingManager()
        {
            sinks = new Dictionary<string, LoggingSinkType>();
        }

        public virtual LoggingSession CreateSession(string name)
        {
            return CreateSession(name, false);
        }
        public virtual LoggingSession CreateSession(string name, bool enableMultisink)
        {
            return CreateSession(name, null, enableMultisink);
        }
        public virtual LoggingSession CreateSession(string name, string sinkName, bool enableMultisink) 
        {
            return CreateSession(name, sinkName, enableMultisink, true);        
        }
        public virtual LoggingSession CreateSession(string name, string sinkName, bool enableMultisink, bool autoflush)
        {
            List<LoggingSinkType> _sinkTypes = new List<LoggingSinkType>();
            List<LoggingSink> sessionSinks = new List<LoggingSink>();
            if (!string.IsNullOrEmpty(sinkName) && sinks.ContainsKey(sinkName))
            {
                _sinkTypes.Add(sinks[sinkName]);                
            }
            else
            {
                _sinkTypes.AddRange(sinks.Values);
            }

            List<object> _arguments;
            LoggingSink _sink;
            foreach (LoggingSinkType _sk in _sinkTypes)
            {
                _arguments = new List<object>();
                _arguments.Add(name);
                if ((_sk.Arguments != null) && (_sk.Arguments.Length > 0))
                {
                    _arguments.AddRange(_sk.Arguments);
                }
                _sink = (LoggingSink)Activator.CreateInstance(_sk.Type, _arguments.ToArray());
                _sink.IsDefault = _sk.IsDefault;
                _sink.SessionName = name;
                sessionSinks.Add(_sink);
            }            

            return CreateSession(name, sessionSinks.ToArray(), enableMultisink, autoflush);
        }
        public virtual LoggingSession CreateSession(string name, LoggingSink[] sinks, bool enableMultisink, bool autoflush)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            else
            {
                return new LoggingSession(name, sinks) { EnableMultiSink = enableMultisink, AutoFlush = autoflush };
            }            
        }
                
        public void AddSink(string name, Type type, params object[] args)
        {
            AddSink(name, type, false, args);
        }
        public void AddSink(string name, Type type, bool isDefault, params object[] args)
        {
            if (type != null)
            {                
                LoggingSinkType sinkType = new LoggingSinkType() {
                    Name = name,
                    IsDefault = isDefault,
                    Type = type,
                    Arguments = args
                };

                if (sinks.ContainsKey(name))
                {
                    sinks[name] = sinkType;
                }
                else
                {
                    sinks.Add(name, sinkType);
                }
            }            
        }
        public void RemoveSink(string name)
        {
            if (!string.IsNullOrEmpty(name) && sinks.ContainsKey(name))
            {
                sinks.Remove(name);
            }
        }
        public void RemoveAllSink()
        {
            sinks.Clear();
        }
        public void Close()
        {
            sinks.Clear();
        }
    }
}
