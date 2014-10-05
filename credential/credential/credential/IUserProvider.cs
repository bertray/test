///
/// <author>lufty.abdillah@gmail.com</author>
/// <summary>
/// Toyota .Net Development Kit
/// Copyright (c) Toyota Motor Manufacturing Indonesia, All Right Reserved.
/// </summary>
/// 
/// <modified>
///     <author>yudha - yudha_hyp@yahoo.com (28-nov-2013)</date> 
///     <summary>
///         ~ add method Fetch Organization
///     </summary>   
///     <author>yudha - yudha_hyp@yahoo.com (6-dec-2013)</date> 
///     <summary>
///         ~ add method Fetch Plant
///     </summary>    
/// </modified>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toyota.Common.Database;
namespace Toyota.Common.Credential
{
    public interface IUserProvider: IDisposable
    {
        void Save(User user);
        void Delete(string username);
        IList<User> GetUsers();
        IList<User> GetUsers(long pageNumber, long pageSize);
        long GetUserCount();
        User GetUser(string username);
        User IsUserAuthentic(string username, string password);

        [Obsolete("This method will be removed soon, please do not use this")]
        void FetchAuthorization(User user);
        [Obsolete("This method will be removed soon, please do not use this")]
        void FetchAuthorization(IList<User> users);
        [Obsolete("This method will be removed soon, please do not use this")]
        void FetchOrganization(User user);
        [Obsolete("This method will be removed soon, please do not use this")]
        void FetchOrganization(IList<User> users);
        [Obsolete("This method will be removed soon, please do not use this")]
        void FetchPlant(User user);
        [Obsolete("This method will be removed soon, please do not use this")]
        void FetchPlant(IList<User> users);

        [Obsolete("This method will be removed soon, please do not use this")]
        IList<User> Search(UserSearchCriteria criteria, object key);
        [Obsolete("This method will be removed soon, please do not use this")]
        IPagedData<User> Search(UserSearchCriteria criteria, long pageNumber, long pageSize, object key);
    }
}
