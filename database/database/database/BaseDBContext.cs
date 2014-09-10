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
    /// Basic implementation of a database context
    /// </summary>
    public abstract class BaseDBContext: IDBContext
    {
        private string name;
        private ConnectionDescriptor connectionDescriptor;
        private List<ISqlLoader> sqlLoaders;
        private DBContextExecutionMode executionMode;

        /// <summary>
        /// Creates instance of this class
        /// </summary>
        /// <param name="connectionDescriptor">Connection descriptor used to connect to database</param>
        /// <param name="sqlLoaders">Supported SQL loaders</param>
        public BaseDBContext(ConnectionDescriptor connectionDescriptor, IList<ISqlLoader> sqlLoaders)
        {
            this.name = connectionDescriptor.Name;
            this.connectionDescriptor = connectionDescriptor;
            this.executionMode = DBContextExecutionMode.Direct;
            this.sqlLoaders = new List<ISqlLoader>();
            this.sqlLoaders.AddRange(sqlLoaders);
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
        /// </summary>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
        /// </summary>
        public abstract IEnumerable<T> Query<T>(string sql, params object[] args);

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
        /// </summary>
        public abstract IList<T> Fetch<T>(string sql, params object[] args);

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
        /// </summary>
        public abstract IPagedData<T> FetchByPage<T>(string sql, long pageNumber, long pageSize, params object[] args);

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
        /// </summary>
        public abstract T SingleOrDefault<T>(string sql, params object[] args);

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
        /// </summary>
        public abstract T ExecuteScalar<T>(string sql, params object[] args);

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
        /// </summary>
        public abstract int Execute(string sql, params object[] args);

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
        /// </summary>
        public abstract void BeginTransaction();

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
        /// </summary>
        public abstract void CommitTransaction();

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
        /// </summary>
        public abstract void AbortTransaction();

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
        /// </summary>
        public IList<ISqlLoader> GetSqlLoaders()
        {
            return sqlLoaders;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
        /// </summary>
        public void SetExecutionMode(DBContextExecutionMode executionMode)
        {
            this.executionMode = executionMode;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
        /// </summary>
        public DBContextExecutionMode GetExecutionMode()
        {
            return executionMode;
        }

        /// <summary>
        /// Load the sql context given by the argument. 
        /// If current execution mode is ByName, it will ask SQL script
        /// of the context from registered SQL Loaders.
        /// If current execution mode is Direct, it will return back the passed sql context to the caller.
        /// </summary>
        /// <param name="sqlContext">SQL context</param>
        /// <returns>SQL script or the SQL context itself</returns>
        protected string LoadSqlContext(string sqlContext)
        {
            if (sqlLoaders != null)
            {
                if (executionMode == DBContextExecutionMode.ByName)
                {
                    string sqlStatement;
                    foreach (ISqlLoader sqlLoader in sqlLoaders)
                    {
                        sqlStatement = sqlLoader.LoadScript(sqlContext);
                        if (!string.IsNullOrEmpty(sqlStatement))
                        {
                            return sqlStatement;
                        }
                    }
                }
            }            
            return sqlContext;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IDBContext"/>
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
    }
}
