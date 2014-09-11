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
using Toyota.Common.Lookup;
using Toyota.Common.Credential;
using System.Web;

namespace Toyota.Common.Web.Platform
{
    public class LookupUpdateListener: ILookupEventListener
    {
        public void LookupChanged(LookupEvent evt)
        {
            UpdateToServer(evt.Broadcaster);
        }

        private void UpdateToServer(object param)
        {
            if (ApplicationSettings.Instance.Security.EnableSingleSignOn && ApplicationSettings.Instance.Security.EnableAuthentication)
            {
                ISessionProvider sessionProvider = ProviderRegistry.Instance.Get<ISessionProvider>();
                if (sessionProvider != null)
                {
                    ILookup lookup = (ILookup)param;
                    lookup.IsEventSuppressed = true;
                    UserSession userSession = lookup.Get<UserSession>();                        
                    if (userSession != null)
                    {
                        UserSession _userSession = sessionProvider.GetSession(userSession.Id);
                        if (_userSession != null)
                        {
                            userSession.Locked = _userSession.Locked;
                            userSession.LockTime = _userSession.LockTime;
                            userSession.LogoutTime = _userSession.LogoutTime;
                            userSession.LoginTime = _userSession.LoginTime;
                            userSession.Timeout = _userSession.Timeout;
                            userSession.UnlockTime = _userSession.UnlockTime;
                        }
                        userSession.Data = lookup;
                        sessionProvider.SaveSession(userSession);
                    }
                    lookup.IsEventSuppressed = false;
                }
            }            
        }
    }
}
