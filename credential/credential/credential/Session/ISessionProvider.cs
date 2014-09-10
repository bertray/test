///
/// <author>lufty.abdillah@gmail.com</author>
/// <summary>
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

namespace Toyota.Common.Credential
{
    public interface ISessionProvider: IDisposable
    {        
        UserSession Login(User user, string location, string client);
        void Logout(string sessionId);
        void Lock(string sessionId);
        void Unlock(string sessionId, string username, string password);

        void SaveSession(UserSession session);
        IList<UserSession> GetSessions();
        IList<UserSession> GetSessions(long pageNumber, long pageSize);
        IList<UserSession> GetSessions(User user);        
        UserSession GetSession(string sessionId);
        void RemoveSession(UserSession session);
        void RemoveSession(string sessionId);        
        long GetSessionCount();

        void SaveHistory(UserSession session);
        IList<UserSession> GetHistories(User user);
        IList<UserSession> GetHistories(User user, long pageNumber, long pageSize);
        IList<UserSession> GetHistories(string username);
        IList<UserSession> GetHistories(string username, long pageNumber, long pageSize);

        ISessionStorage SessionStorage { get; }
        UserSession FetchSessionData(UserSession session);
    }
}
