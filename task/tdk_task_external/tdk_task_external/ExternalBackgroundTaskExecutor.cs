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
using Toyota.Common.Task;
using System.Diagnostics;
using System.Reflection;
using Toyota.Common.Logging;
using Toyota.Common.Database;
using System.Threading;

namespace Toyota.Common.Task.External
{
    public class ExternalBackgroundTaskExecutor: BackgroundTaskExecutorEventBroadcaster, 
                                                 IBackgroundTaskExecutor, 
                                                 IBackgroundTaskRuntimeListener
    {
        public ExternalBackgroundTaskExecutor()
        {
            maximumRunningTask = 10;
            runningTaskCounter = 0;
        }

        public LoggingManager LogManager { set; get; }
        public IEnumerable<Lazy<ExternalBackgroundTaskRuntime>> Runtimes { set; get; }
        private IDBContextManager DatabaseManager { set; get; }
        private string DatabaseContextName { set; get; }

        public ExternalBackgroundTaskExecutor(IDBContextManager dbManager, string contextName, LoggingManager logManager)
        {
            DatabaseManager = dbManager;
            DatabaseContextName = contextName;
            LogManager = logManager;
            RunningRuntimes = new List<BackgroundTaskRuntime>();
            SetMaximumRunningTask(10);
        }        
        private IList<BackgroundTaskRuntime> RunningRuntimes { set; get; }

        public BackgroundTaskRuntime Execute(BackgroundTask task)
        {
            BackgroundTaskRuntime runtime = null;
            if (runningTaskCounter < maximumRunningTask)
            {
                runtime = CreateRuntime(task);
                if (runtime != null)
                {
                    NotifyListeners(new BackgroundTaskExecutorEvent()
                    {
                        Executor = this,
                        Runtime = runtime,
                        Type = BackgroundTaskExecutorEventType.Runtime_Prepared
                    });

                    ThreadPool.QueueUserWorkItem(new WaitCallback(ExecuteTaskThread), new object[] { task, runtime });
                    runningTaskCounter++;
                }
            }            

            return runtime;
        }
        private BackgroundTaskRuntime CreateRuntime(BackgroundTask task)
        {
            if (Runtimes != null)
            {
                ExternalBackgroundTaskRuntime selectedRuntime = null;
                foreach (Lazy<ExternalBackgroundTaskRuntime> runtime in Runtimes)
                {
                    if (task.Name.Equals(runtime.Value.Name))
                    {
                        selectedRuntime = runtime.Value;
                        break;
                    }
                }

                if (selectedRuntime != null)
                {
                    ExternalBackgroundTaskRuntime runtime = (ExternalBackgroundTaskRuntime)Activator.CreateInstance(selectedRuntime.GetType());                    
                    runtime.Id = task.Id;
                    runtime.DatabaseManager = DatabaseManager;
                    runtime.LogManager = LogManager;
                    runtime.DefaultLogSession = LogManager.CreateSession(string.Format("{0}-{1}", runtime.Name, runtime.Id));
                    runtime.DefaultLogSession.EnableMultiSink = true;                                        
                    runtime.AddListener(this);
                    runtime.Parameters = task.Parameters;
                    return runtime;
                }
            }

            return null;
        }

        private void ExecuteTaskThread(object param)
        {            
            
            object[] parameters = (object[])param;
            ExternalBackgroundTask task = (ExternalBackgroundTask)parameters[0];
            ExternalBackgroundTaskRuntime runtime = (ExternalBackgroundTaskRuntime)parameters[1];
            if ((task != null) && (runtime != null))
            {
                try
                {
                    runtime.Execute(task.Parameters);
                }
                catch (Exception ex) {
                    runtime.DefaultLogSession.WriteLine(ex.Message, LoggingSeverity.Error);
                }                
            }
        }
                
        private int runningTaskCounter;
        public int  GetRunningTaskCount()
        {
            return runningTaskCounter;
        }

        private int maximumRunningTask;
        public void  SetMaximumRunningTask(int count)
        {
            maximumRunningTask = count;
        }
        public int  GetMaximumRunningTask()
        {
            return maximumRunningTask;
        }

        public void BackgroundTaskRuntimeChanged(BackgroundTaskRuntimeEvent evt)
        {
            BackgroundTaskRuntimeEventType eventType = evt.Type;
            TaskStatus status = evt.Runtime.GetStatus();
            if (eventType == BackgroundTaskRuntimeEventType.Status_Changed)
            {
                if ((status == TaskStatus.Finished) ||
                    (status == TaskStatus.Error) ||
                    (status == TaskStatus.Aborted) ||
                    (status == TaskStatus.Cancelled))
                {
                    runningTaskCounter--;
                    evt.Runtime.Dispose();

                    NotifyListeners(new BackgroundTaskExecutorEvent()
                    {
                        Executor = this,
                        Type = BackgroundTaskExecutorEventType.Runtime_Shutted_Down,
                        Runtime = evt.Runtime
                    });
                }

                if (status == TaskStatus.Active)
                {
                    NotifyListeners(new BackgroundTaskExecutorEvent()
                    {
                        Executor = this,
                        Type = BackgroundTaskExecutorEventType.Runtime_Activated,
                        Runtime = evt.Runtime
                    });
                }
            }
        }
    }
}
