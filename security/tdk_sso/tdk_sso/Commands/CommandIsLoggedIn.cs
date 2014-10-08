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
using Toyota.Common.Utilities;
using Toyota.Common.Database;
using Toyota.Common.Credential;

namespace Toyota.Common.SSO
{
    public class CommandIsLoggedIn: ServiceCommand
    {
        public CommandIsLoggedIn(): base("IsLoggedIn") { }

        public override ServiceResult Execute(ServiceParameter parameter)
        {
            ServiceResult result = null;
            if (!parameter.IsNull() &&
                parameter.Parameters.HasKey("username") &&
                parameter.Parameters.HasKey("hostname") &&
                parameter.Parameters.HasKey("host_ip") &&
                parameter.Parameters.HasKey("browser") &&
                parameter.Parameters.HasKey("browser_version") &&
                parameter.Parameters.HasKey("is_mobile"))
            {
                string username = parameter.Parameters.Get<string>("username");
                string hostname = parameter.Parameters.Get<string>("hostname");
                string hostIP = parameter.Parameters.Get<string>("host_ip");
                string browser = parameter.Parameters.Get<string>("browser");
                string browserVersion = parameter.Parameters.Get<string>("browser_version");
                bool isMobile = parameter.Parameters.Get<bool>("is_mobile");

                IDBContext db = null;
                try
                {
                    DateTime today = DateTime.Now;
                    db = SSO.Instance.DatabaseManager.GetContext();

                    IList<SSOLoginInfo> infos = db.Fetch<SSOLoginInfo>("Login_SelectLoggedIn", new
                    {
                        Username = username,
                        Hostname = hostname,
                        HostIP = hostIP,
                        Browser = browser,
                        BrowserVersion = browserVersion,
                        IsMobile = isMobile
                    });
                    if (!infos.IsNullOrEmpty())
                    {
                        result = new ServiceResult();
                        result.Data.Add<string>("id", infos.First().Id);
                        result.Status = ServiceStatus.Confirmed;
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
