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
using Toyota.Common.Database;
using Toyota.Common.Task.External;

namespace Toyota.Common.Task.External
{
    public class ExternalBackgroundTaskHistory: IBackgroundTaskHistory
    {
        private IDBContextManager dbManager;

        public ExternalBackgroundTaskHistory(IDBContextManager dbManager, string contextName)
        {
            this.dbManager = dbManager;
            DatabaseContextName = contextName;
        }
        public ExternalBackgroundTaskHistory(IDBContextManager dbManager) : this(dbManager, null) { }

        public string DatabaseContextName { set; get; }

        public IList<BackgroundTask> GetAll()
        {
            string query = dbManager.LoadSqlScript("History_Select");
            IDBContext db = CreateDBContext();
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query);
            db.Close();

            return ExternalBackgroundTaskModelUtil.NaturalizedExternalModelList(models);
        }

        public IList<BackgroundTask> GetAll(long pageNumber, long pageSize)
        {
            string query = dbManager.LoadSqlScript("History_Select");
            IDBContext db = CreateDBContext();
            IPagedData<ExternalBackgroundTaskModel> pagedData = db.FetchByPage<ExternalBackgroundTaskModel>(query, pageNumber, pageSize);
            db.Close();

            if (pagedData != null)
            {
                return ExternalBackgroundTaskModelUtil.NaturalizedExternalModelList(pagedData.GetData());
            }
            return null;
        }

        public void Register(BackgroundTask task)
        {
            ExternalBackgroundTask cmdTask = (ExternalBackgroundTask)task;

            string query = dbManager.LoadSqlScript("History_Submit");
            IDBContext db = CreateDBContext();
            var parameters = new
            {
                Id = cmdTask.Id,
                RegistryId = cmdTask.RegistryId,
                Name = cmdTask.Name,
                Description = cmdTask.Description,
                Submitter = cmdTask.Submitter.Username,
                FunctionName = cmdTask.FunctionName,
                Parameter = cmdTask.Parameters != null ? cmdTask.Parameters.ToString() : string.Empty,
                Type = cmdTask.Type,
                Status = cmdTask.Status,
                Command = cmdTask.Command,
                Progress = task.Progress,
                StartTime = cmdTask.ExecutionDescriptor != null ? (DateTime?) cmdTask.ExecutionDescriptor.StartTime : null,
                FinishTime = cmdTask.ExecutionDescriptor != null ? (DateTime?) cmdTask.ExecutionDescriptor.FinishTime : null
            };

            db.Execute(query, parameters);
            db.Close();
        }

        public IList<BackgroundTask> SearchByFunctionName(string functionName)
        {
            string query = dbManager.LoadSqlScript("History_SelectByFunctionName");
            IDBContext db = CreateDBContext();
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query, new { FunctionName = functionName });
            db.Close();

            if (models.Any())
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

        public IList<BackgroundTask> SearchByType(TaskType type)
        {
            string query = dbManager.LoadSqlScript("History_SelectByType");
            IDBContext db = CreateDBContext();
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query, new { Type = (int)type });
            db.Close();

            if (models.Any())
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

        public IList<BackgroundTask> SearchByName(string name)
        {
            string query = dbManager.LoadSqlScript("History_SelectByName");
            IDBContext db = CreateDBContext();
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query, new { Name = name });
            db.Close();

            if (models.Any())
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

        public IList<BackgroundTask> SearchByRegistryId(string id)
        {
            string query = dbManager.LoadSqlScript("History_SelectByRegistryId");
            IDBContext db = CreateDBContext();
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query, new { RegistryId = id });
            db.Close();

            if (models.Any())
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

        public IList<BackgroundTask> SearchByRegistryIdAndStartTimeRange(string registryId, DateTime start, DateTime end)
        {
            string query = dbManager.LoadSqlScript("History_SelectByRegIdAndStartTimeRange");
            IDBContext db = CreateDBContext();

            DateTime currentDate = DateTime.Now;
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query, new { 
                RegistryId = registryId,
                StartDate = start != null ? start : currentDate, 
                EndDate = end != null ? end : currentDate
            });
            db.Close();

            if (models.Any())
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

        public void Dispose()
        {
         
        }

        public void RemoveByFunctionName(string functionName)
        {
            string query = dbManager.LoadSqlScript("History_RemoveByFunctionName");
            IDBContext db = CreateDBContext();
            db.Execute(query, new { FunctionName = functionName });
            db.Close();
        }

        public void RemoveByType(TaskType type)
        {
            string query = dbManager.LoadSqlScript("History_RemoveByType");
            IDBContext db = CreateDBContext();
            db.Execute(query, new { Type = (int)type });
            db.Close();
        }

        public void RemoveByName(string name)
        {
            string query = dbManager.LoadSqlScript("History_RemoveByName");
            IDBContext db = CreateDBContext();
            db.Execute(query, new { Name = name });
            db.Close();
        }

        public void Remove(params BackgroundTask[] tasks)
        {
            if ((tasks != null) && (tasks.Length > 0))
            {
                IDBContext db = CreateDBContext();
                ExternalBackgroundTask extask;
                foreach (BackgroundTask t in tasks)
                {
                    extask = (ExternalBackgroundTask)t;
                    db.Execute("History_Remove", new
                    {
                        Id = extask.Id,
                        RegistryId = extask.RegistryId
                    });
                }
                db.Close();
            }
        }

        public void RemoveAll()
        {
            string query = dbManager.LoadSqlScript("History_RemoveAll");
            IDBContext db = CreateDBContext();
            db.Execute(query);
            db.Close();
        }

        private IDBContext CreateDBContext()
        {
            if (dbManager == null)
            {
                return null;
            }

            IDBContext db;
            if (string.IsNullOrEmpty(DatabaseContextName))
            {                
                db = dbManager.GetContext();
            }
            else
            {
                db = dbManager.GetContext(DatabaseContextName);
            }

            db.SetExecutionMode(DBContextExecutionMode.ByName);
            return db;
        }
    }
}
