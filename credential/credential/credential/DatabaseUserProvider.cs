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

namespace Toyota.Common.Credential
{    
    public abstract class DatabaseUserProvider: IUserProvider
    {
        protected IDBContextManager DatabaseManager { set; get; }
        protected string DatabaseContextName { set; get; }

        public DatabaseUserProvider() { }
        public DatabaseUserProvider(IDBContextManager dbManager) : this(dbManager, null) { }
        public DatabaseUserProvider(IDBContextManager dbManager, string contextName)
        {
            DatabaseManager = dbManager;
            DatabaseContextName = contextName;
        }

        protected IDBContext GetDatabaseContext()
        {
            IDBContext db;
            if (string.IsNullOrEmpty(DatabaseContextName))
            {
                db = DatabaseManager.GetContext();
            }
            else
            {
                db = DatabaseManager.GetContext(DatabaseContextName);
            }
            db.SetExecutionMode(DBContextExecutionMode.ByName);
            return db;
        }
        public virtual void Init(IDBContextManager dbManager, string contextName) { }

        public abstract void Save(User user);
        public abstract void Delete(string username);
        public abstract IList<User> GetUsers();
        public abstract IList<User> GetUsers(long pageNumber, long pageSize);
        public abstract long GetUserCount();
        public abstract User GetUser(string username);
        public abstract User IsUserAuthentic(string username, string password);
        public virtual void Dispose() { }

        [Obsolete("This method will be removed soon, please do not use this")]
        public abstract void FetchAuthorization(User user);
        [Obsolete("This method will be removed soon, please do not use this")]
        public abstract void FetchAuthorization(IList<User> users);
        [Obsolete("This method will be removed soon, please do not use this")]
        public abstract void FetchOrganization(User user);
        [Obsolete("This method will be removed soon, please do not use this")]
        public abstract void FetchOrganization(IList<User> users);
        [Obsolete("This method will be removed soon, please do not use this")]
        public abstract void FetchPlant(User user);
        [Obsolete("This method will be removed soon, please do not use this")]
        public abstract void FetchPlant(IList<User> users);
        [Obsolete("This method will be removed soon, please do not use this")]
        public abstract IList<User> Search(UserSearchCriteria criteria, object key);
        [Obsolete("This method will be removed soon, please do not use this")]
        public abstract IPagedData<User> Search(UserSearchCriteria criteria, long pageNumber, long pageSize, object key);        
    }
}
