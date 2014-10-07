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
    internal class CommandIsSessionAlive: ServiceCommand
    {
        public const string NAME = "IsSessionAlive";

        public CommandIsSessionAlive() : base(NAME) { }

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
                    result.Data.Add<bool>("IsSessionAlive", !logins.IsNullOrEmpty());
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