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
using System.Data.SqlClient;

namespace Toyota.Common.Task.External
{
    public class ExternalBackgroundTaskMonitor: BackgroundTaskMonitorEventBroadcaster, IBackgroundTaskMonitor
    {
        private const int WEEKLY_DAY_RANGE = 7;
        public const int DEFAULT_WORKING_INTERVAL = 15000;

        private bool keepMonitor;
        public int WorkingInterval { set; get; }
        private int _WorkingInterval { set; get; }

        private ExternalBackgroundTaskRegistry registry;
        private ExternalBackgroundTaskQueue queue;
        private ExternalBackgroundTaskHistory history;
        private ExternalBackgroundTaskExecutor executor;

        private Thread thread;

        public ExternalBackgroundTaskMonitor(ExternalBackgroundTaskRegistry registry, ExternalBackgroundTaskQueue queue, ExternalBackgroundTaskHistory history, ExternalBackgroundTaskExecutor executor)
        {
            this.registry = registry;
            this.queue = queue;
            this.history = history;
            this.executor = executor;

            this.keepMonitor = false;
            this.WorkingInterval = DEFAULT_WORKING_INTERVAL;            
        }

        public LoggingManager LogManager { set; get; }
        public LoggingSession LogSession { private set; get; }

        public void Start()
        {
            LogSession = LogManager.CreateSession("Monitor");
            LogSession.AutoFlush = true;
            LogSession.EnableMultiSink = true;
            LogSession.Write(new LoggingMessage("Starting monitor, please wait ... "));

            thread = new Thread(new ParameterizedThreadStart(ExecuteMonitor));
            thread.IsBackground = true;
            thread.Start();
        }
        public void Stop()
        {
            if (LogSession != null)
            {
                LogSession.Write(new LoggingMessage("Stopping monitor, please wait ... "));
            }
            thread.Abort();
            _WorkingInterval = 100;
        }
        public bool IsMonitoring()
        {
            return keepMonitor || (thread != null);
        }

        private void ExecuteMonitor(object param)
        {
            bool autoRestart = false;

            try
            {
                keepMonitor = true;
                TimeSpan timeSpan;
                bool found;
                _WorkingInterval = WorkingInterval;

                ExternalBackgroundTask task;
                IList<BackgroundTask> taskList;

                IList<BackgroundTask> executingTasks;
                BackgroundTask executedTask;
                IList<BackgroundTask> executedTasks;
                List<BackgroundTask> taskHolder = new List<BackgroundTask>();

                DailyTaskRange dailyRange;
                WeeklyTaskRange weeklyRange;
                MonthlyTaskRange monthlyRange;
                YearlyTaskRange yearlyRange;

                DateTime currentDate;
                DateTime tempDate;

                bool matchedDay;
                int runningDays;
                int interval;
                byte daysInMonth;                

                LogSession.WriteLine(new LoggingMessage("Monitor started.", true));
                NotifyListeners(new BackgroundTaskMonitorEvent()
                {
                    Monitor = this,
                    Type = BackgroundTaskMonitorEventType.Started
                });

                while (keepMonitor)
                {
                    Thread.Sleep(_WorkingInterval);
                    if (!keepMonitor)
                    {
                        break;
                    }

                    currentDate = DateTime.Now;
                    TimeSpan currentTimeOfDay = currentDate.TimeOfDay;
                    DayOfWeek currentDayOfWeek = currentDate.DayOfWeek;

                    taskHolder.Clear();
                    taskList = registry.GetByType(TaskType.Immediate);
                    if (taskList != null)
                    {
                        taskHolder.AddRange(taskList);
                    }
                    taskList = registry.GetByType(TaskType.Delayed);
                    if (taskList != null)
                    {
                        foreach (BackgroundTask t in taskList)
                        {
                            task = (ExternalBackgroundTask)t;
                            if (task.Range != null)
                            {
                                if (TimeSpan.Compare(task.Range.Time, currentTimeOfDay) <= 0)
                                {
                                    taskHolder.Add(t);
                                }
                            }
                        }
                    }
                    taskList = registry.GetByType(TaskType.Periodic);
                    if (taskList != null)
                    {
                        foreach (BackgroundTask t in taskList)
                        {
                            task = (ExternalBackgroundTask)t;
                            if (task.PeriodicType == PeriodicTaskType.UNDEFINED)
                            {
                                taskHolder.Add(t);
                            }
                            else
                            {
                                if (!task.StartDate.HasValue)
                                {
                                    continue;
                                }

                                if (DateTime.Compare(currentDate, task.StartDate.Value) < 0)
                                {
                                    continue;
                                }

                                if (task.EndDate.HasValue)
                                {
                                    if (DateTime.Compare(currentDate, task.EndDate.Value) > 0)
                                    {
                                        registry.RemoveById(task.Id);
                                        continue;
                                    }
                                }

                                if (task.Range != null)
                                {
                                    executingTasks = queue.GetByRegistryId(task.Id);
                                    if ((executingTasks == null) || (executingTasks.Count == 0))
                                    {
                                        matchedDay = false;
                                        #region Daily
                                        if (task.PeriodicType == PeriodicTaskType.DAILY)
                                        {
                                            dailyRange = (DailyTaskRange)task.Range;
                                            executedTasks = history.SearchByRegistryId(task.Id);

                                            if (dailyRange.Days != null)
                                            {
                                                foreach (DayOfWeek d in dailyRange.Days)
                                                {
                                                    if (d == currentDayOfWeek)
                                                    {
                                                        if ((executedTasks != null) && (executedTasks.Count > 0))
                                                        {
                                                            executedTask = (ExternalBackgroundTask)executedTasks[0];
                                                            if ((executedTask.ExecutionDescriptor != null)
                                                                && (currentDate.Subtract(executedTask.ExecutionDescriptor.StartTime.Value).Days >= 1))
                                                            {
                                                                matchedDay = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            matchedDay = true;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                            else if (dailyRange.Interval.HasValue && task.StartDate.HasValue)
                                            {
                                                if ((executedTasks != null) && (executedTasks.Count > 0))
                                                {
                                                    executedTask = (ExternalBackgroundTask)executedTasks[0];
                                                    if (executedTask.ExecutionDescriptor.StartTime.HasValue)
                                                    {
                                                        tempDate = executedTask.ExecutionDescriptor.StartTime.Value;
                                                        matchedDay = currentDate.Subtract(tempDate).Days >= dailyRange.Interval.Value;
                                                    }
                                                }
                                                else
                                                {
                                                    matchedDay = true;
                                                }
                                            }

                                            if (matchedDay)
                                            {
                                                if (dailyRange.Time == null)
                                                {
                                                    matchedDay = true;
                                                }
                                                else
                                                {
                                                    matchedDay = TimeSpan.Compare(dailyRange.Time, currentTimeOfDay) <= 0;
                                                }
                                            }

                                            if (matchedDay)
                                            {
                                                taskHolder.Add(t);
                                            }
                                        }
                                        #endregion Daily
                                        else
                                        {
                                            #region Weekly
                                            if (task.PeriodicType == PeriodicTaskType.WEEKLY)
                                            {
                                                weeklyRange = (WeeklyTaskRange)task.Range;
                                                if ((weeklyRange.Days == null) || (weeklyRange.Days.Length == 0))
                                                {
                                                    weeklyRange.Days = new DayOfWeek[] { DayOfWeek.Monday };
                                                }

                                                timeSpan = currentDate.Subtract(task.StartDate.Value);
                                                interval = weeklyRange.Interval.HasValue ? weeklyRange.Interval.Value : 1;
                                                runningDays = timeSpan.Days % (WEEKLY_DAY_RANGE * interval);

                                                tempDate = currentDate.AddDays(runningDays * -1);
                                                executedTasks = history.SearchByRegistryIdAndStartTimeRange(
                                                    task.Id,
                                                    new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 0, 0, 0),
                                                    new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 1, 0, 0, 0)
                                                );
                                                if ((executedTasks != null) && (executedTasks.Count > 0))
                                                {
                                                    executedTask = executedTasks[0];
                                                    if (executedTask.ExecutionDescriptor != null)
                                                    {
                                                        tempDate = executedTask.ExecutionDescriptor.StartTime.Value;
                                                        if ((tempDate.DayOfWeek == currentDayOfWeek) && (currentDate.Subtract(tempDate).Days < (WEEKLY_DAY_RANGE * interval)))
                                                        {
                                                            continue;
                                                        }
                                                    }
                                                }

                                                foreach (DayOfWeek d in weeklyRange.Days)
                                                {
                                                    if (d == currentDayOfWeek)
                                                    {
                                                        if ((executedTasks != null) && (executedTasks.Count > 0))
                                                        {
                                                            found = false;
                                                            foreach (BackgroundTask execTask in executedTasks)
                                                            {
                                                                if (execTask.ExecutionDescriptor != null)
                                                                {
                                                                    found = (execTask.ExecutionDescriptor.StartTime.Value.DayOfWeek == d);
                                                                }
                                                            }
                                                            matchedDay = !found;
                                                        }
                                                        else
                                                        {
                                                            matchedDay = true;
                                                        }
                                                    }
                                                }

                                                if (matchedDay)
                                                {
                                                    if (weeklyRange.Time == null)
                                                    {
                                                        matchedDay = true;
                                                    }
                                                    else
                                                    {
                                                        matchedDay = (TimeSpan.Compare(currentTimeOfDay, weeklyRange.Time) >= 0);
                                                    }
                                                }

                                                if (matchedDay)
                                                {
                                                    taskHolder.Add(t);
                                                }
                                            }
                                            #endregion Weekly
                                            #region Monthly
                                            else if (task.PeriodicType == PeriodicTaskType.MONTHLY)
                                            {
                                                monthlyRange = (MonthlyTaskRange)task.Range;
                                                interval = monthlyRange.Interval.HasValue ? monthlyRange.Interval.Value : 1;
                                                executedTasks = history.SearchByRegistryId(task.Id);
                                                if ((executedTasks != null) && (executedTasks.Count > 0))
                                                {
                                                    executedTask = executedTasks[0];
                                                    tempDate = executedTask.ExecutionDescriptor.StartTime.Value;
                                                    if ((currentDate.Year == tempDate.Year) && ((currentDate.Month - tempDate.Month) == 0))
                                                    {
                                                        continue;
                                                    }

                                                    if ((currentDate.Month - tempDate.Month) > 0)
                                                    {
                                                        if (!monthlyRange.Day.HasValue)
                                                        {
                                                            monthlyRange.Day = 1;
                                                        }

                                                        daysInMonth = (byte)DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                                                        if (monthlyRange.Day.Value > daysInMonth)
                                                        {
                                                            monthlyRange.Day = daysInMonth;
                                                        }

                                                        matchedDay = (currentDate.Day == monthlyRange.Day.Value);
                                                    }
                                                }
                                                else
                                                {
                                                    matchedDay = true;
                                                }

                                                if (matchedDay)
                                                {
                                                    if (monthlyRange.Time == null)
                                                    {
                                                        matchedDay = true;
                                                    }
                                                    else
                                                    {
                                                        matchedDay = (TimeSpan.Compare(currentTimeOfDay, monthlyRange.Time) >= 0);
                                                    }
                                                }

                                                if (matchedDay)
                                                {
                                                    taskHolder.Add(t);
                                                }
                                            }
                                            #endregion Monthly
                                            #region Yearly
                                            else if (task.PeriodicType == PeriodicTaskType.YEARLY)
                                            {
                                                yearlyRange = (YearlyTaskRange)task.Range;
                                                interval = yearlyRange.Interval.HasValue ? yearlyRange.Interval.Value : 1;
                                                if (!yearlyRange.Month.HasValue)
                                                {
                                                    yearlyRange.Month = 1;
                                                }
                                                if (!yearlyRange.Day.HasValue)
                                                {
                                                    yearlyRange.Day = 1;
                                                }
                                                executedTasks = history.SearchByRegistryId(task.Id);
                                                if ((executedTasks != null) && (executedTasks.Count > 0))
                                                {
                                                    executedTask = executedTasks[0];
                                                    tempDate = executedTask.ExecutionDescriptor.StartTime.Value;
                                                    if ((currentDate.Year - tempDate.Year) == 0)
                                                    {
                                                        continue;
                                                    }

                                                    matchedDay = ((currentDate.Year - tempDate.Year) > 0) && (currentDate.Month == yearlyRange.Month);
                                                }
                                                else
                                                {
                                                    matchedDay = true;
                                                }

                                                if (matchedDay)
                                                {
                                                    if (yearlyRange.Time == null)
                                                    {
                                                        matchedDay = true;
                                                    }
                                                    else
                                                    {
                                                        matchedDay = (TimeSpan.Compare(currentTimeOfDay, yearlyRange.Time) >= 0);
                                                    }
                                                }

                                                if (matchedDay)
                                                {
                                                    taskHolder.Add(t);
                                                }
                                            }
                                            #endregion Yearly
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (BackgroundTask t in taskHolder)
                    {
                        queue.Register(t);
                        if ((t.Type == TaskType.Delayed) || (t.Type == TaskType.Immediate))
                        {
                            registry.RemoveById(t.Id);
                        }
                    }
                    NotifyListeners(new BackgroundTaskMonitorEvent()
                    {
                        Monitor = this,
                        Type = BackgroundTaskMonitorEventType.Registry_Sweeped
                    });

                    taskHolder.Clear();
                    taskList = queue.GetByStatus(TaskStatus.Cancelled);
                    if (taskList != null)
                    {
                        taskHolder.AddRange(taskList);
                    }
                    taskList = queue.GetByStatus(TaskStatus.Error);
                    if (taskList != null)
                    {
                        taskHolder.AddRange(taskList);
                    }
                    taskList = queue.GetByStatus(TaskStatus.Finished);
                    if (taskList != null)
                    {
                        taskHolder.AddRange(taskList);
                    }
                    foreach (BackgroundTask t in taskHolder)
                    {
                        history.Register(t);
                        queue.RemoveById(t.Id);
                    }

                    taskList = queue.GetByStatus(TaskStatus.Released);
                    if (taskList != null)
                    {
                        if (LogSession != null)
                        {
                            LogSession.WriteLine(new LoggingMessage(string.Format("Found {0} released tasks, initiate execution ...", taskList.Count)));
                        }

                        IEnumerable<BackgroundTask> executionTasks = null;
                        int executionNumberFactor = (executor.GetMaximumRunningTask() - executor.GetRunningTaskCount());
                        if (taskList.Count > executionNumberFactor)
                        {
                            if (executionNumberFactor == 0)
                            {
                                executionNumberFactor = executor.GetMaximumRunningTask();
                            }
                            executionTasks = taskList.Take<BackgroundTask>(executionNumberFactor);
                        }
                        else
                        {
                            executionTasks = taskList.AsEnumerable<BackgroundTask>();
                        }

                        foreach (BackgroundTask t in executionTasks)
                        {
                            if (t.Progress == null)
                            {
                                task = (ExternalBackgroundTask)t;
                                executor.Execute(task);
                            }
                        }
                    }
                    NotifyListeners(new BackgroundTaskMonitorEvent()
                    {
                        Monitor = this,
                        Type = BackgroundTaskMonitorEventType.Queue_Sweeped
                    });
                }
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                {                    
                }
                else
                {
                    if (LogSession != null)
                    {
                        LogSession.Write(new LoggingMessage()
                        {
                            Message = ex.ToString()
                        });
                        if (ex.InnerException != null)
                        {
                            LogSession.Write(new LoggingMessage()
                            {
                                Message = ex.InnerException.ToString()
                            });
                        }
                    }

                    autoRestart = true;
                }                
            }
            finally
            {
                if (LogSession != null)
                {
                    LogSession.WriteLine(new LoggingMessage("Monitor stopped.", true));
                }

                NotifyListeners(new BackgroundTaskMonitorEvent()
                {
                    Monitor = this,
                    Type = BackgroundTaskMonitorEventType.Stopped
                });
                thread = null;
                keepMonitor = false;
            }

            if (autoRestart)
            {
                Start();
            }
        }

        public void Dispose()
        {
            
        }
    }
}
