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
using Toyota.Common.Web.Service;
using Toyota.Common.Credential;

namespace Toyota.Common.Security
{
    public abstract class BaseSingleSignOnAction: IServiceAction
    {
        private string name;

        public BaseSingleSignOnAction(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }

        public IUserProvider UserProvider { set; get; }
        public ISessionProvider SessionProvider { set; get; }

        public abstract ServiceResult Execute(ServiceParameters parameters);
    }
}
