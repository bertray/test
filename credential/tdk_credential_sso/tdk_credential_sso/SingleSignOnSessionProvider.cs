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
using System.ServiceModel;
using Toyota.Common.Web.Service;
using System.ServiceModel.Channels;
using Toyota.Common.Security;

namespace Toyota.Common.Credential.SingleSignOn
{
    public class SingleSignOnSessionProvider: ISessionProvider
    {
        private ServiceClientFactory<IStreamedWebService> ServiceFactory { set; get; }

        public SingleSignOnSessionProvider(string url) : this(url, null) { }
        public SingleSignOnSessionProvider(string url, Binding binding)
        {
            if (binding == null)
            {
                BasicHttpBinding serviceBinding = ServiceBindings.CreateBasicBinding();
                serviceBinding.TransferMode = TransferMode.Streamed;
                serviceBinding.MaxReceivedMessageSize = 2147483647;
                ServiceFactory = new ServiceClientFactory<IStreamedWebService>(url, serviceBinding);
            }
            else
            {
                ServiceFactory = new ServiceClientFactory<IStreamedWebService>(url, binding);
            }
        }

        public UserSession Login(User user, string location, string client)
        {
            UserSession session = null;

            if (user != null)
            {
                StreamedServiceClient serviceClient = new StreamedServiceClient(ServiceFactory.Create());  
                StreamedServiceResult result = serviceClient.Execute(
                    new StreamedServiceParameter(SingleSignOnServiceCommand.Login, param =>
                    {
                        param.Add<User>(SingleSignOnServiceParameter.User, user);
                        param.Add<string>(SingleSignOnServiceParameter.Location, location);
                        param.Add<string>(SingleSignOnServiceParameter.Client, client);
                    })
                );

                if (result != null)
                {
                    session = result.Data.Get<UserSession>(SingleSignOnServiceParameter.Session);
                }
                serviceClient.Dispose();
            }

            return session;
        }
        public void Logout(string sessionId)
        {
            StreamedServiceClient serviceClient = new StreamedServiceClient(ServiceFactory.Create());  
            serviceClient.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.Logout, param =>
                {
                    param.Add<string>(SingleSignOnServiceParameter.Id, sessionId);
                })
            );
            serviceClient.Dispose();
        }
        public void Lock(string sessionId)
        {
            StreamedServiceClient serviceClient = new StreamedServiceClient(ServiceFactory.Create());
            serviceClient.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.Lock, param =>
                {
                    param.Add<string>(SingleSignOnServiceParameter.Id, sessionId);
                })
            );
            serviceClient.Dispose();
        }
        public void Unlock(string sessionId, string username, string password)
        {
            StreamedServiceClient serviceClient = new StreamedServiceClient(ServiceFactory.Create());  
            serviceClient.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.Unlock, param =>
                {
                    param.Add<string>(SingleSignOnServiceParameter.Id, sessionId);
                    param.Add<string>(SingleSignOnServiceParameter.Username, username);
                    param.Add<string>(SingleSignOnServiceParameter.Password, password);
                })
            );
            serviceClient.Dispose();
        }
        public IList<UserSession> GetSessions()
        {
            IList<UserSession> sessions = null;

            StreamedServiceClient serviceClient = new StreamedServiceClient(ServiceFactory.Create());  
            StreamedServiceResult result = serviceClient.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.GetSessions)
            );

            if (result != null)
            {
                sessions = result.Data.Get<IList<UserSession>>(SingleSignOnServiceParameter.Sessions);
            }
            serviceClient.Dispose();

            return sessions;
        }
        public IList<UserSession> GetSessions(long pageNumber, long pageSize)
        {
            IList<UserSession> sessions = null;

            StreamedServiceClient serviceClient = new StreamedServiceClient(ServiceFactory.Create());  
            StreamedServiceResult result = serviceClient.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.GetSessions, param => {
                    param.Add<long>(SingleSignOnServiceParameter.PageNumber, pageNumber);
                    param.Add<long>(SingleSignOnServiceParameter.PageSize, pageSize);
                })
            );

            if (result != null)
            {
                sessions = result.Data.Get<IList<UserSession>>(SingleSignOnServiceParameter.Sessions);
            }
            serviceClient.Dispose();

            return sessions;
        }
        public IList<UserSession> GetSessions(User user)
        {
            IList<UserSession> sessions = null;

            if (user != null)
            {
                StreamedServiceClient serviceClient = new StreamedServiceClient(ServiceFactory.Create());  
                StreamedServiceResult result = serviceClient.Execute(
                    new StreamedServiceParameter(SingleSignOnServiceCommand.GetSessions, param =>
                    {
                        param.Add<User>(SingleSignOnServiceParameter.User, user);
                    })
                );

                if (result != null)
                {
                    sessions = result.Data.Get<IList<UserSession>>(SingleSignOnServiceParameter.Sessions);
                }
                serviceClient.Dispose();
            }            

            return sessions;
        }
        public UserSession GetSession(string sessionId)
        {
            UserSession session = null;

            if (!string.IsNullOrEmpty(sessionId))
            {
                StreamedServiceClient serviceClient = new StreamedServiceClient(ServiceFactory.Create());  
                StreamedServiceResult result = serviceClient.Execute(
                    new StreamedServiceParameter(SingleSignOnServiceCommand.GetSession, param =>
                    {
                        param.Add<string>(SingleSignOnServiceParameter.Id, sessionId);
                    })
                );

                if (result != null)
                {
                    session = result.Data.Get<UserSession>(SingleSignOnServiceParameter.Session);
                }
                serviceClient.Dispose();
            }            

            return session;
        }
        public long GetSessionCount()
        {
            throw new NotImplementedException();
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
            StreamedServiceClient serviceClient = new StreamedServiceClient(ServiceFactory.Create());  
            serviceClient.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.RemoveSession, param =>
                {
                    param.Add<string>(SingleSignOnServiceParameter.Id, sessionId);
                })
            );
            serviceClient.Dispose();
        }
        public void SaveSession(UserSession session)
        {
            StreamedServiceClient serviceClient = new StreamedServiceClient(ServiceFactory.Create());  
            serviceClient.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.UpdateSession, param =>
                {
                    param.Add<UserSession>(SingleSignOnServiceParameter.Session, session);
                })
            );
            serviceClient.Dispose();
        }
        public void SaveHistory(UserSession session)
        {
            StreamedServiceClient serviceClient = new StreamedServiceClient(ServiceFactory.Create());  
            StreamedServiceResult result = serviceClient.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.SaveSessionHistory, param =>
                {
                    param.Add<UserSession>(SingleSignOnServiceParameter.Session, session);
                })
            );
            serviceClient.Dispose();
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
            IList<UserSession> sessions = null;

            if (!string.IsNullOrEmpty(username))
            {
                StreamedServiceClient serviceClient = new StreamedServiceClient(ServiceFactory.Create());  
                StreamedServiceResult result = serviceClient.Execute(
                    new StreamedServiceParameter(SingleSignOnServiceCommand.GetSessionHistories, param =>
                    {
                        param.Add<string>(SingleSignOnServiceParameter.Username, username);
                    })
                );

                if (result != null)
                {
                    sessions = result.Data.Get<IList<UserSession>>(SingleSignOnServiceParameter.SessionHistories);
                }
                serviceClient.Dispose();
            }

            return sessions;
        }
        public IList<UserSession> GetHistories(string username, long pageNumber, long pageSize)
        {
            IList<UserSession> sessions = null;

            if (!string.IsNullOrEmpty(username))
            {
                StreamedServiceClient serviceClient = new StreamedServiceClient(ServiceFactory.Create());  
                StreamedServiceResult result = serviceClient.Execute(
                    new StreamedServiceParameter(SingleSignOnServiceCommand.GetSessionHistories, param =>
                    {
                        param.Add<string>(SingleSignOnServiceParameter.Username, username);
                        param.Add<long>(SingleSignOnServiceParameter.PageNumber, pageNumber);
                        param.Add<long>(SingleSignOnServiceParameter.PageSize, pageSize);
                    })
                );

                if (result != null)
                {
                    sessions = result.Data.Get<IList<UserSession>>(SingleSignOnServiceParameter.SessionHistories);
                }
                serviceClient.Dispose();
            }

            return sessions;
        }

        public void Dispose()
        {
            if (ServiceFactory != null)
            {
                ServiceFactory.Dispose();
            }
        }

        public ISessionStorage SessionStorage
        {
            get { throw new NotImplementedException(); }
        }

        public UserSession FetchSessionData(UserSession session)
        {
            throw new NotImplementedException();
        }
    }
}
