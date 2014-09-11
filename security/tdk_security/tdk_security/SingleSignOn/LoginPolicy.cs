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
using Toyota.Common.Utilities;
using Toyota.Common.Credential;

namespace Toyota.Common.Security
{
    public class LoginPolicy: ISingleSignOnPolicy
    {
        public bool Apply(IList<UserSession> sessions, ISessionProvider sessionProvider, IUserProvider userProvider)
        {
            User user;
            UserSession session;
            DateTime now = DateTime.Now;
            TimeSpan timeDifference;
            foreach (UserSession s in sessions)
            {
                if (s.LoginTime.IsNull())
                {
                    continue;
                }

                user = userProvider.GetUser(s.Username);
                if (user.IsNull())
                {
                    sessionProvider.RemoveSession(s);
                    continue;
                }

                timeDifference = now.Subtract(s.LoginTime);
                if (timeDifference.Minutes > user.SessionTimeout)
                {
                    sessionProvider.Logout(s.Id);
                    session = sessionProvider.GetSession(s.Id);
                    sessionProvider.RemoveSession(session);
                    sessionProvider.SaveHistory(session);
                }
            }

            return true;
        }
    }
}
