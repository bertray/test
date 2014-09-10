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
using Toyota.Common.Utilities;
using Toyota.Common.Lookup;

namespace Toyota.Common.Credential
{
    public class DefaultSessionProvider: ISessionProvider
    {
        private IDBContextManager DatabaseManager { set; get; }
        private string DatabaseContextName { set; get; }
        private ISqlLoader SqlLoader { set; get; }
        private IUserProvider UserProvider { set; get; }

        public DefaultSessionProvider(IUserProvider userProvider, IDBContextManager dbManager, ISessionStorage sessionStorage) : this(userProvider, dbManager, sessionStorage, null) { }
        public DefaultSessionProvider(IUserProvider userProvider, IDBContextManager dbManager, ISessionStorage sessionStorage, string contextName)
        {
            DatabaseManager = dbManager;
            DatabaseContextName = contextName;
            UserProvider = userProvider;
            _sessionStorage = sessionStorage;

            SqlLoader = new AssemblyFileSqlLoader(GetType().Assembly, "Toyota.Common.Credential.Queries");
            if (DatabaseManager != null)
            {
                DatabaseManager.AddSqlLoader(SqlLoader);
            }

            IDBContext db = CreateDatabaseContext();
            db.Execute("Credential_Session_CreateTable");
            db.Execute("Credential_History_CreateTable");
            db.Close();
        }        

        public UserSession Login(User user, string location, string client)
        {
            if (user != null)
            {
                IList<UserSession> sessions = GetSessions(user);
                if (sessions.IsNullOrSizeLessThan(user.MaximumConcurrentLogin))
                {
                    UserSession session = new UserSession() { 
                        Username = user.Username,
                        LoginTime = DateTime.Now,
                        Timeout = user.SessionTimeout,
                        Location = location,
                        ClientAgent = client,
                        LockTimeout = user.LockTimeout,
                        Locked = false
                    };
                    IDBContext db = CreateDatabaseContext();
                    db.Execute("Credential_Session_Create", new
                    {
                        Id = session.Id,
                        Username = session.Username,
                        LoginTime = session.LoginTime,
                        Timeout = session.Timeout,
                        Location = session.Location,
                        ClientAgent = session.ClientAgent,
                        LockTimeout = session.LockTimeout,
                        Locked = session.Locked
                    });
                    db.Close();

                    /* Save Session File */
                    SessionStorage.Save(session);

                    return session;
                }
                else
                {
                    throw new MaximumLoginReachedException();
                }
            }
            return null;
        }
        public void Logout(string sessionId)
        {
            if (!string.IsNullOrEmpty(sessionId))
            {
                IDBContext db = CreateDatabaseContext();
                db.Execute("Credential_Session_Logout", new
                {
                    Id = sessionId,
                    LogoutTime = DateTime.Now
                });
                db.Close();

                SessionStorage.Delete(sessionId);
            }            
        }
        public void Lock(string sessionId)
        {
            if (!string.IsNullOrEmpty(sessionId))
            {
                IDBContext db = CreateDatabaseContext();
                db.Execute("Credential_Session_Lock", new
                {
                    Id = sessionId,
                    LockTime = DateTime.Now
                });
                db.Close();
            }            
        }
        public void Unlock(string sessionId, string username, string password)
        {
            if (!string.IsNullOrEmpty(sessionId))
            {
                User user = UserProvider.IsUserAuthentic(username, password);
                if (!user.IsNull())
                {
                    IDBContext db = CreateDatabaseContext();
                    db.Execute("Credential_Session_Unlock", new
                    {
                        Id = sessionId,
                        UnlockTime = DateTime.Now
                    });
                    db.Close();
                }                
            }
        }

        public IList<UserSession> GetSessions()
        {
            IDBContext db = CreateDatabaseContext();
            IList<UserSession> sessions = db.Fetch<UserSession>("Credential_Session_Select");
            db.Close();
            return sessions;
        }
        public IList<UserSession> GetSessions(long pageNumber, long pageSize)
        {
            IDBContext db = CreateDatabaseContext();
            IPagedData<UserSession> sessions = db.FetchByPage<UserSession>("Credential_Session_Select", pageNumber, pageSize);
            db.Close();
            return sessions.GetData();
        }
        public IList<UserSession> GetSessions(User user)
        {
            if (user != null)
            {
                IDBContext db = CreateDatabaseContext();
                IList<UserSession> sessions = db.Fetch<UserSession>("Credential_Session_Select_ByUsername", new { Username = user.Username });
                db.Close();

                return FetchSessionData(sessions);
            }

            return null;
        }
        public UserSession GetSession(string sessionId)
        {
            if (!string.IsNullOrEmpty(sessionId))
            {
                IDBContext db = CreateDatabaseContext();
                IList<UserSession> sessions = db.Fetch<UserSession>("Credential_Session_Select_ById", new { Id = sessionId });
                db.Close();

                if (sessions.Count > 0)
                {
                    return FetchSessionData(sessions.First());
                }                
            }

            return null;
        }
        public long GetSessionCount()
        {
            IDBContext db = CreateDatabaseContext();
            long count = db.ExecuteScalar<long>("Credential_Session_Count");
            db.Close();

            return count;
        }
        public void RemoveSession(UserSession session)
        {
            if (session != null)
            {
                RemoveSession(session.Id);
            }
        }
        public void RemoveSession(string sessionId)
        {
            if (!string.IsNullOrEmpty(sessionId))
            {
                IDBContext db = CreateDatabaseContext();
                db.Execute("Credential_Session_Remove", new { Id = sessionId });
                db.Close();
            }
        }
        public void SaveSession(UserSession session)
        {
            if (session != null)
            {
                IDBContext db = CreateDatabaseContext();
                db.Execute("Credential_Session_Update", new
                {
                    Id = session.Id,
                    LoginTime = (DateTime?)session.LoginTime,
                    LogoutTime = (DateTime?)session.LogoutTime,
                    Timeout = session.Timeout,
                    Location = session.Location,
                    ClientAgent = session.ClientAgent,
                    LockTimeout = session.LockTimeout,
                    Locked = session.Locked,
                    LockTime = (DateTime?)session.LockTime,
                    UnlockTime = (DateTime?)session.UnlockTime
                });
                db.Close();

                if (!SessionStorage.IsNull())
                {
                    SessionStorage.Save(session);
                }
            }            
        }

        public void SaveHistory(UserSession session)
        {
            if (session != null)
            {
                IDBContext db = CreateDatabaseContext();
                db.Execute("Credential_Session_History_Create", new {
                    Id = session.Id,
                    Username = session.Username,
                    LoginTime = (DateTime?) session.LoginTime,
                    LogoutTime = (DateTime?) session.LogoutTime,
                    Timeout = session.Timeout,
                    Location = session.Location,
                    ClientAgent = session.ClientAgent,
                    LockTimeout = session.LockTimeout,
                    Locked = session.Locked,
                    LockTime = (DateTime?) session.LockTime,
                    UnlockTime = (DateTime?) session.UnlockTime
                });
                db.Close();
            }
        }
        public IList<UserSession> GetHistories(User user)
        {
            if (user != null)
            {
                return GetHistories(user.Username);
            }
            return null;
        }
        public IList<UserSession> GetHistories(User user, long pageNumber, long pageSize)
        {
            if (user != null)
            {
                return GetHistories(user.Username, pageNumber, pageSize);
            }
            return null;
        }
        public IList<UserSession> GetHistories(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                IDBContext db = CreateDatabaseContext();
                IList<UserSession> sessions = db.Fetch<UserSession>("Credential_Session_History_Select_ByUsername", new { Username = username });
                db.Close();

                return sessions;
            }
            return null;
        }
        public IList<UserSession> GetHistories(string username, long pageNumber, long pageSize)
        {
            if (!string.IsNullOrEmpty(username))
            {
                IDBContext db = CreateDatabaseContext();
                IPagedData<UserSession> pagedSessions = db.FetchByPage<UserSession>("Credential_Session_History_Select_ByUsername", pageNumber, pageSize, new { Username = username });
                db.Close();

                return pagedSessions.GetData();
            }
            return null;
        }

        public void Dispose()
        {
            if (DatabaseManager != null)
            {
                DatabaseManager.RemoveSqlLoader(SqlLoader);
            }
        }

        private IDBContext CreateDatabaseContext()
        {
            IDBContext db = DatabaseManager.GetContext(DatabaseContextName);
            if (db != null)
            {
                db.SetExecutionMode(DBContextExecutionMode.ByName);
            }
            return db;
        }

        private ISessionStorage _sessionStorage;
        public ISessionStorage SessionStorage
        {
            get { return _sessionStorage; }
        }

        private IList<UserSession> FetchSessionData(IList<UserSession> sessions)
        {
            IList<UserSession> resultSessions = new List<UserSession>();
            foreach (UserSession session in sessions)
            {
                resultSessions.Add(FetchSessionData(session));
            }
            return resultSessions;
        }
        public UserSession FetchSessionData(UserSession session)
        {
            if(!session.IsNull() && !SessionStorage.IsNull())
            {
                return SessionStorage.Load(session);
            }
            return session;
        }
    }
}
