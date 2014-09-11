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
using Toyota.Common.Credential;

namespace Toyota.Common.Web.Platform
{
    public abstract class ConsoleController: Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            string username = Request.Params["usr"];
            string password = Request.Params["pwd"];
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                IUserProvider userProvider = ProviderRegistry.Instance.Get<IUserProvider>();
                if (userProvider != null)
                {
                    User user = userProvider.IsUserAuthentic(username, password);
                    if (user != null)
                    {
                        string command = Request.Params["cmd"];
                        return ExecuteCommand(command);
                    }
                }
            }
            return Content(string.Empty);
        }

        protected abstract ActionResult ExecuteCommand(string command);
    }
}
