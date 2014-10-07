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
using Toyota.Common.Database;
using Toyota.Common.Credential;

namespace Toyota.Common.SSO
{
    internal class CommandLogout: ServiceCommand
    {
        public CommandLogout() : base("Logout") { }

        public override ServiceResult Execute(ServiceParameter parameter)
        {
            ServiceResult result = null;
            if (!parameter.IsNull() && parameter.Parameters.HasKey("id"))
            {
                string id = parameter.Parameters.Get<string>("id");
                result = new ServiceResult();
                DateTime today = DateTime.Now;
                IDBContext db = SSO.Instance.DatabaseManager.GetContext();
                db.BeginTransaction();
                try
                {
                    IList<SSOLoginInfo> logins = db.Fetch<SSOLoginInfo>("Login_SelectById", new { Id = id });
                    if (!logins.IsNullOrEmpty())
                    {
                        SSOLoginInfo login = logins.First();
                        db.Execute("Login_History_Insert", new
                        {
                            Id = login.Id,
                            Username = login.Username,                            
                            LoginTime = today,
                            SessionTimeout = login.SessionTimeout,
                            LockTimeout = login.LockTimeout,
                            MaxLogin = login.MaximumLogin,
                            Hostname = login.Hostname,
                            HostIP = login.HostIP,
                            Browser = login.Browser,
                            BrowserVersion = login.BrowserVersion,
                            IsMobile = login.IsMobile
                        });
                        db.Execute("Login_Delete", new { Id = login.Id });
                    }

                    db.CommitTransaction();
                    result.Status = ServiceStatus.Success;
                }
                catch (Exception ex)
                {
                    db.AbortTransaction();
                    throw ex;
                }
                finally
                {
                    db.Close();
                }
            }
            return result;
        }
    }
}