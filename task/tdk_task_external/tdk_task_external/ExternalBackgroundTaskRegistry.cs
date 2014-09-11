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
using Toyota.Common.Database;
using Toyota.Common.Task;
using System.IO;
using Toyota.Common.Credential;
using Toyota.Common.Task.External;
using System.Dynamic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Toyota.Common.Task.External
{
    public class ExternalBackgroundTaskRegistry: BackgroundTaskRegistryEventBroadcaster, IBackgroundTaskRegistry
    {
        private IDBContextManager dbManager;        

        public ExternalBackgroundTaskRegistry(IDBContextManager dbManager, string contextName)
        {
            this.dbManager = dbManager;
            DatabaseContextName = contextName;
        }
        public ExternalBackgroundTaskRegistry(IDBContextManager dbManager) : this(dbManager, null) { }

        private string DatabaseContextName { set; get; }
        
        public void Register(BackgroundTask task)
        {
            ExternalBackgroundTask cmdTask = (ExternalBackgroundTask) task;

            cmdTask.Id = GenerateId();
            cmdTask.Status = TaskStatus.Scheduled;

            string query = dbManager.LoadSqlScript("Registry_Submit");
            IDBContext db = CreateDBContext();            

            dynamic parameters = new ExpandoObject();
            parameters.Id = cmdTask.Id;
            parameters.Name = cmdTask.Name;
            parameters.Description = cmdTask.Description;
            parameters.Submitter = cmdTask.Submitter.Username;
            parameters.FunctionName = cmdTask.FunctionName;
            parameters.Parameter = string.Empty;
            if (cmdTask.Parameters != null)
            {
                parameters.Parameter = cmdTask.Parameters.ToString();
            }
            parameters.Type = cmdTask.Type;
            parameters.Status = cmdTask.Status;
            parameters.Command = cmdTask.Command;
            parameters.StartDate = DateTime.MinValue.Ticks;
            if (cmdTask.StartDate.HasValue)
            {
                parameters.StartDate = cmdTask.StartDate.Value.Ticks;
            }
            parameters.EndDate = DateTime.MinValue.Ticks;
            if (cmdTask.EndDate.HasValue)
            {
                parameters.EndDate = cmdTask.EndDate.Value.Ticks;
            }
            parameters.PeriodicType = cmdTask.PeriodicType;
            parameters.Interval = null;
            if (cmdTask.Range != null)
            {
                parameters.Interval = cmdTask.Range.Interval;
            }
            parameters.Time = TimeSpan.MinValue.TotalMilliseconds;
            if (cmdTask.Range != null)
            {
                parameters.Time = cmdTask.Range.Time.Ticks;
            }

            string executionDays = string.Empty;
            string executionMonths = string.Empty;
            if (cmdTask.Type == TaskType.Periodic)
            {
                switch (cmdTask.PeriodicType)
                {
                    case PeriodicTaskType.UNDEFINED: break;
                    case PeriodicTaskType.WEEKLY:
                        StringBuilder stringBuilder = new StringBuilder();
                        WeeklyTaskRange weeklyRange = (WeeklyTaskRange)cmdTask.Range;
                        if (weeklyRange.Days != null)
                        {
                            foreach (DayOfWeek d in weeklyRange.Days)
                            {
                                stringBuilder.Append(Convert.ToString((int)d)).Append(',');
                            }
                            stringBuilder.Remove(stringBuilder.Length - 1, 1);
                            executionDays = stringBuilder.ToString();
                        }

                        break;
                    case PeriodicTaskType.MONTHLY:
                        MonthlyTaskRange monthlyRange = (MonthlyTaskRange)cmdTask.Range;
                        if (monthlyRange.Day != null)
                        {
                            executionDays = Convert.ToString(monthlyRange.Day);
                        }
                        break;
                    case PeriodicTaskType.YEARLY:
                        YearlyTaskRange yearlyRange = (YearlyTaskRange)cmdTask.Range;
                        if (yearlyRange.Day != null)
                        {
                            executionDays = Convert.ToString(yearlyRange.Day);
                        }
                        if (yearlyRange.Month != null)
                        {
                            executionMonths = Convert.ToString(yearlyRange.Month);
                        }

                        break;
                    default:
                        stringBuilder = new StringBuilder();
                        DailyTaskRange dailyRange = (DailyTaskRange)cmdTask.Range;
                        if (dailyRange.Days != null)
                        {
                            foreach (DayOfWeek d in dailyRange.Days)
                            {
                                stringBuilder.Append(Convert.ToString((int)d)).Append(',');
                            }
                            stringBuilder.Remove(stringBuilder.Length - 1, 1);
                            executionDays = stringBuilder.ToString();
                        }

                        break;
                }
            }
            parameters.ExecutionDays = executionDays;
            parameters.ExecutionMonths = executionMonths;


            db.Execute(query, new
            {
                Id = parameters.Id,
                Name = parameters.Name,
                Description = parameters.Description,
                Submitter = parameters.Submitter,
                FunctionName = parameters.FunctionName,
                Parameter = parameters.Parameter,
                Type = parameters.Type,
                Status = parameters.Status,
                Command = parameters.Command,
                StartDate = parameters.StartDate,
                EndDate = parameters.EndDate,
                PeriodicType = parameters.PeriodicType,
                Interval = parameters.Interval,
                Time = parameters.Time,
                ExecutionDays = parameters.ExecutionDays,
                ExecutionMonths = parameters.ExecutionMonths
            });
            db.Close();

            NotifyListeners(new BackgroundTaskRegistryEvent() { 
                Registry = this, 
                Type = BackgroundTaskRegistryEventType.Task_Registered
            });
        }

        public IList<BackgroundTask> GetAll()
        {
            string query = dbManager.LoadSqlScript("Registry_Select");
            IDBContext db = CreateDBContext();
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query);
            db.Close();

            return ExternalBackgroundTaskModelUtil.NaturalizedExternalModelList(models);
        }

        public IList<BackgroundTask> GetAll(long pageNumber, long pageSize)
        {
            string query = dbManager.LoadSqlScript("Registry_Select");
            IDBContext db = CreateDBContext();
            IPagedData<ExternalBackgroundTaskModel> pagedData = db.FetchByPage<ExternalBackgroundTaskModel>(query, pageNumber, pageSize);
            db.Close();
                        
            if (pagedData != null)
            {
                return ExternalBackgroundTaskModelUtil.NaturalizedExternalModelList(pagedData.GetData());
            }
            return null;
        }

        public BackgroundTask GetById(string id)
        {

            string query = dbManager.LoadSqlScript("Registry_SelectById");
            IDBContext db = CreateDBContext();
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query, new { Id = id });
            db.Close();

            if (models.Any())
            {
                return ExternalBackgroundTaskModelUtil.TranslateModel(models[0]);
            }
            return null;
        }

        public IList<BackgroundTask> GetByFunctionName(string functionName)
        {
            string query = dbManager.LoadSqlScript("Registry_SelectByFunctionName");
            IDBContext db = CreateDBContext();
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query, new { FunctionName = functionName });
            db.Close();

            return ExternalBackgroundTaskModelUtil.NaturalizedExternalModelList(models);
        }

        public IList<BackgroundTask> GetByType(TaskType type)
        {
            string query = dbManager.LoadSqlScript("Registry_SelectByType");
            IDBContext db = CreateDBContext();
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query, new { Type = (int) type });
            db.Close();

            return ExternalBackgroundTaskModelUtil.NaturalizedExternalModelList(models);
        }

        public IList<BackgroundTask> GetByName(string name)
        {
            string query = dbManager.LoadSqlScript("Registry_SelectByName");
            IDBContext db = CreateDBContext();
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query, new { Name = name });
            db.Close();

            return ExternalBackgroundTaskModelUtil.NaturalizedExternalModelList(models);
        }

        public void Dispose()
        {
            
        }

        private string GenerateId()
        {
            string id = null;
            int existingId;
            bool unique = false;

            string query = dbManager.LoadSqlScript("Registry_CheckId");
            IDBContext db = CreateDBContext();
            while (!unique)
            {
                id = Guid.NewGuid().ToString();
                existingId = db.ExecuteScalar<int>(query, new { Id = id });
                unique = (existingId == 0);
            }
            db.Close();

            return id;
        }

        public void RemoveById(string id)
        {
            string query = dbManager.LoadSqlScript("Registry_RemoveById");
            IDBContext db = CreateDBContext();
            db.Execute(query, new { Id = id });
            db.Close();

            NotifyListeners(new BackgroundTaskRegistryEvent()
            {
                Registry = this,
                Type = BackgroundTaskRegistryEventType.Task_Removed
            });
        }

        public void RemoveByFunctionName(string functionName)
        {
            string query = dbManager.LoadSqlScript("Registry_RemoveByFunctionName");
            IDBContext db = CreateDBContext();
            db.Execute(query, new { FunctionName = functionName });
            db.Close();

            NotifyListeners(new BackgroundTaskRegistryEvent()
            {
                Registry = this,
                Type = BackgroundTaskRegistryEventType.Task_Removed
            });
        }

        public void RemoveByType(TaskType type)
        {
            string query = dbManager.LoadSqlScript("Registry_RemoveByType");
            IDBContext db = CreateDBContext();
            db.Execute(query, new { Type = (int) type });
            db.Close();

            NotifyListeners(new BackgroundTaskRegistryEvent()
            {
                Registry = this,
                Type = BackgroundTaskRegistryEventType.Task_Removed
            });
        }

        public void RemoveByName(string name)
        {
            string query = dbManager.LoadSqlScript("Registry_RemoveByName");
            IDBContext db = CreateDBContext();
            db.Execute(query, new { Name = name });
            db.Close();

            NotifyListeners(new BackgroundTaskRegistryEvent()
            {
                Registry = this,
                Type = BackgroundTaskRegistryEventType.Task_Removed
            });
        }        

        public void RemoveAll()
        {
            string query = dbManager.LoadSqlScript("Registry_RemoveAll");
            IDBContext db = CreateDBContext();
            db.Execute(query);
            db.Close();

            NotifyListeners(new BackgroundTaskRegistryEvent()
            {
                Registry = this,
                Type = BackgroundTaskRegistryEventType.Task_Removed
            });
        }

        private IDBContext CreateDBContext()
        {
            if (dbManager == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(DatabaseContextName))
            {
                return dbManager.GetContext();
            }
            else
            {
                return dbManager.GetContext(DatabaseContextName);
            }
        }
    }
}
