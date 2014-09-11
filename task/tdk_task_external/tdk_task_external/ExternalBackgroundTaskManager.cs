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
using System.Reflection;
using Toyota.Common.Database.Petapoco;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Toyota.Common.Logging;
using Toyota.Common.Logging.Sink;
using System.IO;

namespace Toyota.Common.Task.External
{
    public class ExternalBackgroundTaskManager: BackgroundTaskManager
    {
        private IDBContextManager dbManager;
        public IList<string> RuntimeLocations { private set; get; }

        [ImportMany(typeof(ExternalBackgroundTaskRuntime))]
        private IEnumerable<Lazy<ExternalBackgroundTaskRuntime>> Runtimes;

        public ExternalBackgroundTaskManager(ConnectionDescriptor connectionDescriptor) : this(
            (IDBContextManager) new PetaPocoContextManager(
                new ISqlLoader[] {
                    new AssemblyFileSqlLoader(Assembly.GetAssembly(typeof(ExternalBackgroundTask)), "Toyota.Common.Task.External.Queries")
                }, new ConnectionDescriptor[] { connectionDescriptor }
            ), connectionDescriptor.Name, true) {}

        public ExternalBackgroundTaskManager(IDBContextManager dbManager) : this(dbManager, null, false) { }
        public ExternalBackgroundTaskManager(IDBContextManager dbManager, string contextName) : this(dbManager, contextName, false) { }

        private ExternalBackgroundTaskManager(IDBContextManager dbManager, string contextName, bool internalDBManager): base(null, null, null, null, null)
        {
            if (internalDBManager)
            {
                this.dbManager = dbManager;
            }

            IDBContext db;
            if (string.IsNullOrEmpty(contextName))
            {
                db = dbManager.GetContext();
            }
            else
            {
                db = dbManager.GetContext(contextName);
            }
            db.SetExecutionMode(DBContextExecutionMode.ByName);
            db.Execute("Table_Create");
            db.Close();

            Registry = new ExternalBackgroundTaskRegistry(dbManager, contextName);
            Queue = new ExternalBackgroundTaskQueue(dbManager, contextName);
            History = new ExternalBackgroundTaskHistory(dbManager, contextName);
            Executor = new ExternalBackgroundTaskExecutor(dbManager, contextName, LogManager);
            Monitor = new ExternalBackgroundTaskMonitor(
                (ExternalBackgroundTaskRegistry)Registry,
                (ExternalBackgroundTaskQueue)Queue,
                (ExternalBackgroundTaskHistory)History,
                (ExternalBackgroundTaskExecutor)Executor
            );            

            ExternalBackgroundTaskMonitor externalMonitor = (ExternalBackgroundTaskMonitor)Monitor;
            externalMonitor.LogManager = LogManager;

            RuntimeLocations = new List<string>();
            string defaultTaskPath = Directory.GetCurrentDirectory() + "\\Tasks";
            if (!Directory.Exists(defaultTaskPath))
            {
                Directory.CreateDirectory(defaultTaskPath);
            }
            RuntimeLocations.Add(defaultTaskPath);

            LogManager.RemoveAllSink();
            LogManager.AddSink("Database", typeof(DatabaseLoggingSink), "System", "TB_R_BACKGROUND_TASK_LOG", dbManager);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (dbManager != null) 
            {
                dbManager.Close();
            }
        }

        public override void Start(bool blocking)
        {
            if (RuntimeLocations.Count > 0)
            {
                AggregateCatalog runtimeCatalog = new AggregateCatalog();
                foreach (string location in RuntimeLocations)
                {
                    runtimeCatalog.Catalogs.Add(new DirectoryCatalog(location));
                }
                CompositionContainer runtimeContainer = new CompositionContainer(runtimeCatalog);
                runtimeContainer.ComposeParts(this);

                ExternalBackgroundTaskExecutor executor = (ExternalBackgroundTaskExecutor)Executor;
                executor.Runtimes = Runtimes;
            }                        

            base.Start(blocking);
        }
    }
}
