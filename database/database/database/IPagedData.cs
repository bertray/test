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
    /// Represents a page on data paging when performing fetch operation, <see cref="Toyota.Common.Database.IDBContext"/>.
    /// </summary>
    /// <typeparam name="T">Type of data model</typeparam>
    public interface IPagedData<T>
    {
        /// <summary>
        /// Gets current page
        /// </summary>
        /// <returns>Current page</returns>
        long GetCurrentPage();

        /// <summary>
        /// Gets page size
        /// </summary>
        /// <returns>Page size</returns>
        long GetPageSize();

        /// <summary>
        /// Gets total page
        /// </summary>
        /// <returns>Tota page count</returns>
        long GetPageCount();

        /// <summary>
        /// Gets total data contained on all page
        /// </summary>
        /// <returns>Total data count</returns>
        long GetDataCount();

        /// <summary>
        /// Gets data contained by this page
        /// </summary>
        /// <returns>List of data</returns>
        IList<T> GetData();

        /// <summary>
        /// Creates a new paged data using given list of data.
        /// </summary>
        /// <typeparam name="U">Type of the data</typeparam>
        /// <param name="data">List of data</param>
        /// <returns>Instance of paged data</returns>
        IPagedData<U> Clone<U>(IList<U> data);
    }
}
