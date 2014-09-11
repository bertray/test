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
using Toyota.Common.Credential;
using Toyota.Common.Utilities;

namespace Toyota.Common.Security
{
    public class LockPolicy: ISingleSignOnPolicy
    {
        public bool Apply(IList<Credential.UserSession> sessions, Credential.ISessionProvider sessionProvider, Credential.IUserProvider userProvider)
        {
            User user;
            DateTime now = DateTime.Now;
            TimeSpan timeDifference;
            foreach (UserSession session in sessions)
            {
                if (session.Locked)
                {
                    continue;
                }

                user = userProvider.GetUser(session.Username);
                if (user.IsNull())
                {
                    sessionProvider.RemoveSession(session);
                    continue;
                }

                if (session.UnlockTime != null)
                {
                    timeDifference = now.Subtract(session.UnlockTime.Value);
                }
                else
                {
                    timeDifference = now.Subtract(session.LoginTime);
                }
                
                if (timeDifference.Minutes > user.LockTimeout)
                {
                    sessionProvider.Lock(session.Id);
                }
            }
            return true;
        }
    }
}
