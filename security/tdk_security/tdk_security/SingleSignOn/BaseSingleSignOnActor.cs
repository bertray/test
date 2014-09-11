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
    public class BaseSingleSignOnActor: SimpleServiceActor
    {
        public BaseSingleSignOnActor(string name): base(name) { }

        private ISessionProvider sessionProvider;
        public ISessionProvider SessionProvider
        {
            set
            {
                sessionProvider = value;
                IServiceAction[] actions = GetActions();
                if (actions != null)
                {
                    BaseSingleSignOnAction ssoAction;
                    foreach (IServiceAction act in actions)
                    {
                        ssoAction = (BaseSingleSignOnAction)act;
                        ssoAction.SessionProvider = sessionProvider;
                    }
                }                
            }

            get
            {
                return sessionProvider;
            }
        }

        private IUserProvider userProvider;
        public IUserProvider UserProvider
        {
            set
            {
                userProvider = value;
                IServiceAction[] actions = GetActions();
                if (actions != null)
                {
                    BaseSingleSignOnAction ssoAction;
                    foreach (IServiceAction act in actions)
                    {
                        ssoAction = (BaseSingleSignOnAction)act;
                        ssoAction.UserProvider = userProvider;
                    }
                }                
            }
            get
            {
                return userProvider;
            }
        }
    }
}
