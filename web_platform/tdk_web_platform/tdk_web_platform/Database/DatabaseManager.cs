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
using Toyota.Common.Configuration;

namespace Toyota.Common.Web.Platform
{
    public class DatabaseManager: IDBContextManager
    {
        private IDBContextManager dbManager;
        private DatabaseManager() 
        {
            dbManager = ProviderRegistry.Instance.Get<IDBContextManager>();
            dbManager.SetContextExecutionMode(DBContextExecutionMode.ByName);
        }

        private static DatabaseManager instance = new DatabaseManager();
        public static DatabaseManager Instance
        {
            get { return instance; }
        }

        public IDBContext GetContext()
        {

            return dbManager.GetContext();
        }

        public IDBContext GetContext(string name)
        {
            return dbManager.GetContext(name);
        }

        public void AddConnectionDescriptor(ConnectionDescriptor connectionDescriptor)
        {
            dbManager.AddConnectionDescriptor(connectionDescriptor);
        }

        public ConnectionDescriptor GetConnectionDescriptor(string name)
        {
            return dbManager.GetConnectionDescriptor(name);
        }

        public void SetConnectionDescriptor(params ConnectionDescriptor[] connectionDescriptors)
        {
            dbManager.SetConnectionDescriptor(connectionDescriptors);
        }

        public IList<ConnectionDescriptor> GetConnectionDescriptors()
        {
            return dbManager.GetConnectionDescriptors();
        }

        public void SetDefaultConnectionDescriptor(ConnectionDescriptor connectionDescriptor)
        {
            dbManager.SetDefaultConnectionDescriptor(connectionDescriptor);
        }

        public void SetDefaultConnectionDescriptor(string name)
        {
            dbManager.SetDefaultConnectionDescriptor(name);
        }

        public ConnectionDescriptor GetDefaultConnectionDescriptor()
        {
            return dbManager.GetDefaultConnectionDescriptor();
        }

        public void AddSqlLoader(ISqlLoader queryLoader)
        {
            dbManager.AddSqlLoader(queryLoader);
        }

        public IList<ISqlLoader> GetSqlLoaders()
        {
            return dbManager.GetSqlLoaders();
        }

        public string LoadSqlScript(string name)
        {
            return dbManager.LoadSqlScript(name);
        }

        public void Close()
        {
            dbManager.Close();
        }

        public void SetContextExecutionMode(DBContextExecutionMode contextExecutionMode)
        {
            dbManager.SetContextExecutionMode(contextExecutionMode);
        }

        public DBContextExecutionMode GetContextExecutionMode()
        {
            return dbManager.GetContextExecutionMode();
        }


        public void RemoveSqlLoader(ISqlLoader sqlLoader)
        {
            dbManager.RemoveSqlLoader(sqlLoader);
        }
    }
}
