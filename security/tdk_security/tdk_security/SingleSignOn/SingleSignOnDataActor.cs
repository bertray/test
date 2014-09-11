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
using Toyota.Common.Credential;

namespace Toyota.Common.Security
{
    public class SingleSignOnDataActor: BaseSingleSignOnActor
    {
        public const string NAME = "Data";
        public const string DATA_KEY_USERS = "Users";
        public const string DATA_KEY_USER = "User";
        public const string DATA_KEY_SESSION = "SessionID";

        public const string PARAMETER_USER = "User";
        public const string PARAMETER_USERNAME = "Username";
        public const string PARAMETER_PASSWORD = "Password";
        public const string PARAMETER_NAME = "Name";
        public const string PARAMETER_FIRST_NAME = "FirstName";
        public const string PARAMETER_LAST_NAME = "LastName";
        public const string PARAMETER_DATA_COUNT = "DataCount";
        public const string PARAMETER_LOCATION = "Location";
        public const string PARAMETER_CLIENT = "Application";
        public const string PARAMETER_SESSION_ID = "SessionID";

        public const string ACTION_FETCH_AUTHORIZATION_BY_NAME = "FetchAuthorization"; 
        public const string ACTION_GET_USERS = "GetUsers";
        public const string ACTION_GET_USER = "GetUser";
        public const string ACTION_GET_USER_BY_NAME = "GetUserByName";
        public const string ACTION_GET_USER_BY_FIRST_NAME = "GetUserByFirstName";
        public const string ACTION_GET_USER_BY_LAST_NAME = "GetUserByLastName";
        public const string ACTION_GET_SESSION = "GetSession";
        public const string ACTION_VALIDATE_USER = "ValidateUser";
        public const string ACTION_LOGIN = "Login";
        public const string ACTION_LOGOUT = "Logout";
        public const string ACTION_LOCK = "Lock";
        public const string ACTION_UNLOCK = "Unlock";        
        
        public SingleSignOnDataActor(): base(NAME)
        {
        }

        public override ServiceResult ExecuteAction(string actionName, ServiceParameters parameters)
        {
            if(string.IsNullOrEmpty(actionName) || (UserProvider == null)) {
                return null;
            }
            
            ServiceResult serviceResult = null;
            if (actionName.Equals(SingleSignOnDataActor.ACTION_FETCH_AUTHORIZATION_BY_NAME))
            {
                User user = (User)parameters.Get(SingleSignOnDataActor.PARAMETER_USER);
                if (user == null)
                {
                    return null;
                }
                serviceResult = new ServiceResult();
                UserProvider.FetchAuthorization(user);
                if (user != null)
                {
                    serviceResult.AddValue(DATA_KEY_USER, user, typeof(User));
                }
            }
            else if (actionName.Equals(SingleSignOnDataActor.ACTION_GET_USERS))
            {
                serviceResult = new ServiceResult();
                serviceResult.AddValue(SingleSignOnDataActor.DATA_KEY_USERS, UserProvider.GetUsers(), typeof(IList<User>));
            }
            else if (actionName.Equals(SingleSignOnDataActor.ACTION_GET_USER))
            {
                string username = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_USERNAME);
                if (string.IsNullOrEmpty(username))
                {
                    return null;
                }
                serviceResult = new ServiceResult();
                User user = UserProvider.GetUser(username);
                if (user != null)
                {
                    serviceResult.AddValue(DATA_KEY_USER, user, typeof(User));
                }
            }
            else if (actionName.Equals(SingleSignOnDataActor.ACTION_GET_USER_BY_NAME))
            {
                string fullName = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_NAME);
                if (string.IsNullOrEmpty(fullName))
                {
                    return null;
                }

                serviceResult = new ServiceResult();
                IList<User> users = UserProvider.GetUserByName(fullName);
                if (users != null)
                {
                    serviceResult.AddValue(DATA_KEY_USERS, users, typeof(IList<User>));
                }
            }
            else if (actionName.Equals(SingleSignOnDataActor.ACTION_GET_USER_BY_FIRST_NAME))
            {
                string firstName = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_FIRST_NAME);
                if (string.IsNullOrEmpty(firstName))
                {
                    return null;
                }

                serviceResult = new ServiceResult();
                IList<User> users = UserProvider.GetUserByFirstName(firstName);
                if (users != null)
                {
                    serviceResult.AddValue(DATA_KEY_USERS, users, typeof(IList<User>));
                }
            }
            else if (actionName.Equals(SingleSignOnDataActor.ACTION_GET_USER_BY_LAST_NAME))
            {
                string lastName = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_LAST_NAME);
                if (string.IsNullOrEmpty(lastName))
                {
                    return null;
                }

                serviceResult = new ServiceResult();
                IList<User> users = UserProvider.GetUserByFirstName(lastName);
                if (users != null)
                {
                    serviceResult.AddValue(DATA_KEY_USERS, users, typeof(IList<User>));
                }
            }
            else if (actionName.Equals(SingleSignOnDataActor.ACTION_VALIDATE_USER))
            {
                string username = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_USERNAME);
                string password = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_PASSWORD);
                if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
                {
                    return null;
                }

                serviceResult = new ServiceResult();
                User user = SessionProvider.Validate(username, password);
                if (user != null)
                {
                    serviceResult.AddValue(DATA_KEY_USER, user, typeof(User));
                }
            }
            else if (actionName.Equals(SingleSignOnDataActor.ACTION_LOGIN))
            {
                string username = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_USERNAME);
                string password = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_PASSWORD);
                string location = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_LOCATION);
                string application = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_CLIENT);
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(application))
                {
                    serviceResult = new ServiceResult();
                    User user = SessionProvider.Login(username, password, location, application);
                    if (user != null)
                    {
                        serviceResult.AddValue(DATA_KEY_USER, user, typeof(User));
                        UserSession session = SessionProvider.GetSession(location, application);
                        serviceResult.AddValue(DATA_KEY_SESSION, session, typeof(UserSession));
                    }
                }
            }
            else if (actionName.Equals(SingleSignOnDataActor.ACTION_LOGOUT))
            {
                string sessionId = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_SESSION_ID);
                if (!string.IsNullOrEmpty(sessionId))
                {
                    SessionProvider.Logout(sessionId);
                }
            }
            else if (actionName.Equals(SingleSignOnDataActor.ACTION_GET_SESSION))
            {
                string location = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_LOCATION);
                string application = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_CLIENT);
                if (!string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(application))
                {
                    serviceResult = new ServiceResult();
                    UserSession session = SessionProvider.GetSession(location, application);
                    if (session != null)
                    {
                        serviceResult.AddValue(DATA_KEY_SESSION, session, typeof(UserSession));
                        serviceResult.AddValue(DATA_KEY_USER, UserProvider.GetUser(session.Username), typeof(User));
                    }
                }
            }
            else if (actionName.Equals(SingleSignOnDataActor.ACTION_LOCK))
            {
                string sessionId = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_SESSION_ID);
                if (!string.IsNullOrEmpty(sessionId))
                {
                    SessionProvider.Lock(sessionId);
                }
            }
            else if (actionName.Equals(SingleSignOnDataActor.ACTION_UNLOCK))
            {
                string sessionId = (string)parameters.Get(SingleSignOnDataActor.PARAMETER_SESSION_ID);
                if (!string.IsNullOrEmpty(sessionId))
                {
                    SessionProvider.Unlock(sessionId);
                }
            }

            return serviceResult;
        }
    }
}
