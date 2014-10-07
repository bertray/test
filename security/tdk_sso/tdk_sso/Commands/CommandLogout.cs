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
            if (!parameter.IsNull() && parameter.Parameters.HasKey("id"))
            {
                string id = parameter.Parameters.Get<string>("id");
                ServiceResult result = new ServiceResult();
                DateTime today = DateTime.Now;
                IDBContext db = SSO.Instance.DatabaseManager.GetContext();
                db.BeginTransaction();
                try
                {
                    IList<LoginModel> logins = db.Fetch<LoginModel>("Login_SelectById", new { Id = id });
                    if (!logins.IsNullOrEmpty())
                    {
                        LoginModel login = logins.First();
                        db.Execute("Login_History_Insert", new
                        {
                            Username = login.Username,
                            LoginTime = login.LoginTime,
                            LogoutTime = today,
                            SessionTimeout = login.SessionTimeout,
                            LockTimeout = login.LockTimeout
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
                return result;
            }
            return null;
        }
    }
}