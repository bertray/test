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
using Toyota.Common.Credential;
using Toyota.Common.Web.Service;
using System.ServiceModel;
using Toyota.Common.Security;
using Toyota.Common.Utilities;
using System.ServiceModel.Channels;
using Toyota.Common.Database;
using System.Xml;

namespace Toyota.Common.Credential.SingleSignOn
{
    public class SingleSignOnUserProvider: IUserProvider
    {
        private ServiceClientFactory<IStreamedWebService> ServiceFactory { set; get; }

        public SingleSignOnUserProvider(string url) : this(url, null) { }
        public SingleSignOnUserProvider(string url, Binding binding)
        {
            if (binding == null)
            {
                BasicHttpBinding serviceBinding = ServiceBindings.CreateBasicBinding();
                serviceBinding.TransferMode = TransferMode.Streamed;
                serviceBinding.MaxReceivedMessageSize = 2147483647;
                serviceBinding.ReaderQuotas.MaxStringContentLength = 2147483647;
                ServiceFactory = new ServiceClientFactory<IStreamedWebService>(url, serviceBinding);
            }
            else
            {
                ServiceFactory = new ServiceClientFactory<IStreamedWebService>(url, binding);
            }
        }

        public IList<User> GetUsers()
        {
            IList<User> resultUsers = null;
            StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
            StreamedServiceResult result = client.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.GetUsers)
            );

            if (result != null)
            {
                resultUsers = result.Data.Get<IList<User>>(SingleSignOnServiceParameter.Users);
            }

            client.Dispose();
            return resultUsers;
        }
        public IList<User> GetUsers(long pageNumber, long pageSize)
        {
            IList<User> resultUsers = null;
            StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
            StreamedServiceResult result = client.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.GetUsers, param => {
                    param.Add<long>(SingleSignOnServiceParameter.PageNumber, pageNumber);
                    param.Add<long>(SingleSignOnServiceParameter.PageSize, pageSize);
                })
            );

            if (result != null)
            {
                resultUsers = result.Data.Get<IList<User>>(SingleSignOnServiceParameter.Users);
            }

            client.Dispose();
            return resultUsers;
        }
        public long GetUserCount()
        {
            long count = 0;
            StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
            StreamedServiceResult result = client.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.GetUserCount)
            );

            if (result != null)
            {
                count = result.Data.Get<long>(SingleSignOnServiceParameter.UserCount);
            }

            client.Dispose();
            return count;
        }
        public User GetUser(string username)
        {
            User user = null;
            StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
            StreamedServiceResult result = client.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.GetUser, param => {
                    param.Add<string>(SingleSignOnServiceParameter.Username, username);
                })
            );

            if (result != null)
            {
                user = result.Data.Get<User>(SingleSignOnServiceParameter.User);
            }

            client.Dispose();
            return user;
        }
        //public IList<User> GetUserByName(string name)
        //{
        //    IList<User> userList = null;
        //    StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
        //    StreamedServiceResult result = client.Execute(
        //        new StreamedServiceParameter(SingleSignOnServiceCommand.GetUserByName, param =>
        //        {
        //            param.Add<string>(SingleSignOnServiceParameter.Name, name);
        //        })
        //    );

        //    if (result != null)
        //    {
        //        userList = result.Data.Get<IList<User>>(SingleSignOnServiceParameter.Users);
        //    }

        //    client.Dispose();
        //    return userList;
        //}
        //public IList<User> GetUserByName(string name, long pageNumber, long pageSize)
        //{
        //    IList<User> userList = null;
        //    StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
        //    StreamedServiceResult result = client.Execute(
        //        new StreamedServiceParameter(SingleSignOnServiceCommand.GetUserByName, param =>
        //        {
        //            param.Add<string>(SingleSignOnServiceParameter.Name, name);
        //            param.Add<long>(SingleSignOnServiceParameter.PageNumber, pageNumber);
        //            param.Add<long>(SingleSignOnServiceParameter.PageSize, pageSize);
        //        })
        //    );

        //    if (result != null)
        //    {
        //        userList = result.Data.Get<IList<User>>(SingleSignOnServiceParameter.Users);
        //    }

        //    client.Dispose();
        //    return userList;
        //}
        //public IList<User> GetUserByFirstName(string firstName)
        //{
        //    IList<User> userList = null;
        //    StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
        //    StreamedServiceResult result = client.Execute(
        //        new StreamedServiceParameter(SingleSignOnServiceCommand.GetUserByFirstName, param =>
        //        {
        //            param.Add<string>(SingleSignOnServiceParameter.Name, firstName);
        //        })
        //    );

        //    if (result != null)
        //    {
        //        userList = result.Data.Get<IList<User>>(SingleSignOnServiceParameter.Users);
        //    }
        //    client.Dispose();
        //    return userList;
        //}
        //public IList<User> GetUserByFirstName(string firstName, long pageNumber, long pageSize)
        //{
        //    IList<User> userList = null;
        //    StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
        //    StreamedServiceResult result = client.Execute(
        //        new StreamedServiceParameter(SingleSignOnServiceCommand.GetUserByFirstName, param =>
        //        {
        //            param.Add<string>(SingleSignOnServiceParameter.Name, firstName);
        //            param.Add<long>(SingleSignOnServiceParameter.PageNumber, pageNumber);
        //            param.Add<long>(SingleSignOnServiceParameter.PageSize, pageSize);
        //        })
        //    );

        //    if (result != null)
        //    {
        //        userList = result.Data.Get<IList<User>>(SingleSignOnServiceParameter.Users);
        //    }

        //    client.Dispose();
        //    return userList;
        //}
        //public IList<User> GetUserByLastName(string lastName)
        //{
        //    IList<User> userList = null;
        //    StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
        //    StreamedServiceResult result = client.Execute(
        //        new StreamedServiceParameter(SingleSignOnServiceCommand.GetUserByLastName, param =>
        //        {
        //            param.Add<string>(SingleSignOnServiceParameter.Name, lastName);
        //        })
        //    );

        //    if (result != null)
        //    {
        //        userList = result.Data.Get<IList<User>>(SingleSignOnServiceParameter.Users);
        //    }
        //    client.Dispose();
        //    return userList;
        //}
        //public IList<User> GetUserByLastName(string lastName, long pageNumber, long pageSize)
        //{
        //    IList<User> userList = null;
        //    StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
        //    StreamedServiceResult result = client.Execute(
        //        new StreamedServiceParameter(SingleSignOnServiceCommand.GetUserByLastName, param =>
        //        {
        //            param.Add<string>(SingleSignOnServiceParameter.Name, lastName);
        //            param.Add<long>(SingleSignOnServiceParameter.PageNumber, pageNumber);
        //            param.Add<long>(SingleSignOnServiceParameter.PageSize, pageSize);
        //        })
        //    );

        //    if (result != null)
        //    {
        //        userList = result.Data.Get<IList<User>>(SingleSignOnServiceParameter.Users);
        //    }

        //    client.Dispose();
        //    return userList;
        //}
        public IPagedData<User> Search(string key, long pageNumber, long pageSize)
        {
            throw new NotImplementedException();
        }
        public User IsUserAuthentic(string username, string password)
        {
            User user = null;
            StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
            StreamedServiceResult result = client.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.AuthenticateUser, param =>
                {
                    param.Add<string>(SingleSignOnServiceParameter.Username, username);
                    param.Add<string>(SingleSignOnServiceParameter.Password, password);
                })
            );

            if (result != null)
            {
                user = result.Data.Get<User>(SingleSignOnServiceParameter.User);
            }
            client.Dispose();
            return user;
        }

        public void FetchAuthorization(User user)
        {
            if (user == null)
            {
                return;
            }

            StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
            StreamedServiceResult result = client.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.FetchAuthorization, param =>
                {
                    param.Add<User[]>(SingleSignOnServiceParameter.Users, new User[] { user });
                })
            );

            if (result != null)
            {
                User[] users = result.Data.Get<User[]>(SingleSignOnServiceParameter.Users);
                if (!users.IsNullOrEmpty())
                {
                    foreach (Role role in users[0].Roles)
                    {
                        user.Roles.Add(role);
                    }                    
                }
            }
            client.Dispose();
        }
        public void FetchAuthorization(IList<User> users)
        {
            if (users.IsNullOrEmpty())
            {
                return;
            }

            StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
            StreamedServiceResult result = client.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.FetchAuthorization, param =>
                {
                    param.Add<User[]>(SingleSignOnServiceParameter.Users, users.ToArray());
                })
            );

            if (result != null)
            {
                User[] resultArray = result.Data.Get<User[]>(SingleSignOnServiceParameter.Users);
                if (!resultArray.IsNullOrEmpty())
                {
                    List<User> resultList = new List<User>(resultArray.Length);
                    resultList.AddRange(resultArray);
                    users = resultList;
                }
            }
            client.Dispose();
        }
        public void FetchOrganization(User user)
        {
            if (user == null)
            {
                return;
            }

            StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
            StreamedServiceResult result = client.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.FetchOrganization, param =>
                {
                    param.Add<User[]>(SingleSignOnServiceParameter.Users, new User[] { user });
                })
            );

            if (result != null)
            {
                User[] users = result.Data.Get<User[]>(SingleSignOnServiceParameter.Users);
                if (!users.IsNullOrEmpty())
                {
                    user = users[0];
                }
            }
            client.Dispose();
        }
        public void FetchOrganization(IList<User> users)
        {
            if (users.IsNullOrEmpty())
            {
                return;
            }

            StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
            StreamedServiceResult result = client.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.FetchOrganization, param =>
                {
                    param.Add<User[]>(SingleSignOnServiceParameter.Users, users.ToArray());
                })
            );

            if (result != null)
            {
                User[] resultArray = result.Data.Get<User[]>(SingleSignOnServiceParameter.Users);
                if (!resultArray.IsNullOrEmpty())
                {
                    List<User> resultList = new List<User>(resultArray.Length);
                    resultList.AddRange(resultArray);
                    users = resultList;
                }
            }
            client.Dispose();
        }
        public void FetchPlant(User user)
        {
            if (user == null)
            {
                return;
            }

            StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
            StreamedServiceResult result = client.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.FetchPlant, param =>
                {
                    param.Add<User[]>(SingleSignOnServiceParameter.Users, new User[] { user });
                })
            );

            if (result != null)
            {
                User[] users = result.Data.Get<User[]>(SingleSignOnServiceParameter.Users);
                if (!users.IsNullOrEmpty())
                {
                    user = users[0];
                }
            }
            client.Dispose();
        }
        public void FetchPlant(IList<User> users)
        {
            if (users.IsNullOrEmpty())
            {
                return;
            }

            StreamedServiceClient client = new StreamedServiceClient(ServiceFactory.Create());    
            StreamedServiceResult result = client.Execute(
                new StreamedServiceParameter(SingleSignOnServiceCommand.FetchPlant, param =>
                {
                    param.Add<User[]>(SingleSignOnServiceParameter.Users, users.ToArray());
                })
            );

            if (result != null)
            {
                User[] resultArray = result.Data.Get<User[]>(SingleSignOnServiceParameter.Users);
                if (!resultArray.IsNullOrEmpty())
                {
                    List<User> resultList = new List<User>(resultArray.Length);
                    resultList.AddRange(resultArray);
                    users = resultList;
                }
            }
            client.Dispose();
        }

        public void Dispose()
        {
            if (ServiceFactory != null)
            {
                ServiceFactory.Dispose();
            }
        }

        public void Delete(string username)
        {
            throw new NotImplementedException();
        }
        public void Save(User user)
        {
            throw new NotImplementedException();
        }
        public void Create(User user)
        {
            throw new NotImplementedException();
        }

        public IList<User> Search(UserSearchCriteria criteria, object key)
        {
            throw new NotImplementedException();
        }
        public IPagedData<User> Search(UserSearchCriteria criteria, long pageNumber, long pageSize, object key)
        {
            throw new NotImplementedException();
        }
    }
}
