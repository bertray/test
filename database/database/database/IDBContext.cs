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

///
/// <summary>
/// Database modul. This module abstracts common API used to handle common database operations.
/// </summary>
///
namespace Toyota.Common.Database
{
    /// <summary>
    /// A representation of a database context. A database context holds one connection to a database.
    /// </summary>
    /// <remarks>
    /// The term context used here is by means of a "channel", hence database context means a channel to a database.
    /// Using this channel we hopely can do something to the database by using its language, i.e. SQL language.
    /// <para>
    ///     All SQL script execution methods support indexed and named parameter. 
    ///     Indexed parameter uses integer numbers, hence first parameter is written <c>@0</c> ... and so on.
    ///     Named parameters uses human readable words, e.g. if we have a parameter denoting a username we can write the parameter as <c>@username</c>.
    /// </para>
    /// <example>
    /// For indexed parameter we can execute an SQL script like following,
    /// <code>
    ///     /* db is a database context */
    ///     var result = db.Query("select * from tb_r_notification where (sender = @0) and (recipient = @1)", new object[] { "lufty", "yogi" }");
    ///     IList&lt;Notification&gt; result = db.Fetch&lt;Notification&gt;("select * from tb_r_notification where (sender = @0) and (recipient = @1)", new object[] { "lufty", "yogi" }");
    /// </code>
    /// 
    /// For indexed parameter we can execute an SQL script like following,
    /// <code>
    ///     /* db is a database context */
    ///     var result = db.Query("select * from tb_r_notification where (sender = @sender) and (recipient = @recipient)", new { sender = "lufty", recipient = "yogi" }");
    ///     IList&lt;Notification&gt; result = db.Fetch&lt;Notification&gt;("select * from tb_r_notification where (sender = @sender) and (recipient = @recipient)", new { sender = "lufty", recipient = "yogi" }");
    /// </code>
    /// </example>
    /// </remarks>
    public interface IDBContext
    {
        /// <summary>
        /// Gets the name of this context
        /// </summary>
        /// <returns>Name of the context</returns>
        string GetName();

        /// <summary>
        /// Executes an SQL query script.
        /// <c>Query</c> returns an enumerator which can be used by the implementation to use a "lazy loading" mechanism for the result.
        /// </summary>
        /// <typeparam name="T">Type of the data model</typeparam>
        /// <param name="sqlContext">SQL script</param>
        /// <param name="args">SQL script's arguments</param>
        /// <returns>Data result in an enumerator</returns> 
        IEnumerable<T> Query<T>(string sqlContext, params object[] args);

        /// <summary>
        /// Executes an SQL query script.
        /// Basically, <c>Fetch</c> executes a script in the same way as <c>Query</c> does. 
        /// The only difference is <c>Fetch</c> returns a list of data models. Hence the implementation has to pull all resulted data from the
        /// script execution and packs them into their models on a single list.
        /// </summary>
        /// <typeparam name="T">Type of the data model</typeparam>
        /// <param name="sqlContext">SQL script</param>
        /// <param name="args">SQL script's arguments</param>
        /// <returns>List of data models</returns>
        IList<T> Fetch<T>(string sqlContext, params object[] args);

        /// <summary>
        /// Pulling huge number of data is painful, especially when we use <c>Fetch</c>, it "hurts" the cpu and "suffocates" the memory :D. 
        /// <c>FetchByPage</c> reduce the pain by pulling data result of executed script in form of "blocks" or by other common term "page".
        /// For example if we have 10.000 records, instead of pulling all of them, we pull them partially page by page 
        /// which every page contains, let say, 50 data. This way we dont waste memory and gain some cpu performance.
        /// It is submitted to the implementation on what terms does the paging process applies, such as whether the script needs to have 
        /// some particular syntax or a particular structure.
        /// </summary>
        /// <typeparam name="T">Type of data model</typeparam>
        /// <param name="sqlContext">SQL script</param>
        /// <param name="pageNumber">Current page number</param>
        /// <param name="pageSize">Number of data contained on a page</param>
        /// <param name="args">SQL script's arguments</param>
        /// <returns>Instance of <c>IPagedData</c> which type is <c>T</c></returns>
        IPagedData<T> FetchByPage<T>(string sqlContext, long pageNumber, long pageSize, params object[] args);

        /// <summary>
        /// Executes an SQL query script and returns only one data if any data successfully pulled from the database.
        /// It is submitted to the implementation on what terms does it choose which data to be pulled from the whole result.
        /// The implementation also can apply some additional rules as a requirement, such as if there are more than one data 
        /// resulted from the database then the method will throw an exception. Or contrary, the method will return the first data 
        /// available from the database.
        /// </summary>
        /// <typeparam name="T">Type of the data model</typeparam>
        /// <param name="sqlContext">SQL script</param>
        /// <param name="args">SQL script's arguments</param>
        /// <returns>Data object</returns>
        T SingleOrDefault<T>(string sqlContext, params object[] args);

        /// <summary>
        /// Executes an SQL query script and returns the first data of whole data successfully pulled from the database.
        /// So the difference between <c>ExecuteScalar</c> and <c>SingleOrDefault</c> 
        /// is that <c>ExecuteScalar</c> defines a fixed policy that must be applied to the implementation.
        /// </summary>
        /// <typeparam name="T">Type of the data model</typeparam>
        /// <param name="sqlContext">SQL script</param>
        /// <param name="args">SQL script's arguments</param>
        /// <returns>Data object</returns>
        T ExecuteScalar<T>(string sqlContext, params object[] args);

        /// <summary>
        /// Executes an SQL non-query script. SQL operations applies to this method such as UPDATE, DELETE, DROP, ALTER, 
        /// and soon ... including stored procedure execution.
        /// </summary>
        /// <param name="sqlContext">SQL script</param>
        /// <param name="args">SQL script's arguments</param>
        /// <returns>Number of row affected</returns>
        int Execute(string sqlContext, params object[] args);

        /// <summary>
        /// Mark the start of transactional operation. 
        /// This method has to ensure that all database operation between this method and subsequent <c>CommitTransaction</c> call are transactioned,
        /// meaning that if any error, exception, or unexpected condition occurs then all affected data from operations prior that error, 
        /// exception, or unexpected condition will be rolled back to their previous state.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Abort transaction marked by <c>BeginTransaction</c>, <see cref="BeginTransaction"/>.
        /// The implementer should perform rollback process before completely close the transaction.
        /// </summary>
        void AbortTransaction();

        /// <summary>
        /// Mark the end of transactional operation and perform a commit process.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Close the context.
        /// </summary>
        void Close();

        /// <summary>
        /// Gets all query loader supported by the manager.
        /// </summary>
        /// <returns>List of query loader</returns>
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
        /// Sets the execution mode. This will only affect next operation performed.
        /// Implementer should set the default mode to Direct.
        /// <see cref="Toyota.Common.Database.DBContextExecutionMode"/>
        /// </summary>
        /// <param name="executionMode">Execution mode</param>
        void SetExecutionMode(DBContextExecutionMode executionMode);

        /// <summary>
        /// Gets the execution mode.
        /// <see cref="Toyota.Common.Database.DBContextExecutionMode"/>
        /// </summary>
        /// <returns>Execution mode</returns>
        DBContextExecutionMode GetExecutionMode();
    }
}
