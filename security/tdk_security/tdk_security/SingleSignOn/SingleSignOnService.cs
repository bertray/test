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
using Toyota.Common.Web.Service;
using System.ServiceModel;
using Toyota.Common.Credential;
using Toyota.Common.Utilities;

namespace Toyota.Common.Security
{
    public class SingleSignOnService: StreamedWebService
    {
        public SingleSignOnService()
        {
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.GetUser, data => { return GetUser(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.AuthenticateUser, data => { return IsUserAuthentic(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.GetUserCount, data => { return GetUserCount(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.GetUsers, data => { return GetUsers(data.Parameters); }));
            //Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.GetUserByName, data => { return GetUserByName(data.Parameters); }));
            //Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.GetUserByFirstName, data => { return GetUserByFirstName(data.Parameters); }));
            //Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.GetUserByLastName, data => { return GetUserByLastName(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.FetchAuthorization, data => { return FetchAuthorization(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.FetchOrganization, data => { return FetchOrganization(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.FetchPlant, data => { return FetchPlant(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.Login, data => { return Login(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.Logout, data => { return Logout(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.Lock, data => { return Lock(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.Unlock, data => { return Unlock(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.GetSessions, data => { return GetSessions(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.GetSession, data => { return GetSession(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.RemoveSession, data => { return RemoveSession(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.UpdateSession, data => { return UpdateSession(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.SaveSessionHistory, data => { return SaveSessionHistory(data.Parameters); }));
            Commands.AddCommand(new StreamedActionServiceCommand(SingleSignOnServiceCommand.GetSessionHistories, data => { return GetSessionHistories(data.Parameters); }));
        }

        protected IUserProvider UserProvider { set; get; }
        protected ISessionProvider SessionProvider { set; get; }

        private StreamedServiceResult IsUserAuthentic(JsonDataMap data) {
            if((UserProvider != null) && data.HasKey(SingleSignOnServiceParameter.Username) && data.HasKey(SingleSignOnServiceParameter.Password))
            {
                string username = data.Get<string>(SingleSignOnServiceParameter.Username);
                string password = data.Get<string>(SingleSignOnServiceParameter.Password);
                return new StreamedServiceResult(result =>
                {
                    result.Add<User>(SingleSignOnServiceParameter.User, UserProvider.IsUserAuthentic(username, password));
                });
            }
            return null;
        }
        private StreamedServiceResult GetUser(JsonDataMap data)
        {
            if ((UserProvider != null) && data.HasKey(SingleSignOnServiceParameter.Username))
            {
                string username = data.Get<string>(SingleSignOnServiceParameter.Username);
                User user = UserProvider.GetUser(username);
                if (user != null)
                {
                    return new StreamedServiceResult(result =>
                    {
                        result.Add<User>(SingleSignOnServiceParameter.User, user);
                    });
                }
            }
            return null;
        }
        private StreamedServiceResult GetUserCount(JsonDataMap data)
        {
            if (UserProvider != null)
            {
                long count = UserProvider.GetUserCount();
                return new StreamedServiceResult(resultData => {
                    resultData.Add<long>(SingleSignOnServiceParameter.UserCount, count);
                });
            }
            return null;
        }
        private StreamedServiceResult GetUsers(JsonDataMap data)
        {
            if (UserProvider != null)
            {
                IList<User> users;
                if (data.HasKey(SingleSignOnServiceParameter.PageSize) && data.HasKey(SingleSignOnServiceParameter.PageNumber))
                {
                    long pageSize = data.Get<long>(SingleSignOnServiceParameter.PageSize);
                    long pageNumber = data.Get<long>(SingleSignOnServiceParameter.PageNumber);
                    users = UserProvider.GetUsers(pageNumber, pageSize);
                }
                else
                {
                    users = UserProvider.GetUsers();                    
                }

                if (!users.IsNullOrEmpty())
                {
                    return new StreamedServiceResult(resultData =>
                    {
                        resultData.Add<IList<User>>(SingleSignOnServiceParameter.Users, users);
                    });
                }
            }
            return null;
        }
        //private StreamedServiceResult GetUserByName(JsonDataMap data)
        //{
        //    if ((UserProvider != null) && data.HasKey(SingleSignOnServiceParameter.Name))
        //    {
        //        IList<User> users;
        //        string name = data.Get<string>(SingleSignOnServiceParameter.Name);                
        //        if (data.HasKey(SingleSignOnServiceParameter.PageSize) && data.HasKey(SingleSignOnServiceParameter.PageNumber))
        //        {
        //            long pageSize = data.Get<long>(SingleSignOnServiceParameter.PageSize);
        //            long pageNumber = data.Get<long>(SingleSignOnServiceParameter.PageNumber);
        //            users = UserProvider.GetUserByName(name, pageNumber, pageSize);
        //        }
        //        else
        //        {
        //            users = UserProvider.GetUserByName(name);
        //        }

        //        if (!users.IsNullOrEmpty())
        //        {
        //            return new StreamedServiceResult(resultData =>
        //            {
        //                resultData.Add<IList<User>>(SingleSignOnServiceParameter.Users, users);
        //            });
        //        }
        //    }
        //    return null;
        //}
        //private StreamedServiceResult GetUserByFirstName(JsonDataMap data)
        //{
        //    if ((UserProvider != null) && data.HasKey(SingleSignOnServiceParameter.Name))
        //    {
        //        IList<User> users;
        //        string name = data.Get<string>(SingleSignOnServiceParameter.Name);
        //        if (data.HasKey(SingleSignOnServiceParameter.PageSize) && data.HasKey(SingleSignOnServiceParameter.PageNumber))
        //        {
        //            long pageSize = data.Get<long>(SingleSignOnServiceParameter.PageSize);
        //            long pageNumber = data.Get<long>(SingleSignOnServiceParameter.PageNumber);
        //            users = UserProvider.GetUserByFirstName(name, pageNumber, pageSize);
        //        }
        //        else
        //        {
        //            users = UserProvider.GetUserByFirstName(name);
        //        }

        //        if (!users.IsNullOrEmpty())
        //        {
        //            return new StreamedServiceResult(resultData =>
        //            {
        //                resultData.Add<IList<User>>(SingleSignOnServiceParameter.Users, users);
        //            });
        //        }
        //    }
        //    return null;
        //}
        //private StreamedServiceResult GetUserByLastName(JsonDataMap data)
        //{
        //    if ((UserProvider != null) && data.HasKey(SingleSignOnServiceParameter.Name))
        //    {
        //        IList<User> users;
        //        string name = data.Get<string>(SingleSignOnServiceParameter.Name);
        //        if (data.HasKey(SingleSignOnServiceParameter.PageSize) && data.HasKey(SingleSignOnServiceParameter.PageNumber))
        //        {
        //            long pageSize = data.Get<long>(SingleSignOnServiceParameter.PageSize);
        //            long pageNumber = data.Get<long>(SingleSignOnServiceParameter.PageNumber);
        //            users = UserProvider.GetUserByLastName(name, pageNumber, pageSize);
        //        }
        //        else
        //        {
        //            users = UserProvider.GetUserByLastName(name);
        //        }

        //        if (!users.IsNullOrEmpty())
        //        {
        //            return new StreamedServiceResult(resultData =>
        //            {
        //                resultData.Add<IList<User>>(SingleSignOnServiceParameter.Users, users);
        //            });
        //        }
        //    }
        //    return null;
        //}
        private StreamedServiceResult FetchAuthorization(JsonDataMap data)
        {
            if ((UserProvider != null) && data.HasKey(SingleSignOnServiceParameter.Users))
            {
                User[] users = data.Get<User[]>(SingleSignOnServiceParameter.Users);
                if (users != null)
                {
                    List<User> userList = new List<User>();
                    userList.AddRange(users);
                    UserProvider.FetchAuthorization(userList);
                    return new StreamedServiceResult(resultData =>
                    {
                        resultData.Add<User[]>(SingleSignOnServiceParameter.Users, userList.ToArray());
                    });
                }                
            }
            return null;
        }
        private StreamedServiceResult FetchOrganization(JsonDataMap data)
        {
            if ((UserProvider != null) && data.HasKey(SingleSignOnServiceParameter.Users))
            {
                User[] users = data.Get<User[]>(SingleSignOnServiceParameter.Users);
                if (users != null)
                {
                    List<User> userList = new List<User>();
                    userList.AddRange(users);
                    UserProvider.FetchOrganization(userList);
                    return new StreamedServiceResult(resultData =>
                    {
                        resultData.Add<User[]>(SingleSignOnServiceParameter.Users, userList.ToArray());
                    });
                }
            }
            return null;
        }
        private StreamedServiceResult FetchPlant(JsonDataMap data)
        {
            if ((UserProvider != null) && data.HasKey(SingleSignOnServiceParameter.Users))
            {
                User[] users = data.Get<User[]>(SingleSignOnServiceParameter.Users);
                if (users != null)
                {
                    List<User> userList = new List<User>();
                    userList.AddRange(users);
                    UserProvider.FetchPlant(userList);
                    return new StreamedServiceResult(resultData =>
                    {
                        resultData.Add<User[]>(SingleSignOnServiceParameter.Users, userList.ToArray());
                    });
                }
            }
            return null;
        }
        private StreamedServiceResult Login(JsonDataMap data)
        {
            if ((SessionProvider != null) && data.HasKey(SingleSignOnServiceParameter.User) &&
                data.HasKey(SingleSignOnServiceParameter.Location) && data.HasKey(SingleSignOnServiceParameter.Client))
            {
                User user = data.Get<User>(SingleSignOnServiceParameter.User);
                string location = data.Get<string>(SingleSignOnServiceParameter.Location);
                string client = data.Get<string>(SingleSignOnServiceParameter.Client);
                UserSession session = SessionProvider.Login(user, location, client);
                if (session != null)
                {
                    return new StreamedServiceResult(resultData =>
                    { 
                        resultData.Add<UserSession>(SingleSignOnServiceParameter.Session, session);
                    });
                }
            }
            return null;
        }
        private StreamedServiceResult Logout(JsonDataMap data)
        {
            if ((SessionProvider != null) && data.HasKey(SingleSignOnServiceParameter.Id))
            {
                string id = data.Get<string>(SingleSignOnServiceParameter.Id);
                UserSession session = SessionProvider.GetSession(id);
                if (session != null)
                {
                    SessionProvider.Logout(id);
                    session = SessionProvider.GetSession(id);
                    SessionProvider.SaveHistory(session);
                    SessionProvider.RemoveSession(session);
                }                
            }
            return new StreamedServiceResult();
        }
        private StreamedServiceResult Lock(JsonDataMap data)
        {
            if ((SessionProvider != null) && data.HasKey(SingleSignOnServiceParameter.Id))
            {
                SessionProvider.Lock(data.Get<string>(SingleSignOnServiceParameter.Id));
            }
            return new StreamedServiceResult();
        }
        private StreamedServiceResult Unlock(JsonDataMap data)
        {
            if ((SessionProvider != null) && data.HasKey(SingleSignOnServiceParameter.Id) && data.HasKey(SingleSignOnServiceParameter.Username) && data.HasKey(SingleSignOnServiceParameter.Password))
            {
                SessionProvider.Unlock(data.Get<string>(SingleSignOnServiceParameter.Id), data.Get<string>(SingleSignOnServiceParameter.Username), data.Get<string>(SingleSignOnServiceParameter.Password));
            }
            return new StreamedServiceResult();
        }
        private StreamedServiceResult GetSessions(JsonDataMap data)
        {
            if ((SessionProvider != null))
            {
                IList<UserSession> sessions;
                if (data.HasKey(SingleSignOnServiceParameter.PageNumber) && data.HasKey(SingleSignOnServiceParameter.PageSize))
                {
                    long pageNumber = data.Get<long>(SingleSignOnServiceParameter.PageNumber);
                    long pageSize = data.Get<long>(SingleSignOnServiceParameter.PageSize);
                    sessions = SessionProvider.GetSessions(pageNumber, pageSize);
                }
                else if (data.HasKey(SingleSignOnServiceParameter.User))
                {
                    User user = data.Get<User>(SingleSignOnServiceParameter.User);
                    sessions = SessionProvider.GetSessions(user);
                }
                else
                {
                    sessions = SessionProvider.GetSessions();
                }

                if (sessions != null)
                {
                    return new StreamedServiceResult(resultData =>
                    {
                        resultData.Add<IList<UserSession>>(SingleSignOnServiceParameter.Sessions, sessions);
                    });
                }
            }
            return null;
        }
        private StreamedServiceResult GetSession(JsonDataMap data)
        {
            if((SessionProvider != null) && (data.HasKey(SingleSignOnServiceParameter.Id))) 
            {
                string id = data.Get<string>(SingleSignOnServiceParameter.Id);
                UserSession session = SessionProvider.GetSession(id);
                if (session != null)
                {
                    return new StreamedServiceResult(resultData =>
                    {
                        resultData.Add<UserSession>(SingleSignOnServiceParameter.Session, session);
                    });
                }
            }
            return null;
        }
        private StreamedServiceResult RemoveSession(JsonDataMap data)
        {
            if ((SessionProvider != null) && (data.HasKey(SingleSignOnServiceParameter.Id)))
            {
                SessionProvider.RemoveSession(data.Get<string>(SingleSignOnServiceParameter.Id));
            }
            return new StreamedServiceResult();
        }
        private StreamedServiceResult UpdateSession(JsonDataMap data)
        {
            if ((SessionProvider != null) && (data.HasKey(SingleSignOnServiceParameter.Session)))
            {
                UserSession session = data.Get<UserSession>(SingleSignOnServiceParameter.Session);
                SessionProvider.SaveSession(session);
            }
            return new StreamedServiceResult();
        }
        private StreamedServiceResult SaveSessionHistory(JsonDataMap data)
        {
            if ((SessionProvider != null) && (data.HasKey(SingleSignOnServiceParameter.Session)))
            {
                UserSession session = data.Get<UserSession>(SingleSignOnServiceParameter.Session);
                SessionProvider.SaveHistory(session);
            }
            return new StreamedServiceResult();
        }
        private StreamedServiceResult GetSessionHistories(JsonDataMap data)
        {
            if ((SessionProvider != null) && data.HasKey(SingleSignOnServiceParameter.Username))
            {
                IList<UserSession> sessions;
                string username = data.Get<string>(SingleSignOnServiceParameter.Username);
                if (data.HasKey(SingleSignOnServiceParameter.PageNumber) && data.HasKey(SingleSignOnServiceParameter.PageSize)) 
                {
                    long pageNumber = data.Get<long>(SingleSignOnServiceParameter.PageNumber);
                    long pageSize = data.Get<long>(SingleSignOnServiceParameter.PageSize);
                    sessions = SessionProvider.GetHistories(username, pageNumber, pageSize);                    
                } 
                else 
                {
                    sessions = SessionProvider.GetHistories(username);
                }

                if(sessions != null) 
                {
                    return new StreamedServiceResult(resultData =>
                    {
                        resultData.Add<IList<UserSession>>(SingleSignOnServiceParameter.SessionHistories, sessions);
                    });
                }
            }
            return null;
        }        
    }
}
