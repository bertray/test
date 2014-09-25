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
using Toyota.Common.Utilities;
using System.Web;
using System.Security.Cryptography;

namespace Toyota.Common.Web.Platform
{
    public class LoginPageController: PageController
    {
        [HttpPost]
        public MvcHtmlString AjaxLogin(string username, string password)
        {
            Lookup.Clear();

            if (ApplicationSettings.Instance.Security.SimulateAuthenticatedSession && !ApplicationSettings.Instance.Security.SimulatedAuthenticatedUser.IsNull())
            {
                User dummyUser = ApplicationSettings.Instance.Security.SimulatedAuthenticatedUser;
                string dummyUsername = dummyUser.Username;
                string dummyPassword = dummyUser.Password;
                if (!string.IsNullOrEmpty(dummyUsername) && !string.IsNullOrEmpty(dummyPassword) &&
                    dummyUsername.Equals(username) && dummyPassword.Equals(password))
                {
                    _SaveAuthenticatedUser(dummyUser);
                    return new MvcHtmlString("true");
                }
            }

            IUserProvider userProvider = ProviderRegistry.Instance.Get<IUserProvider>();
            if (userProvider != null)
            {
                User user = userProvider.IsUserAuthentic(username, password);
                if (user != null)
                {
                    userProvider.FetchAuthorization(user);
                    userProvider.FetchOrganization(user);
                    userProvider.FetchPlant(user);

                    _SaveAuthenticatedUser(user);

                    if (ApplicationSettings.Instance.Security.EnableSingleSignOn)
                    {
                        string id = SSOClient.Instance.IsUserLoggedIn(user.Username, user.Password);
                        if (!string.IsNullOrEmpty(id))
                        {
                            SSOClient.Instance.Logout(user.Username, user.Password);
                        }
                        SSOClient.Instance.Login(user.Username, user.Password);

                        //ISessionProvider sessionProvider = ProviderRegistry.Instance.Get<ISessionProvider>();
                        //if (sessionProvider != null)
                        //{
                        //    UserSession session;
                        //    IList<UserSession> sessions = sessionProvider.GetSessions(user);                            
                        //    if (!sessions.IsNullOrEmpty())
                        //    {
                        //        session = sessions.First();
                        //    }
                        //    else
                        //    {
                        //        string location = Request.UserHostAddress + ";" + Request.LogonUserIdentity.Name;
                        //        string agent = Request.UserAgent;
                        //        string hostname = Request.UserHostName;

                        //        session = sessionProvider.Login(user, location, agent);                                
                        //    }

                        //    Lookup.Add(session);
                        //    string encryptedSessionId = HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(session.Id));
                        //    string decryptedSessionId = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(encryptedSessionId));
                        //    HttpCookie cookie = new HttpCookie(GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID, encryptedSessionId);
                        //    cookie.Expires = DateTime.Now.AddMinutes((int)user.SessionTimeout + 5);
                        //    Response.Cookies.Add(cookie);
                        //}
                    }                    

                    return new MvcHtmlString("true");
                }
            }
            return new MvcHtmlString("false");
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            MvcHtmlString feedback = AjaxLogin(username, password);
            if (feedback.ToHtmlString().Equals("true"))
            {
                return Redirect(Descriptor.BaseUrl + "/" + ApplicationSettings.Instance.Runtime.HomeController);
            }
            else
            {
                return Redirect(Descriptor.BaseUrl + "/" + ApplicationSettings.Instance.Security.LoginController);
            }
        }
                
        public MvcHtmlString AjaxLogout()
        {
            User user = Lookup.Get<User>();               
            if (user != null)
            {
                Lookup.Remove<User>();
                if (ApplicationSettings.Instance.Security.EnableSingleSignOn)
                {
                    SSOClient.Instance.Logout(user.Username, user.Password);
                }                

                //ISessionProvider sessionProvider = ProviderRegistry.Instance.Get<ISessionProvider>();
                //if(sessionProvider != null) 
                //{
                //    UserSession session = Lookup.Get<UserSession>();
                //    if (session != null)
                //    {
                //        sessionProvider.Logout(session.Id);
                //        Lookup.Clear();
                //    }                    
                //}                
            }

            //if (Request.Cookies.AllKeys.Contains(GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID))
            //{
            //    HttpCookie cookie = Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
            //    cookie.Expires = DateTime.Now.AddDays(-1);
            //    Response.Cookies.Add(cookie);
            //}

            user = Lookup.Get<User>();
            return new MvcHtmlString((user == null) ? "true" : "false");
        }
                
        public ActionResult Logout()
        {
            MvcHtmlString feedback = AjaxLogout();
            if (feedback.ToHtmlString().Equals("false"))
            {
                return Redirect(Descriptor.BaseUrl + "/" + ApplicationSettings.Instance.Runtime.HomeController);
            }
            else
            {
                return Redirect(Descriptor.BaseUrl + "/" + ApplicationSettings.Instance.Security.LoginController);
            }
        }

        public MvcHtmlString IsLocked()
        {
            HttpCookie cookie = Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
            if (cookie != null)
            {
                ISessionProvider sessionProvider = ProviderRegistry.Instance.Get<ISessionProvider>();
                if (sessionProvider != null)
                {
                    UserSession session = sessionProvider.GetSession(Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value)));
                    if ((session != null) && session.Locked)
                    {

                        return new MvcHtmlString("true");
                    }
                }
            }
            return new MvcHtmlString("false");
        }

        public MvcHtmlString Unlock(string username, string password)
        {
            HttpCookie cookie = Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
            if (cookie != null)
            {
                ISessionProvider sessionProvider = ProviderRegistry.Instance.Get<ISessionProvider>();
                if (sessionProvider != null)
                {
                    UserSession session = sessionProvider.GetSession(Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value)));
                    if ((session != null) && session.Locked)
                    {
                        sessionProvider.Unlock(session.Id, username, password);
                        session = sessionProvider.GetSession(Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value)));
                        if (!session.Locked)
                        {
                            Lookup.Remove<UserSession>();
                            Lookup.Add(session);
                            return new MvcHtmlString("true");
                        }                        
                    }
                }
            }
            return new MvcHtmlString("false");
        }

        public MvcHtmlString Lock()
        {
            HttpCookie cookie = Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
            if (cookie != null)
            {
                ISessionProvider sessionProvider = ProviderRegistry.Instance.Get<ISessionProvider>();
                if (sessionProvider != null)
                {
                    UserSession session = sessionProvider.GetSession(Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value)));
                    if ((session != null) && !session.Locked)
                    {
                        sessionProvider.Lock(session.Id);
                        session = sessionProvider.GetSession(Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value)));
                        if (session.Locked)
                        {
                            Lookup.Remove<UserSession>();
                            Lookup.Add(session);
                            return new MvcHtmlString("true");
                        }
                    }
                }
            }
            return new MvcHtmlString("false");
        }

        protected override void Startup()
        {
            Settings.Title = "Please Login";
        }    

        private void _SaveAuthenticatedUser(User user)
        {
            Lookup.Add(user);
            if (ApplicationSettings.Instance.Security.EnableSingleSignOn)
            {
                Session.Timeout = 5;
            }
            else
            {
                Session.Timeout = (int) user.SessionTimeout;
            }
        }
    }
}
