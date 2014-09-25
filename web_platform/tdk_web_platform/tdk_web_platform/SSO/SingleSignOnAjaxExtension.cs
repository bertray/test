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
using System.Web.Mvc;
using Toyota.Common.Lookup;
using Toyota.Common.Credential;
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Platform
{
    public class SingleSignOnAjaxExtension: IAjaxExtension
    {
        public string GetName()
        {
            return "SingleSignOn";
        }

        public System.Web.Mvc.ActionResult Execute(System.Web.HttpRequestBase request, System.Web.HttpResponseBase response, System.Web.HttpSessionStateBase session)
        {   
            ILookup lookup = session.Lookup();
            User user = lookup.Get<User>();
            string cmd = request.Params["command"];
            if (!string.IsNullOrEmpty(cmd) && !user.IsNull())
            {                
                if (cmd.Equals("IsUserLocked"))
                {
                    bool locked = SSOClient.Instance.IsUserLocked(user.Username, user.Password);
                    return new ContentResult() { Content = Convert.ToString(locked).ToLower() };
                }
                else if (cmd.Equals("IsUserLoggedIn"))
                {
                    string id = SSOClient.Instance.IsUserLoggedIn(user.Username, user.Password);
                    return new ContentResult() { Content = Convert.ToString(!string.IsNullOrEmpty(id)).ToLower() };
                }
            }

            return new ContentResult() { Content = string.Empty };
        }
    }
}
