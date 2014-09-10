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
    /// A default implementation of <c>IPagedData</c>.
    /// <see cref="Toyota.Common.Database.IPagedData"/>
    /// </summary>
    /// <typeparam name="T">Type of Data</typeparam>
    public class PagedData<T>: IPagedData<T>
    {
        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        public PagedData(): this(null) { }

        /// <summary>
        /// Constructs instance of the class
        /// </summary>
        /// <param name="data">List of data to use</param>
        public PagedData(IList<T> data) {
            this.data = data;
        }

        private long currentPage;
        /// <summary>
        /// <see cref="Toyota.Common.Database.IPagedData"/>
        /// </summary>
        public long GetCurrentPage()
        {
            return currentPage;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IPagedData"/>
        /// </summary>
        public void SetCurrentPage(long currentPage)
        {
            this.currentPage = currentPage;
        }

        private long pageSize;
        /// <summary>
        /// <see cref="Toyota.Common.Database.IPagedData"/>
        /// </summary>
        public long GetPageSize()
        {
            return pageSize;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IPagedData"/>
        /// </summary>
        public void SetPageSize(long pageSize)
        {
            this.pageSize = pageSize;
        }

        private long pageCount;
        /// <summary>
        /// <see cref="Toyota.Common.Database.IPagedData"/>
        /// </summary>
        public long GetPageCount()
        {
            return pageCount;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IPagedData"/>
        /// </summary>
        public void SetPageCount(long pageCount)
        {
            this.pageCount = pageCount;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IPagedData"/>
        /// </summary>
        public long GetDataCount()
        {
            return (data != null) ? data.Count : 0;
        }

        private IList<T> data;
        /// <summary>
        /// <see cref="Toyota.Common.Database.IPagedData"/>
        /// </summary>
        public IList<T> GetData()
        {
            return data;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IPagedData"/>
        /// </summary>
        public void SetData(IList<T> data)
        {
            this.data = data;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.IPagedData"/>
        /// </summary>
        public IPagedData<U> Clone<U>(IList<U> data)
        {
            PagedData<U> pagedData = new PagedData<U>();
            pagedData.SetData(data);
            pagedData.SetCurrentPage(currentPage);
            pagedData.SetPageSize(pageSize);
            pagedData.SetPageCount(pageCount);

            return pagedData;
        }
    }
}
