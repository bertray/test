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
            if (!parameter.IsNull() && 
                parameter.Parameters.HasKey("username") && 
                parameter.Parameters.HasKey("password") &&
                parameter.Parameters.HasKey("hostname") &&
                parameter.Parameters.HasKey("host_ip") &&
                parameter.Parameters.HasKey("browser") &&
                parameter.Parameters.HasKey("browser_version") &&
                parameter.Parameters.HasKey("is_mobile"))
            {
                string username = parameter.Parameters.Get<string>("username");
                string password = parameter.Parameters.Get<string>("password");
                string hostname = parameter.Parameters.Get<string>("hostname");
                string hostIP = parameter.Parameters.Get<string>("host_ip");
                string browser = parameter.Parameters.Get<string>("browser");
                string browserVersion = parameter.Parameters.Get<string>("browser_version");
                bool isMobile = parameter.Parameters.Get<bool>("is_mobile");

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
                            Id = sessionId,
                            Username = username,                            
                            LoginTime = today,
                            SessionTimeout = user.SessionTimeout,
                            LockTimeout = user.LockTimeout,
                            MaxLogin = user.MaximumConcurrentLogin,
                            Hostname = hostname,
                            HostIP = hostIP,
                            Browser = browser,
                            BrowserVersion = browserVersion,
                            IsMobile = isMobile
                        });
                        result.Data.Add("id", sessionId);
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
            }
            return result;
        }
    }
}