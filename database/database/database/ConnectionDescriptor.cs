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
    /// Holds informations about a database connection.
    /// </summary>
    public class ConnectionDescriptor
    {
        /// <summary>
        /// Creates instance of this class using default criteria.
        /// </summary>
        public ConnectionDescriptor() : this(null, null, "System.Data.SqlClient") { }

        /// <summary>
        /// Creates instance of this class using default provider (<c>System.Data.SqlClient</c>).
        /// </summary>
        /// <param name="name">Name of this connection descriptor</param>
        /// <param name="connectionString">Connection string</param>
        public ConnectionDescriptor(string name, string connectionString): this(name, connectionString, "System.Data.SqlClient") { }

        /// <summary>
        /// Creates instance of this class.
        /// </summary>
        /// <param name="name">Name of this connection descriptor</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="providerName">Full namespace of the client provider</param>
        public ConnectionDescriptor(string name, string connectionString, string providerName)
        {
            this.Name = name;
            this.ConnectionString = connectionString;
            this.ProviderName = providerName;
        }

        /// <summary>
        /// Name of this connection descriptor
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// Connection string
        /// </summary>
        public string ConnectionString { set; get; }

        /// <summary>
        /// Full namespace of the client provider.
        /// <example><c>System.Data.SqlClient</c></example>
        /// </summary>
        public string ProviderName { set; get; }

        /// <summary>
        /// Sets and Gets whether this is the default connection descriptor on the database manager.
        /// <see cref="Toyota.Common.Database.IDBContextManager"/>
        /// </summary>
        public bool IsDefault { set; get; }

        /// <summary>
        /// A string representation of the connection descriptor
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + "," + ProviderName;
        }
    }
}
