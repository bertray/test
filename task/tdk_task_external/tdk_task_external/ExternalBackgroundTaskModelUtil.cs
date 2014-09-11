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
using Toyota.Common.Credential;
using Toyota.Common.Task.External;

namespace Toyota.Common.Task.External
{
    class ExternalBackgroundTaskModelUtil
    {
        public static BackgroundTask TranslateModel(ExternalBackgroundTaskModel model)
        {
            ExternalBackgroundTask task = new ExternalBackgroundTask()
            {
                Description = model.Description,
                FunctionName = model.FunctionName,
                Id = model.Id,
                RegistryId = model.RegistryId,
                Name = model.Name,
                Command = model.Command,
                Progress = model.Progress,
                Type = model.Type,
                Status = model.Status
            };
            task.Submitter = new User() { Username = model.SubmitterUsername };
            if (!string.IsNullOrEmpty(model.Parameter))
            {
                task.Parameters = BackgroundTaskParameter.FromString(model.Parameter);            
            }

            if (model.Type == TaskType.Periodic)
            {
                DateTime date;
                if (model.StartDateInTicks.HasValue)
                {
                    date = new DateTime(model.StartDateInTicks.Value);
                    if (DateTime.Compare(date, DateTime.MinValue) != 0)
                    {
                        task.StartDate = date;
                    }
                }
                if (model.EndDateInTicks.HasValue)
                {
                    date = new DateTime(model.EndDateInTicks.Value);
                    if (DateTime.Compare(date, DateTime.MinValue) != 0)
                    {
                        task.EndDate = date;
                    }
                }

                switch (model.PeriodicTypeValue)
                {
                    case (byte) PeriodicTaskType.DAILY:
                        DailyTaskRange dailyRange = new DailyTaskRange();
                        dailyRange.Interval = model.Interval;
                        //dailyRange.Time = TimeSpan.FromMilliseconds(model.RangeTimeInTicks.Value);
                        dailyRange.Time = TimeSpan.FromTicks(model.RangeTimeInTicks.Value);
                        if (!string.IsNullOrEmpty(model.ExecutionDays))
                        {
                            IList<int> dayList = new List<int>();
                            string[] dailyFractions = model.ExecutionDays.Split(',');                            
                            foreach (string st in dailyFractions)
                            {
                                if (!string.IsNullOrEmpty(st))
                                {
                                    dayList.Add(Convert.ToInt32(st));
                                }
                            }
                            DayOfWeek[] days = new DayOfWeek[dayList.Count];
                            for (int i = 0; i < days.Length; i++)
                            {
                                days[i] = (DayOfWeek) dayList[i];
                            }
                            dailyRange.Days = days;
                        }
                        task.Range = dailyRange;
                        break;
                    case (byte) PeriodicTaskType.WEEKLY:
                        WeeklyTaskRange weeklyRange = new WeeklyTaskRange();
                        weeklyRange.Interval = model.Interval;
                        //weeklyRange.Time = TimeSpan.FromMilliseconds(model.RangeTimeInTicks.Value);
                        weeklyRange.Time = TimeSpan.FromTicks(model.RangeTimeInTicks.Value);
                        if (!string.IsNullOrEmpty(model.ExecutionDays))
                        {
                            IList<int> dayList = new List<int>();
                            string[] dailyFractions = model.ExecutionDays.Split(',');
                            foreach (string st in dailyFractions)
                            {
                                if (!string.IsNullOrEmpty(st))
                                {
                                    dayList.Add(Convert.ToInt32(st));
                                }
                            }
                            DayOfWeek[] days = new DayOfWeek[dayList.Count];
                            for (int i = 0; i < days.Length; i++)
                            {
                                days[i] = (DayOfWeek)dayList[i];
                            }
                            weeklyRange.Days = days;
                        }
                        task.Range = weeklyRange;
                        break;
                    case (byte) PeriodicTaskType.MONTHLY:
                        MonthlyTaskRange monthlyRange = new MonthlyTaskRange();
                        monthlyRange.Interval = model.Interval;
                        //monthlyRange.Time = TimeSpan.FromMilliseconds(model.RangeTimeInTicks.Value);
                        monthlyRange.Time = TimeSpan.FromTicks(model.RangeTimeInTicks.Value);
                        if (!string.IsNullOrEmpty(model.ExecutionDays))
                        {
                            string[] dailyFractions = model.ExecutionDays.Split(',');
                            if(!string.IsNullOrEmpty(dailyFractions[0])) {
                                monthlyRange.Day = Convert.ToByte(dailyFractions[0]);
                            }
                        }
                        task.Range = monthlyRange;
                        break;
                    case (byte) PeriodicTaskType.YEARLY:
                        YearlyTaskRange yearlyRange = new YearlyTaskRange();
                        yearlyRange.Interval = model.Interval;
                        //yearlyRange.Time = TimeSpan.FromMilliseconds(model.RangeTimeInTicks.Value);
                        yearlyRange.Time = TimeSpan.FromTicks(model.RangeTimeInTicks.Value);
                        if (!string.IsNullOrEmpty(model.ExecutionDays))
                        {
                            string[] dailyFractions = model.ExecutionDays.Split(',');
                            if(!string.IsNullOrEmpty(dailyFractions[0])) {
                                yearlyRange.Day = Convert.ToByte(dailyFractions[0]);
                            }
                        }
                        if (!string.IsNullOrEmpty(model.ExecutionMonths))
                        {
                            string[] monthFractions = model.ExecutionMonths.Split(',');
                            if(!string.IsNullOrEmpty(monthFractions[0])) {
                                yearlyRange.Month = Convert.ToByte(monthFractions[0]);
                            }
                        }
                        task.Range = yearlyRange;
                        break;
                    default:
                        task.Range = null;
                        break;
                }
            }
            else if (model.Type == TaskType.Delayed)
            {
                TaskRange range = new TaskRange();
                range.Interval = model.Interval;
                if(model.RangeTimeInTicks.HasValue) 
                {
                    //range.Time = TimeSpan.FromMilliseconds(model.RangeTimeInTicks.Value);
                    range.Time = TimeSpan.FromTicks(model.RangeTimeInTicks.Value);
                }
                task.Range = range;
            }

            BackgroundTaskExecutionDescriptor executionDescriptor = new BackgroundTaskExecutionDescriptor(); 
            if (model.ExecutionStartTime != null)
            {
                executionDescriptor.StartTime = model.ExecutionStartTime;        
            }
            if (model.ExecutionFinishTime != null)
            {
                executionDescriptor.FinishTime = model.ExecutionFinishTime;
            }
            task.ExecutionDescriptor = executionDescriptor;

            return task;
        }

        public static IList<BackgroundTask> NaturalizedExternalModelList(IList<ExternalBackgroundTaskModel> models)
        {
            if ((models != null) && models.Any())
            {
                IList<BackgroundTask> tasks = new List<BackgroundTask>();
                foreach (ExternalBackgroundTaskModel model in models)
                {
                    tasks.Add(ExternalBackgroundTaskModelUtil.TranslateModel(model));
                }
                return tasks;
            }
            return null;
        }
    }
}
