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
    internal class CommandIsUserAuthentic: ServiceCommand
    {
        public const string NAME = "IsUserAuthentic";

        public CommandIsUserAuthentic()
            : base(NAME)
        {
        }

        public override ServiceResult Execute(ServiceParameter parameter)
        {
            ServiceResult result = null;
            if (!parameter.IsNull() && parameter.Parameters.HasKey("username") && parameter.Parameters.HasKey("password"))
            {
                string username = parameter.Parameters.Get<string>("username");
                string password = parameter.Parameters.Get<string>("password");
                if (!username.IsNullOrEmpty() && !password.IsNullOrEmpty())
                {
                    result = new ServiceResult();
                    User user = SSO.Instance.UserProvider.IsUserAuthentic(username, password);
                    if (!user.IsNull())
                    {
                        result.Status = ServiceStatus.Success;
                        result.Data.Add<bool>(NAME, true);
                    }
                    else
                    {
                        result.Status = ServiceStatus.Denied;
                        result.Data.Add<bool>(NAME, false);
                    }
                }
            }
            return result;
        }
    }
}