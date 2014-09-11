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
using System.IO;

namespace Toyota.Common.Logging.Sink
{
    public class TextFileLoggingSink: LoggingSink
    {
        private string rootPath;
        private StreamWriter writer;
                
        public TextFileLoggingSink(string name, string rootPath): base(name)
        {
            this.rootPath = rootPath;            
        }

        public override bool PrefetchIO
        {
            get
            {
                return base.PrefetchIO;
            }
            set
            {
                base.PrefetchIO = value;
                if (base.PrefetchIO)
                {
                    writer = _CreateWriter();
                }
                else
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                }
            }
        }

        public override void Write(params LoggingMessage[] messages)
        {
            if (messages == null)
            {
                return;
            }

            if (!PrefetchIO)
            {
                writer = _CreateWriter();
            }            

            LoggingMessage emptyMessage = LoggingMessage.EMPTY;
            DateTime today = DateTime.Now;
            TimeSpan time = today.TimeOfDay;           
            
            if (writer == null)
            {
                return;
            }

            foreach (LoggingMessage message in messages)
            {                                
                if (emptyMessage.Message.Equals(message.Message))
                {
                    writer.WriteLine();
                }
                else
                {
                    if (message.ExcludeHeadStamp)
                    {
                        writer.Write(message.Message != null ? message.Message : string.Empty);
                    }
                    else
                    {
                        writer.Write(
                            string.Format("[{0} | {1}] {2}", today.ToString("dd MMM yyyy, HH:mm:ss"),
                            Convert.ToString(message.Severity).ToUpper(),
                            message.Message != null ? message.Message : string.Empty
                        ));
                    }                    
                }                
            }

            writer.Flush();
            if (!PrefetchIO)
            {
                writer.Close();
            }
        }

        private StreamWriter _CreateWriter()
        {                        
            string currentFileName = string.Format("{0}_{1}.log", SessionName, DateTime.Now.ToString("dd-MMM-yyy"));
            if (!rootPath.EndsWith("\\"))
            {
                rootPath += "\\";
            }
            string filePath = rootPath + currentFileName;
            if (!File.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            StreamWriter writer;
            if (!File.Exists(filePath))
            {
                writer = File.CreateText(filePath);
            }
            else
            {
                FileStream fileStream = File.Open(filePath, FileMode.Append, FileAccess.Write);
                writer = new StreamWriter(fileStream);
            }
            return writer;
        }

        public override void Close()
        {
            if (writer != null)
            {
                writer.Close();
            }
        }

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
