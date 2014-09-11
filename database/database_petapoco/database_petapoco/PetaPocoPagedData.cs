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

namespace Toyota.Common.Database.Petapoco
{
    public class PetaPocoPagedData<T>: IPagedData<T>
    {
        private PetaPoco.Page<T> page;

        public PetaPocoPagedData(PetaPoco.Page<T> page)
        {   
            this.page = page;
        }

        public long GetCurrentPage()
        {
            return page.CurrentPage;
        }

        public IList<T> GetData()
        {
            return page.Items;
        }

        public long GetPageSize()
        {
            return page.ItemsPerPage;
        }

        public long GetPageCount()
        {
            return page.TotalPages;
        }

        public long GetDataCount()
        {
            return page.TotalItems;
        }

        public IPagedData<U> Clone<U>(IList<U> data)
        {
            PetaPoco.Page<U> page = new PetaPoco.Page<U>();
            page.CurrentPage = GetCurrentPage();
            page.Items = data != null ? new List<U>(data) : null;
            page.ItemsPerPage = GetPageSize();
            page.TotalPages = GetPageCount();
            page.TotalItems = GetDataCount();

            return new PetaPocoPagedData<U>(page);
        }
    }
}
