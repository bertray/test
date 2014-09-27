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

        private bool _CheckSingleSignOn(HttpContextBase httpContext)
        {
            if (!ApplicationSettings.Instance.Security.EnableSingleSignOn || ApplicationSettings.Instance.Security.SimulateAuthenticatedSession)
            {
                return false;
            }
            
            HttpSessionStateBase session = httpContext.Session;
            ILookup lookup = session.Lookup();
            User user = lookup.Get<User>();
            if (user == null)
            {
                HttpCookie cookie = httpContext.Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
                if (cookie != null)
                {
                    string id = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value));
                    if (string.IsNullOrEmpty(id))
                    {
                        httpContext.Response.Redirect(string.Format("{0}/{1}/logout", descriptor.BaseUrl, ApplicationSettings.Instance.Security.LoginController));
                        return false;
                    }
                    else
                    {
                        
                    }
                }                
            }
            else
            {
            }

            return true;
            //ISessionProvider sessionProvider = ProviderRegistry.Instance.Get<ISessionProvider>();
            //if (sessionProvider != null)
            //{
            //    ILookup lookup = (ILookup)session[SessionKeys.LOOKUP];
            //    UserSession userSession = lookup.Get<UserSession>();
            //    if (userSession != null)
            //    {
            //        userSession = sessionProvider.GetSession(userSession.Id);
            //        if (userSession != null)
            //        {
            //            TimeSpan elapsed = DateTime.Now.Subtract(userSession.LoginTime);
            //            IsValid = elapsed.Minutes < userSession.Timeout;
            //        }
            //    }
            //    else
            //    {
            //        HttpCookie cookie = httpContext.Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
            //        if (cookie != null)
            //        {
            //            userSession = sessionProvider.GetSession(Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value)));
            //            IsValid = (userSession != null);
            //            if (IsValid)
            //            {
            //                IUserProvider userProvider = ProviderRegistry.Instance.Get<IUserProvider>();
            //                if (userProvider != null)
            //                {
            //                    User user = userProvider.GetUser(userSession.Username);
            //                    if (user != null)
            //                    {
            //                        lookup.Add(user);
            //                        lookup.Add(userSession);
            //                    }
            //                }
            //            }
            //        }                    
            //    }
            //}
        }

        public void Authenticate(RequestContext requestContext)
        {
            HttpContextBase httpContext = requestContext.HttpContext;
            HttpSessionStateBase session = httpContext.Session;
            ILookup lookup = (ILookup)session.Lookup();

            if (!ApplicationSettings.Instance.Security.EnableAuthentication)
            {
                IsValid = true;
                IsAuthorized = false;
                return;
            }
            if (ApplicationSettings.Instance.Security.SimulateAuthenticatedSession)
            {
                if (session.IsNewSession)
                {
                    lookup.Remove<User>();
                    lookup.Add(ApplicationSettings.Instance.Security.SimulatedAuthenticatedUser);
                }
            }
            if (!Enabled)
            {
                return;
            }

            bool loginControllerExecuted = ApplicationSettings.Instance.Security.LoginController.Equals(screenID);
            if (loginControllerExecuted)
            {
                IsValid = true;
                IsAuthorized = true;
                return;
            }

            if (ApplicationSettings.Instance.Security.EnableSingleSignOn)
            {
                HttpCookie cookie = httpContext.Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
                if (cookie.IsNull() || string.IsNullOrEmpty(cookie.Value))
                {
                    IsValid = false;
                    IsAuthorized = false;
                    return;
                }

                string id = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value));
                if (SSOClient.Instance.IsSessionAlive(id))
                {
                    ILookup persistedLookup = SSOSessionStorage.Instance.Load(id);
                    session[SessionKeys.LOOKUP] = persistedLookup;
                    SSOSessionLookupListener.RemoveExistingInstance(lookup);
                    persistedLookup.AddEventListener(new SSOSessionLookupListener(id));
                    lookup = persistedLookup;
                }
                else
                {
                    SSOSessionStorage.Instance.Delete(id);
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    httpContext.Response.Cookies.Add(cookie);
                    IsValid = false;
                    IsAuthorized = false;
                    return;
                }
            }
            else
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
