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

namespace Toyota.Common.SSO
{
    internal class CommandIsAlive: ServiceCommand
    {
        public CommandIsAlive() : base("IsAlive") { }

        public override ServiceResult Execute(ServiceParameter parameter)
        {
            ServiceResult result = null;
            if (!parameter.IsNull() && parameter.Parameters.HasKey("id"))
            {
                string id = parameter.Parameters.Get<string>("id");
                IDBContext db = null;
                try
                {
                    result = new ServiceResult();
                    db = SSO.Instance.DatabaseManager.GetContext();
                    IList<SSOLoginInfo> logins = db.Fetch<SSOLoginInfo>("Login_SelectById", new { Id = id });
                    result.Data.Add<bool>("is_alive", !logins.IsNullOrEmpty());
                    result.Status = ServiceStatus.Confirmed;
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