﻿///
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
                        id = SSOClient.Instance.Login(user.Username, user.Password);
                        string encryptedId = HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(id));
                        HttpCookie cookie = new HttpCookie(GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID, encryptedId);
                        cookie.Expires = DateTime.Now.AddMinutes((int)user.SessionTimeout);
                        Response.Cookies.Add(cookie);
                        SSOSessionStorage.Instance.Save(id, Lookup);
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
            bool loggedOut = !user.IsNull();
            if (user != null)
            {
                Lookup.Remove<User>();                
                if (ApplicationSettings.Instance.Security.EnableSingleSignOn)
                {
                    string id = null;
                    if (Request.Cookies.AllKeys.Contains(GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID))
                    {
                        HttpCookie cookie = Request.Cookies[GlobalConstants.Instance.SECURITY_COOKIE_SESSIONID];
                        id = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cookie.Value));
                        cookie.Value = null;
                        cookie.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(cookie);
                    }

                    if (SSOClient.Instance.Logout(user.Username, user.Password))
                    {
                        SSOSessionStorage.Instance.Delete(id);
                    }
                    else
                    {
                        loggedOut = false;
                    }
                }          
            }

            return new MvcHtmlString(loggedOut ? "true" : "false");
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
