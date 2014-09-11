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
            ISessionProvider sessionProvider = ProviderRegistry.Instance.Get<ISessionProvider>();

            IDictionary<string, string> resultMap = new Dictionary<string, string>();
            if (!sessionProvider.IsNull())
            {
                const string KEY_STATUS = "status";
                const string KEY_LOCKED = "locked";
                string task = request.Params["task"];
                if (!string.IsNullOrEmpty(task))
                {

                    if (task.Equals("state"))
                    {
                        resultMap.Add(KEY_STATUS, "inactive");
                        UserSession loginSession = lookup.Get<UserSession>();
                        loginSession = sessionProvider.GetSession(loginSession.Id);
                        if (!loginSession.IsNull())
                        {
                            resultMap[KEY_STATUS] = "active";
                        }
                        else
                        {
                            resultMap.Add(KEY_LOCKED, Convert.ToString(loginSession.Locked).ToLower());
                        }
                    }
                }
            }            

            return new ContentResult() { Content = new MvcHtmlString(JSON.ToString<IDictionary<string, string>>(resultMap)).ToHtmlString() };
        }
    }
}
