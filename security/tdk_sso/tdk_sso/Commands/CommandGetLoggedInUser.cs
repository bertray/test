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
    internal class CommandGetLoggedInUser: ServiceCommand
    {
        public const string NAME = "GetLoggedInUser";

        public CommandGetLoggedInUser()
            : base(NAME)
        {
        }

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
                    if (!logins.IsNullOrEmpty())
                    {
                        result.Status = ServiceStatus.Success;
                        result.Data.Add<string>(NAME, logins.First().Username);
                    }
                    else
                    {
                        result.Status = ServiceStatus.Unavailable;
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