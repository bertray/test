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
using System.Threading;
using Toyota.Common.Logging;
using Toyota.Common.Logging.Sink;

namespace Toyota.Common.Task
{
    public class BackgroundTaskManager: IDisposable
    {
        public IBackgroundTaskRegistry Registry { protected set; get; }
        public IBackgroundTaskQueue Queue { protected set; get; }
        public IBackgroundTaskHistory History { protected set; get; }
        public IBackgroundTaskExecutor Executor { protected set; get; }
        public IBackgroundTaskMonitor Monitor { protected set; get; }

        private const string LOG_FOLDER = "Logs\\Manager";
        public LoggingManager LogManager { private set; get; }

        public BackgroundTaskManager(IBackgroundTaskRegistry registry, IBackgroundTaskQueue queue, IBackgroundTaskHistory history, IBackgroundTaskExecutor executor, IBackgroundTaskMonitor monitor)
        {
            Registry = registry;
            Queue = queue;
            History = history;
            Executor = executor;
            Monitor = monitor;

            LogManager = new LoggingManager();            
            LogManager.AddSink("TextFile", typeof(TextFileLoggingSink), true);

            SuspendPeriod = 15000;
        }
        
        public int SuspendPeriod { set; get; }
        private bool KeepAlive { set; get; }
        public virtual void Start(bool blocking)
        {
            Monitor.Start();
            if (blocking)
            {
                KeepAlive = true;
                while (KeepAlive)
                {
                    Thread.Sleep(SuspendPeriod);
                }                
            }            
        }
        public virtual void Stop()
        {
            Monitor.Stop();
            KeepAlive = false;
        }

        public virtual void Dispose()
        {
            if (Monitor.IsMonitoring())
            {
                Monitor.Stop();
            }
            Monitor.Dispose();

            Registry.Dispose();
            Queue.Dispose();
            History.Dispose();
        }                
        
    }
}
