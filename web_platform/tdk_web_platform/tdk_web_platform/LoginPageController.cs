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
                        string id = SSOClient.Instance.IsLoggedIn(
                            user.Username, 
                            Request.UserHostName, Request.UserHostAddress, 
                            Request.Browser.Browser, Request.Browser.Version, 
                            Request.Browser.IsMobileDevice

                        );
                        if (!string.IsNullOrEmpty(id))
                        {
                            SSOClient.Instance.Logout(id);
                        }
                        id = SSOClient.Instance.Login(
                            user.Username, user.Password,
                            Request.UserHostName, Request.UserHostAddress,
                            Request.Browser.Browser, Request.Browser.Version,
                            Request.Browser.IsMobileDevice
                        );
                        if (string.IsNullOrEmpty(id))
                        {
                            Lookup.Remove<User>();
                            return new MvcHtmlString("false");
                        }
                        string encryptedId = HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(id));
                        HttpCookie cookie = Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
                        if (cookie.IsNull())
                        {
                            cookie = new HttpCookie(GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID, encryptedId);
                        }
                        else
                        {
                            cookie.Value = encryptedId;
                        }
                        cookie.Expires = DateTime.Now.AddMinutes((int)user.SessionTimeout * 2);
                        Response.Cookies.Add(cookie);
                        SSOSessionStorage.Instance.Save(id, Lookup);
                        SSOSessionLookupListener.RemoveExistingInstance(Lookup);
                        Lookup.AddEventListener(new SSOSessionLookupListener(id));
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
            if (!ApplicationSettings.Instance.Security.EnableAuthentication ||
                ApplicationSettings.Instance.Security.SimulateAuthenticatedSession)
            {
                return new MvcHtmlString("false");
            }

            User user = Lookup.Get<User>();
            bool loggedOut = !user.IsNull();
            if (loggedOut)
            {                
                if (ApplicationSettings.Instance.Security.EnableSingleSignOn)
                {
                    string id = null;
                    HttpCookie cookie = null;
                    if (Request.Cookies.AllKeys.Contains(GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID))
                    {
                        cookie = Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
                        id = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value));
                        if (!string.IsNullOrEmpty(id))
                        {
                            loggedOut = SSOClient.Instance.Logout(id);
                            if (loggedOut)
                            {
                                cookie.Value = null;
                                cookie.Expires = DateTime.Now.AddDays(-1);
                                Response.Cookies.Add(cookie);
                                SSOSessionStorage.Instance.Delete(id);
                            }
                        }
                        else
                        {
                            loggedOut = false;
                        }
                    }
                }          
            }

            if (loggedOut)
            {
                Lookup.Remove<User>();
                Session.Remove("_tdkAuthorizedApplicationMenu");
            }

            return new MvcHtmlString(loggedOut ? "true" : "false");
        }
                
        public ActionResult Logout()
        {
            if (!ApplicationSettings.Instance.Security.EnableAuthentication ||
                ApplicationSettings.Instance.Security.SimulateAuthenticatedSession)
            {
                return null;
            }

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
            SecuritySettings settings = ApplicationSettings.Instance.Security;
            if (settings.EnableAuthentication && settings.EnableSingleSignOn)
            {
                string id = null;
                if (Request.Cookies.AllKeys.Contains(GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID))
                {
                    HttpCookie cookie = Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
                    id = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value));
                }

                if (!string.IsNullOrEmpty(id))
                {
                    bool locked = SSOClient.Instance.IsLocked(id);
                    return Convert.ToString(locked).ToLower().ToMvcHtmlString();
                }
            }
            return "false".ToMvcHtmlString();
        }

        public MvcHtmlString Lock()
        {
            SecuritySettings settings = ApplicationSettings.Instance.Security;
            if (settings.EnableAuthentication && settings.EnableSingleSignOn)
            {
                string id = null;
                if (Request.Cookies.AllKeys.Contains(GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID))
                {
                    HttpCookie cookie = Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
                    id = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value));
                }

                if (!string.IsNullOrEmpty(id))
                {
                    bool locked = SSOClient.Instance.Lock(id);
                    return Convert.ToString(locked).ToLower().ToMvcHtmlString();
                }
            }
            return "false".ToMvcHtmlString();
        }

        public MvcHtmlString Unlock(string password)
        {
            User user = Lookup.Get<User>();
            if (user.IsNull() || !user.Password.Equals(password))
            {
                return "false".ToMvcHtmlString();
            }

            SecuritySettings settings = ApplicationSettings.Instance.Security;
            if (settings.EnableAuthentication && settings.EnableSingleSignOn)
            {
                string id = null;
                if (Request.Cookies.AllKeys.Contains(GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID))
                {
                    HttpCookie cookie = Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
                    id = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value));
                }

                if (!string.IsNullOrEmpty(id))
                {
                    bool locked = SSOClient.Instance.Unlock(id);
                    return Convert.ToString(locked).ToLower().ToMvcHtmlString();
                }
            }
            return "false".ToMvcHtmlString();
        }

        protected override void Startup()
        {
            if (string.IsNullOrEmpty(Settings.Title))
            {
                Settings.Title = "Please Login";
            }            
        }    

        private void _SaveAuthenticatedUser(User user)
        {
            Lookup.Add(user);
            Session.Timeout = (int)user.SessionTimeout;
            if (ApplicationSettings.Instance.Security.EnableSingleSignOn)
            {
                Session.Timeout = 15;
            }
        }
    }
}
