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
using System.IO;
using Toyota.Common.Task.External;

namespace Toyota.Common.Task.External
{
    public class ExternalBackgroundTaskQueue: IBackgroundTaskQueue
    {
        private int maxConcurrentExecution;
        private IDBContextManager dbManager;

        public ExternalBackgroundTaskQueue(IDBContextManager dbManager, string contextName)
        {
            maxConcurrentExecution = 10;
            this.dbManager = dbManager;
            DatabaseContextName = contextName;
        }
        public ExternalBackgroundTaskQueue(IDBContextManager dbManager): this(dbManager, null) { }

        public string DatabaseContextName { set; get; }

        public void SetMaxConcurrentExecution(int numExecution)
        {
            maxConcurrentExecution = numExecution;
        }

        public int GetMaxConcurrentExecution()
        {
            return maxConcurrentExecution;
        }

        public void Register(BackgroundTask task)
        {
            ExternalBackgroundTask cmdTask = (ExternalBackgroundTask)task;
            cmdTask.RegistryId = cmdTask.Id;            
            cmdTask.Id = GenerateIdIfOccupied(cmdTask.Id);
            cmdTask.Status = TaskStatus.Released;

            string query = dbManager.LoadSqlScript("Queue_Submit");
            IDBContext db = CreateDBContext();
            db.Execute(query, new
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
                Command = cmdTask.Command
            });
            db.Close();
        }

        public IList<BackgroundTask> GetAll()
        {
            string query = dbManager.LoadSqlScript("Queue_Select");
            IDBContext db = CreateDBContext();
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query);
            db.Close();

            return ExternalBackgroundTaskModelUtil.NaturalizedExternalModelList(models);
        }

        public IList<BackgroundTask> GetAll(long pageNumber, long pageSize)
        {
            string query = dbManager.LoadSqlScript("Queue_Select");
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
            string query = dbManager.LoadSqlScript("Queue_SelectById");
            IDBContext db = CreateDBContext();
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query, new { Id = id });
            db.Close();

            if (models.Any())
            {
                return ExternalBackgroundTaskModelUtil.TranslateModel(models[0]);
            }
            return null;
        }

        public IList<BackgroundTask> GetByRegistryId(string id)
        {
            string query = dbManager.LoadSqlScript("Queue_SelectByRegistryId");
            IDBContext db = CreateDBContext();
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query, new { RegistryId = id });
            db.Close();

            if (models.Any())
            {
                IList<BackgroundTask> result = new List<BackgroundTask>();
                foreach (ExternalBackgroundTaskModel t in models)
                {
                    result.Add(ExternalBackgroundTaskModelUtil.TranslateModel(t));
                }

                return result.Count > 0 ? result : null ;
            }
            return null;
        }

        public IList<BackgroundTask> GetByFunctionName(string functionName)
        {
            string query = dbManager.LoadSqlScript("Queue_SelectByFunctionName");
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

        public IList<BackgroundTask> GetByType(TaskType type)
        {
            string query = dbManager.LoadSqlScript("Queue_SelectByType");
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

        public IList<BackgroundTask> GetByName(string name)
        {
            string query = dbManager.LoadSqlScript("Queue_SelectByName");
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

        public IList<BackgroundTask> GetByStatus(TaskStatus status)
        {
            string query = dbManager.LoadSqlScript("Queue_SelectByStatus");
            IDBContext db = CreateDBContext();
            IList<ExternalBackgroundTaskModel> models = db.Fetch<ExternalBackgroundTaskModel>(query, new { Status = status });
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

        private string GenerateIdIfOccupied(string taskId)
        {
            string id = null;
            int existingId;
            bool unique = false;

            string query = dbManager.LoadSqlScript("Queue_CheckId");
            IDBContext db = CreateDBContext();

            if (!string.IsNullOrEmpty(taskId))
            {
                existingId = db.ExecuteScalar<int>(query, new { Id = taskId });
                if (existingId == 0)
                {
                    return taskId;
                }
            }            
            
            while (!unique)
            {
                id = Path.GetRandomFileName().Replace(".", "").ToUpper();
                existingId = db.ExecuteScalar<int>(query, new { Id = id });
                unique = existingId == 0;
            }
            db.Close();

            return id;
        }

        public void RemoveById(string id)
        {
            string query = dbManager.LoadSqlScript("Queue_RemoveById");
            IDBContext db = CreateDBContext();
            db.Execute(query, new { Id = id });
            db.Close();
        }

        public void RemoveByFunctionName(string functionName)
        {
            string query = dbManager.LoadSqlScript("Queue_RemoveByFunctionName");
            IDBContext db = CreateDBContext();
            db.Execute(query, new { FunctionName = functionName });
            db.Close();
        }

        public void RemoveByType(TaskType type)
        {
            string query = dbManager.LoadSqlScript("Queue_RemoveByType");
            IDBContext db = CreateDBContext();
            db.Execute(query, new { Type = (int)type });
            db.Close();
        }

        public void RemoveByName(string name)
        {
            string query = dbManager.LoadSqlScript("Queue_RemoveByName");
            IDBContext db = CreateDBContext();
            db.Execute(query, new { Name = name });
            db.Close();
        }        

        public void RemoveByStatus(TaskStatus status)
        {
            string query = dbManager.LoadSqlScript("Queue_RemoveByStatus");
            IDBContext db = CreateDBContext();
            db.Execute(query, new { Status = status });
            db.Close();
        }

        public void RemoveAll()
        {
            string query = dbManager.LoadSqlScript("Queue_RemoveAll");
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
