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
    /// Manages creation of database context, <see cref="Toyota.Common.Database.IDBContext"/>.
    /// It is submitted to the implementation whether to create database context on every request or using some pooling mechanism.
    /// </summary>
    public interface IDBContextManager
    {
        /// <summary>
        /// Gets a database context using the manager's default criteria.
        /// </summary>
        /// <returns>Database context</returns>
        IDBContext GetContext();

        /// <summary>
        /// Gets a database context by its name.
        /// </summary>
        /// <param name="name">Database context</param>
        /// <returns>Database context instance</returns>
        IDBContext GetContext(string name);

        /// <summary>
        /// Adds a connection descriptor. <see cref="Toyota.Common.Database.ConnectionDescriptor"/>.
        /// </summary>
        /// <param name="connectionDescriptor">Connection descriptor</param>
        void AddConnectionDescriptor(ConnectionDescriptor connectionDescriptor);

        /// <summary>
        /// Gets a connection descriptor by its name. <see cref="Toyota.Common.Database.ConnectionDescriptor"/>.
        /// </summary>
        /// <param name="name">Name of descriptor</param>
        /// <returns>Connection descriptor</returns>
        ConnectionDescriptor GetConnectionDescriptor(string name);    
    
        /// <summary>
        /// Sets all connection descriptor supported by the manager. 
        /// This will replace all existing connection descriptors.
        /// <see cref="Toyota.Common.Database.ConnectionDescriptor"/>.
        /// </summary>
        /// <param name="connectionDescriptors">Array of connection descriptors</param>
        void SetConnectionDescriptor(params ConnectionDescriptor[] connectionDescriptors);

        /// <summary>
        /// Gets all connection descriptors supported by the manager.
        /// <see cref="Toyota.Common.Database.ConnectionDescriptor"/>.
        /// </summary>
        /// <returns>List of connection descriptors</returns>
        IList<ConnectionDescriptor> GetConnectionDescriptors();

        /// <summary>
        /// Sets default connection descriptors.
        /// Default connection descriptors is used by <c>GetContext</c>, <see cref="GetContext(string)"/>.
        /// <see cref="Toyota.Common.Database.ConnectionDescriptor"/>.
        /// </summary>
        /// <param name="connectionDescriptor">Connection descriptors</param>
        void SetDefaultConnectionDescriptor(ConnectionDescriptor connectionDescriptor);

        /// <summary>
        /// Sets default connection descriptors by referring to existing descriptor's name.
        /// <see cref="SetDefaultConnectionDescriptor(Toyota.Common.Database.ConnectionDescriptor)"/>.
        /// <see cref="Toyota.Common.Database.ConnectionDescriptor"/>.
        /// </summary>
        /// <param name="name">Existing connection descriptor name</param>
        void SetDefaultConnectionDescriptor(string name);

        /// <summary>
        /// Gets default connection descriptor.
        /// <see cref="Toyota.Common.Database.ConnectionDescriptor"/>.
        /// </summary>
        /// <returns>Default connection description</returns>
        ConnectionDescriptor GetDefaultConnectionDescriptor();

        /// <summary>
        /// Add SQL loader. 
        /// <see cref="Toyota.Common.Database.ISqlLoader"/>.
        /// </summary>
        /// <param name="sqlLoader">SQL loader</param>
        void AddSqlLoader(ISqlLoader sqlLoader);

        /// <summary>
        /// Remove SQL loader. 
        /// <see cref="Toyota.Common.Database.ISqlLoader"/>.
        /// </summary>
        /// <param name="sqlLoader">SQL loader</param>
        void RemoveSqlLoader(ISqlLoader sqlLoader);

        /// <summary>
        /// Gets all sql loader supported by the manager.
        /// <see cref="Toyota.Common.Database.ISqlLoader"/>.
        /// </summary>
        /// <returns>List of SQL loader</returns>
        IList<ISqlLoader> GetSqlLoaders();

        /// <summary>
        /// Loads SQL script by its name. 
        /// The SQL script will be loaded by consulting to registered SQL Loaders.
        /// <see cref="Toyota.Common.Database.ISqlLoader"/>.
        /// </summary>
        /// <param name="name">SQL script name</param>
        /// <returns>SQL script</returns>
        string LoadSqlScript(string name);
        
        /// <summary>
        /// Closes the manager.
        /// </summary>
        void Close();

        /// <summary>
        /// Sets execution mode. This is the execution mode of the manager not the database context.
        /// The manager will use its execution mode as a default mode for newly created database context.
        /// This will only affect next newly created database context,
        /// previous created database context will not be affected.
        /// <see cref="Toyota.Common.Database.DBContextExecutionMode"/>
        /// </summary>
        /// <param name="contextExecutionMode">Execution mode</param>
        void SetContextExecutionMode(DBContextExecutionMode contextExecutionMode);

        /// <summary>
        /// Gets current execution mode. 
        /// This is the execution mode of the manager, not the database context.
        /// </summary>
        /// <returns>Current execution mode of the manager</returns>
        DBContextExecutionMode GetContextExecutionMode();
    }
}
