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
using System.Web;
using Toyota.Common.Web.Service;
using Toyota.Common.Utilities;
using Toyota.Common.Credential;
using Toyota.Common.Database;

namespace Toyota.Common.SSO
{
    internal class CommandLogin: ServiceCommand
    {
        public CommandLogin() : base("Login") { }

        public override ServiceResult Execute(ServiceParameter parameter)
        {
            ServiceResult result = null;
            if (!parameter.IsNull() && parameter.Parameters.HasKey("username") && parameter.Parameters.HasKey("password"))
            {
                string username = parameter.Parameters.Get<string>("username");
                string password = parameter.Parameters.Get<string>("password");
                IDBContext db = null;
                try
                {
                    User user = SSO.Instance.UserProvider.IsUserAuthentic(username, password);
                    result = new ServiceResult();
                    if (!user.IsNull())
                    {
                        DateTime today = DateTime.Now;
                        db = SSO.Instance.DatabaseManager.GetContext();

                        string sessionId = Guid.NewGuid().ToString();
                        db.Execute("Login_Insert", new
                        {
                            Username = username,
                            Id = sessionId,
                            LoginTime = today,
                            SessionTimeout = user.SessionTimeout,
                            LockTimeout = user.LockTimeout,
                            MaxLogin = user.MaximumConcurrentLogin
                        });
                        result.Data.Add("Id", sessionId);
                        result.Status = ServiceStatus.Success;
                    }
                    else
                    {
                        result.Status = ServiceStatus.Denied;
                    }
                }
                finally
                {
                    if (!db.IsNull())
                    {
                        db.Close();
                    }
                }                
                
                return result;
            }
            return null;
        }
    }
}