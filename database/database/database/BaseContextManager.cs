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

namespace Toyota.Common.Database
{
    /// <summary>
    /// Basic implementation of a context manager.
    /// </summary>
    public abstract class BaseContextManager: IDBContextManager
    {
        private List<ISqlLoader> sqlLoaders;
        private IDictionary<string, ConnectionDescriptor> connectionDescriptors;
        private DBContextExecutionMode contextExecutionMode;

        /// <summary>
        /// Creates instance of this class.
        /// </summary>
        /// <param name="sqlLoaders">Supported SQL loaders</param>
        /// <param name="connectionDescriptors">Connection descriptors used to connect to database</param>
        public BaseContextManager(ISqlLoader[] sqlLoaders, ConnectionDescriptor[] connectionDescriptors)
        {
            this.contextExecutionMode = DBContextExecutionMode.Direct;
            this.connectionDescriptors = new Dictionary<string, ConnectionDescriptor>();
            SetConnectionDescriptor(connectionDescriptors);     
            this.sqlLoaders = new List<ISqlLoader>();
            if (sqlLoaders != null)
            {
                this.sqlLoaders.AddRange(sqlLoaders);
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public void AddSqlLoader(ISqlLoader queryLoader)
        {
            if (!sqlLoaders.Contains(queryLoader))
            {
                sqlLoaders.Add(queryLoader);
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public void RemoveSqlLoader(ISqlLoader sqlLoader)
        {
            if (sqlLoaders.Contains(sqlLoader))
            {
                sqlLoaders.Remove(sqlLoader);
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public IList<ISqlLoader> GetSqlLoaders()
        {
            return sqlLoaders.AsReadOnly();
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public string LoadSqlScript(string name)
        {
            string query;
            foreach (ISqlLoader loader in sqlLoaders)
            {
                query = loader.LoadScript(name);
                if (!string.IsNullOrEmpty(query))
                {
                    return query;
                }
            }
            return null;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public abstract IDBContext GetContext(string name);

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public abstract IDBContext GetContext();

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public void AddConnectionDescriptor(ConnectionDescriptor connectionDescriptor)
        {
            string name = connectionDescriptor.Name;
            if (connectionDescriptors.ContainsKey(name))
            {
                connectionDescriptors[name] = connectionDescriptor;
            }
            else
            {
                connectionDescriptors.Add(name, connectionDescriptor);
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public IList<ConnectionDescriptor> GetConnectionDescriptors()
        {
            if (connectionDescriptors.Count > 0)
            {
                return connectionDescriptors.Values.ToList<ConnectionDescriptor>().AsReadOnly();
            }
            return null;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public void SetConnectionDescriptor(params ConnectionDescriptor[] connectionDescriptors)
        {
            this.connectionDescriptors = new Dictionary<string, ConnectionDescriptor>();
            if ((connectionDescriptors != null) && (connectionDescriptors.Length > 0))
            {
                foreach (ConnectionDescriptor descriptor in connectionDescriptors)
                {
                    this.connectionDescriptors.Add(descriptor.Name, descriptor);
                }                
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public ConnectionDescriptor GetConnectionDescriptor(string name)
        {
            if (connectionDescriptors.ContainsKey(name))
            {
                return connectionDescriptors[name];
            }
            return null;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public void SetDefaultConnectionDescriptor(ConnectionDescriptor connectionDescriptor)
        {
            string name = connectionDescriptor.Name;
            if (!connectionDescriptors.ContainsKey(name))
            {
                connectionDescriptors.Add(name, connectionDescriptor);
            }
            SetDefaultConnectionDescriptor(name);
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public void SetDefaultConnectionDescriptor(string name)
        {
            foreach (ConnectionDescriptor descriptor in connectionDescriptors.Values)
            {
                descriptor.IsDefault = descriptor.Name.Equals(name);
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public ConnectionDescriptor GetDefaultConnectionDescriptor()
        {
            foreach (ConnectionDescriptor descriptor in connectionDescriptors.Values)
            {
                if (descriptor.IsDefault)
                {
                    return descriptor;
                }
            }
            return null;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public void SetContextExecutionMode(DBContextExecutionMode contextExecutionMode)
        {
            this.contextExecutionMode = contextExecutionMode;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public DBContextExecutionMode GetContextExecutionMode()
        {
            return contextExecutionMode;
        }
    }
}
