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
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Toyota.Common.Lookup;
using Toyota.Common.Credential;
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Platform
{
    public class SessionAuthentication
    {
        private string screenID;
        private PageDescriptor descriptor;
        public SessionAuthentication(string screenID, PageDescriptor descriptor)
        {
            Enabled = true;
            this.screenID = screenID;
            this.descriptor = descriptor;
        }

        private User _GetSSOUser(HttpContextBase httpContext)
        {
            HttpSessionStateBase session = httpContext.Session;
            ILookup lookup = (ILookup)session.Lookup();
            HttpCookie cookie = httpContext.Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
            if (cookie.IsNull() || string.IsNullOrEmpty(cookie.Value))
            {
                return null;
            }

            string id = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value));
            if (SSOClient.Instance.IsSessionAlive(id))
            {
                ILookup persistedLookup = SSOSessionStorage.Instance.Load(id);
                if (persistedLookup == null)
                {
                    persistedLookup = new SimpleLookup();                    
                }
                persistedLookup.Remove <User>();
                string username = SSOClient.Instance.GetLoggedInUser(id);
                IUserProvider userProvider = EssentialProviders.Instance.UserProvider;
                User user = userProvider.GetUser(username);
                userProvider.FetchAuthorization(user);
                userProvider.FetchOrganization(user);
                userProvider.FetchPlant(user);
                persistedLookup.Add(user);
                SSOSessionStorage.Instance.Save(id, persistedLookup);

                session[SessionKeys.LOOKUP] = persistedLookup;
                SSOSessionLookupListener.RemoveExistingInstance(lookup);
                persistedLookup.AddEventListener(new SSOSessionLookupListener(id));
                lookup = persistedLookup;
                return user;
            }
            else
            {
                SSOSessionStorage.Instance.Delete(id);
                cookie.Expires = DateTime.Now.AddDays(-1);
                httpContext.Response.Cookies.Add(cookie);
                return null;
            }
        }
        
        public void Authenticate(RequestContext requestContext)
        {
            HttpContextBase httpContext = requestContext.HttpContext;
            HttpSessionStateBase session = httpContext.Session;
            ILookup lookup = (ILookup)session.Lookup();
            SecuritySettings settings = ApplicationSettings.Instance.Security;

            if (!settings.EnableAuthentication)
            {
                IsValid = true;
                IsAuthorized = true;
                return;
            }
            else if (!Enabled)
            {
                return;
            }

            User user = lookup.Get<User>();
            if (settings.EnableSingleSignOn)
            {
                user = _GetSSOUser(httpContext);
            }
            if (user.IsNull())
            {                
                if (settings.LoginController.Equals(screenID))
                {
                    IsValid = true;
                    IsAuthorized = true;
                    return;
                }
                else if (settings.SimulateAuthenticatedSession && session.IsNewSession)
                {
                    lookup.Remove<User>();
                    user = settings.SimulatedAuthenticatedUser;
                    lookup.Add(user);
                }

                if (user.IsNull())
                {
                    IsValid = false;
                    IsAuthorized = false;
                    return;
                }
            }
            else
            {
                IsValid = true;
            }

            if (settings.LoginController.Equals(screenID))
            {
                RedirectUrl = descriptor.BaseUrl + "/" + ApplicationSettings.Instance.Runtime.HomeController;                
                IsAuthorized = true;
                return;
            }

            if (settings.IgnoreAuthorization)
            {
                IsAuthorized = true;
                return;
            }

            if (settings.ForgotPasswordController.Equals(screenID))
            {
                IsAuthorized = true;
                return;
            }            

            if(!string.IsNullOrEmpty(settings.UnauthorizedController) && settings.UnauthorizedController.Equals(screenID)) 
            {
                IsAuthorized = true;
                return;
            }

            if (screenID.Equals(ApplicationSettings.Instance.Runtime.HomeController) &&
                screenID.Equals(RuntimeSettings.DEFAULT_HOME_CONTROLLER_NAME))
            {
                IsAuthorized = true;
                return;
            }

            if (screenID.Equals(ApplicationSettings.Instance.Security.LoginController))
            {
                IsAuthorized = true;
                return;
            }

            if (settings.UseCustomAuthorizationRule)
            {
                AuthorizationRuleState resultState = EssentialProviders.Instance.AuthorizationRule.Authorize(requestContext);
                IsAuthorized = resultState.IsAuthorized;
                IsValid = resultState.IsValid;
            }
            else
            {
                AuthorizationFunction authFunc;
                user.Roles.IterateByAction(role => {
                    authFunc = role.Functions.FindElement(func => {
                        return screenID.Equals(func.Id, StringComparison.OrdinalIgnoreCase);
                    });
                    IsAuthorized = !authFunc.IsNull();
                    return !IsAuthorized;
                });
            }            
        }

        public bool Enabled { set; get; }
        public bool IsValid { set; get; }
        public bool IsAuthorized { set; get; }
        public string RedirectUrl { set; get; }
    }
}
