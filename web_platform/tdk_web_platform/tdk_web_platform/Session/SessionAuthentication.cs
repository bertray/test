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
using Toyota.Common.Lookup;
using Toyota.Common.Credential;
using System.Web.Mvc;

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
            ILookup lookup = (ILookup)session[SessionKeys.LOOKUP];

            IsValid = false;
            IsAuthorized = false;

            if (ApplicationSettings.Instance.Security.EnableAuthentication || ApplicationSettings.Instance.Security.SimulateAuthenticatedSession)
            {
                //if (session.IsNewSession && !ApplicationSettings.Instance.Security.SimulateAuthenticatedSession)
                //{
                //    _CheckSingleSignOn(httpContext);
                //    return;
                //}
                
                if (Enabled)
                {
                    if (ApplicationSettings.Instance.Security.UseCustomAuthenticationRule)
                    {
                        IAuthenticationRule rule = ProviderRegistry.Instance.Get<IAuthenticationRule>();
                        if (rule != null)
                        {
                            AuthenticationRuleState resultState = rule.Authenticate(requestContext);
                            IsAuthorized = resultState.IsAuthorized;
                            IsValid = resultState.IsValid;
                            return;
                        }
                    }

                    if (!_CheckSingleSignOn(httpContext))
                    {                        
                        IsValid = false;
                        return;
                    }

                    bool loginControllerExecuted = ApplicationSettings.Instance.Security.LoginController.Equals(screenID);
                    bool forgotPasswordControllerExecuted = ApplicationSettings.Instance.Security.ForgotPasswordController.Equals(screenID);
                    string unauthorizedController = ApplicationSettings.Instance.Security.UnauthorizedController;
                    bool unauthorizedControllerExecuted = string.IsNullOrEmpty(unauthorizedController) ? false : unauthorizedController.Equals(screenID);
                    bool homeControllerExecuted = ApplicationSettings.Instance.Runtime.HomeController.Equals(screenID);                    
                    if (loginControllerExecuted || unauthorizedControllerExecuted || forgotPasswordControllerExecuted)
                    {
                        IsValid = true;
                        IsAuthorized = true;
                    }
                    else
                    {
                        if (ApplicationSettings.Instance.Security.SimulateAuthenticatedSession)
                        {
                            lookup.Remove<User>();
                            lookup.Add(ApplicationSettings.Instance.Security.SimulatedAuthenticatedUser);
                        }

                        User user = lookup.Get<User>();                        
                        if (user != null)
                        {
                            IsValid = true;
                            IsAuthorized = false;
                            if (!string.IsNullOrEmpty(screenID) && (user.Roles != null) && (user.Roles.Count > 0))
                            {
                                if (homeControllerExecuted)
                                {
                                    IsAuthorized = true;
                                }
                                else
                                {
                                    foreach (Role role in user.Roles)
                                    {
                                        if (role.Functions != null)
                                        {
                                            foreach (AuthorizationFunction function in role.Functions)
                                            {
                                                if (screenID.Equals(function.Id, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    IsAuthorized = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (IsAuthorized)
                                        {
                                            break;
                                        }
                                    }
                                }                                
                            }
                        }
                    }                    
                }
                else
                {
                    IsValid = true;
                }
            }
            else
            {
                IsValid = true;
                IsAuthorized = true;
            }
        }

        public bool Enabled { set; get; }
        public bool IsValid { set; get; }
        public bool IsAuthorized { set; get; }
    }
}
