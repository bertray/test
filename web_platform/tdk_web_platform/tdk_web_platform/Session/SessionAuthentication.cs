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

        private bool IsSSOApplicable(HttpContextBase httpContext)
        {
            HttpSessionStateBase session = httpContext.Session;
            ILookup lookup = (ILookup)session.Lookup();
            HttpCookie cookie = httpContext.Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
            if (cookie.IsNull() || string.IsNullOrEmpty(cookie.Value))
            {
                return false;
            }

            string id = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value));
            if (SSOClient.Instance.IsSessionAlive(id))
            {
                ILookup persistedLookup = SSOSessionStorage.Instance.Load(id);
                if (persistedLookup == null)
                {
                    persistedLookup = new SimpleLookup();
                    string username = SSOClient.Instance.GetLoggedInUser(id);
                    IUserProvider userProvider = ProviderRegistry.Instance.Get<IUserProvider>();
                    persistedLookup.Add(userProvider.GetUser(username));
                    SSOSessionStorage.Instance.Save(id, persistedLookup);
                }
                session[SessionKeys.LOOKUP] = persistedLookup;
                SSOSessionLookupListener.RemoveExistingInstance(lookup);
                persistedLookup.AddEventListener(new SSOSessionLookupListener(id));
                lookup = persistedLookup;
                return true;
            }
            else
            {
                SSOSessionStorage.Instance.Delete(id);
                cookie.Expires = DateTime.Now.AddDays(-1);
                httpContext.Response.Cookies.Add(cookie);
                return false;
            }
        }
        
        public void Authenticate(RequestContext requestContext)
        {
            HttpContextBase httpContext = requestContext.HttpContext;
            HttpSessionStateBase session = httpContext.Session;
            ILookup lookup = (ILookup)session.Lookup();

            if (ApplicationSettings.Instance.Security.SimulateAuthenticatedSession)
            {
                if (session.IsNewSession)
                {
                    lookup.Remove<User>();
                    lookup.Add(ApplicationSettings.Instance.Security.SimulatedAuthenticatedUser);
                }
            }

            if (!ApplicationSettings.Instance.Security.EnableAuthentication)
            {
                IsValid = true;
                IsAuthorized = false;
                return;
            }
            
            if (!Enabled)
            {
                return;
            }

            bool loginControllerExecuted = ApplicationSettings.Instance.Security.LoginController.Equals(screenID);
            if (loginControllerExecuted)
            {
                /*if (ApplicationSettings.Instance.Security.EnableSingleSignOn && IsSSOApplicable(httpContext))
                {
                    httpContext.Response.Redirect(descriptor.BaseUrl + "/" + ApplicationSettings.Instance.Runtime.HomeController);
                }*/

                IsValid = true;
                IsAuthorized = true;
                return;
            }

            if (ApplicationSettings.Instance.Security.EnableSingleSignOn && !IsSSOApplicable(httpContext))
            {
                IsValid = false;
                IsAuthorized = false;
                return;
            }

            User user = lookup.Get<User>();
            if (user.IsNull())
            {
                IsValid = false;
                IsAuthorized = false;
                return;            
            }
            IsValid = true;

            if (ApplicationSettings.Instance.Security.IgnoreAuthorization)
            {
                IsAuthorized = true;
                return;
            }
            
            bool forgotPasswordControllerExecuted = ApplicationSettings.Instance.Security.ForgotPasswordController.Equals(screenID);
            bool unauthorizedControllerExecuted = string.IsNullOrEmpty(ApplicationSettings.Instance.Security.UnauthorizedController) ? false : ApplicationSettings.Instance.Security.UnauthorizedController.Equals(screenID);
            bool homeControllerExecuted = ApplicationSettings.Instance.Runtime.HomeController.Equals(screenID);
            if (unauthorizedControllerExecuted || forgotPasswordControllerExecuted || homeControllerExecuted)
            {
                IsValid = true;
                IsAuthorized = true;
                return;
            }            

            if (ApplicationSettings.Instance.Security.UseCustomAuthorizationRule)
            {
                IAuthorizationRule rule = ProviderRegistry.Instance.Get<IAuthorizationRule>();
                if (rule != null)
                {
                    AuthorizationRuleState resultState = rule.Authorize(requestContext);
                    IsAuthorized = resultState.IsAuthorized;
                    IsValid = resultState.IsValid;
                    return;
                }
            }

            AuthorizationFunction authFunc;
            user.Roles.IterateByAction(role => {
                authFunc = role.Functions.FindElement(func => {
                    return screenID.Equals(func.Id, StringComparison.OrdinalIgnoreCase);
                });
                IsAuthorized = !authFunc.IsNull();
                return !IsAuthorized;
            });
        }

        public bool Enabled { set; get; }
        public bool IsValid { set; get; }
        public bool IsAuthorized { set; get; }
    }
}
