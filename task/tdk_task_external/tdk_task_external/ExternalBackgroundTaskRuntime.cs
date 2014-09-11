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
using System.Reflection;
using Toyota.Common.Logging;
using System.IO;
using System.Xml;
using Toyota.Common.Logging.Sink;
using System.ComponentModel.Composition;

namespace Toyota.Common.Task.External
{    
    public abstract class ExternalBackgroundTaskRuntime: BackgroundTaskRuntime
    {
        public const string DEFAULT_LOG_FOLDER = "Logs\\Task";

        public ExternalBackgroundTaskRuntime(string name): this(name, null, null) { }
        public ExternalBackgroundTaskRuntime(string name, IDBContextManager dbManager, string contextName)
        {
            Name = name;
            if (string.IsNullOrEmpty(Name))
            {
                Name = GetType().Name;
            }
            DatabaseManager = dbManager;
            DatabaseContextName = contextName;
        }

        public void ExecuteExternal(string[] args)
        {            
            if((args != null) && (args.Length > 0)) {
                base.Execute(args[0]);
            }
        }
        
        public LoggingManager LogManager { set; get; }
        public LoggingSession DefaultLogSession { set; get; }

        public string DatabaseContextName { set; get; }
        public IDBContextManager DatabaseManager { set; get; }

        public override void SetProgress(byte progress)
        {
            base.SetProgress(progress);

            if (DatabaseManager != null)
            {
                IDBContext db = CreateDBContext();
                db.Execute(DatabaseManager.LoadSqlScript("Queue_UpdateProgress"), new { Progress = GetProgress(), Id = Id });
                db.Close();
            }            
        }
        public override void SetStatus(TaskStatus status)
        {
            base.SetStatus(status);
            if (DatabaseManager != null)
            {
                IDBContext db = CreateDBContext();
                var param = new { Status = (int)GetStatus(), Id = Id };
                db.Execute(DatabaseManager.LoadSqlScript("Queue_UpdateStatus"), param);
                db.Close();
            }            
        }

        protected override void OnBeforeRuntimeExecution()
        {
            if (LogManager == null)
            {
                LogManager = new LoggingManager();
                LogManager.AddSink("TextFile", typeof(TextFileLoggingSink), true);
            }

            if (DefaultLogSession == null)
            {
                DefaultLogSession = LogManager.CreateSession(string.Format("{0}-{1}", Name, Id));
                DefaultLogSession.AutoFlush = true;
                DefaultLogSession.EnableMultiSink = true;
            }

            DefaultLogSession.WriteLine();
            DefaultLogSession.WriteLine(new LoggingMessage(">>>>> START <<<<<"));
        }
        protected override void OnAfterRuntimeExecution()
        {
            DefaultLogSession.WriteLine(new LoggingMessage(">>>>> END <<<<<"));
            if ((DatabaseManager != null) && StartTime.HasValue && FinishTime.HasValue)
            {
                IDBContext db = CreateDBContext();
                db.Execute(DatabaseManager.LoadSqlScript("Queue_UpdateExecutionTime"), new { StartTime = StartTime, FinishTime = FinishTime, Id = Id });
                db.Close();
            }
            CleanUp();            
        }
        protected override void OnExceptionOccured(Exception ex)
        {
            try
            {
                SetStatus(TaskStatus.Error);
                LogException(ex);
            }
            catch (Exception exception)
            {
                LogException(exception);
            }
        }
                
        private void CleanUp()
        {
            if (DefaultLogSession != null)
            {
                DefaultLogSession.Flush();
                DefaultLogSession.Close();
            }
            if (LogManager != null)
            {
                LogManager.Close();
            }
        }
        private void LogException(Exception ex)
        {
            DefaultLogSession.WriteLine(new LoggingMessage(LoggingSeverity.Error, ex.ToString()));
            if (ex.InnerException != null)
            {
                DefaultLogSession.WriteLine(new LoggingMessage(LoggingSeverity.Error, ex.InnerException.ToString()));
            }
        }
        private IDBContext CreateDBContext()
        {
            if (!string.IsNullOrEmpty(DatabaseContextName))
            {
                return DatabaseManager.GetContext(DatabaseContextName);
            }
            else
            {
                return DatabaseManager.GetContext();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (DefaultLogSession != null)
            {
                DefaultLogSession.Dispose();
            }
        }
    }
}
