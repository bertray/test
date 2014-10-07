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
    internal class CommandIsUserLocked: ServiceCommand
    {
        public const string NAME = "IsUserLocked";

        public CommandIsUserLocked() : base(NAME) { }

        public override ServiceResult Execute(ServiceParameter parameter)
        {
            ServiceResult result = null;
            IDBContext db = null;
            if (!parameter.IsNull() && parameter.Parameters.HasKey("id"))
            {
                string id = parameter.Parameters.Get<string>("id");
                try
                {
                    DateTime today = DateTime.Now;
                    db = SSO.Instance.DatabaseManager.GetContext();
                    IList<LoginModel> logins = db.Fetch<LoginModel>("Login_SelectById", new { Id = id });
                    if (!logins.IsNullOrEmpty())
                    {
                        LoginModel login = logins.First();
                        result.Data.Add<bool>(NAME, login.Locked);
                    }
                    db.Close();
                    result.Status = ServiceStatus.Success;
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