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

namespace Toyota.Common.Web.Platform
{
    public class PlatformUtilities: BaseRazorExtension
    {
        public PlatformUtilities()
        {
            Page = new PageRazorExtension();
            Authorization = new AuthorizationRazorExtension();
            Application = new ApplicationRazorExtension();
            Util = new UtilitiesRazorExtension();
        }
         
        public PageRazorExtension Page { private set; get; }
        public AuthorizationRazorExtension Authorization { private set; get; }
        public ApplicationRazorExtension Application { private set; get; }
        public UtilitiesRazorExtension Util { private set; get; }

        private HtmlHelper _helper;
        public override HtmlHelper Helper 
        {
            set
            {
                _helper = value;
                Page.Helper = value;
                Authorization.Helper = value;
                Application.Helper = value;
                Util.Helper = value;
            }
            get
            {
                return _helper;
            }
        }

        private string _InitializationCache { set; get; }
        public MvcHtmlString Initialize()
        {            
            if (string.IsNullOrEmpty(_InitializationCache))
            {
                UrlHelper url = new UrlHelper(Helper.ViewContext.RequestContext);
                StringBuilder cacheBuilder = new StringBuilder();
                cacheBuilder.Append("var $tdk = {};");
                cacheBuilder.Append("$tdk.InhouseCallbacks = [];");
                cacheBuilder.Append("$tdk.InhouseProcessCount = 0;");
                cacheBuilder.Append("$tdk.fnRegisterInhouseProcess = function(callback) { $tdk.InhouseCallbacks.push(callback); };");
                cacheBuilder.Append("$tdk.StartupCallbacks = [];");
                cacheBuilder.Append("$tdk.StartupProcessCount = 0;");
                cacheBuilder.Append("$tdk.fnRegisterStartupProcess = function(callback) { $tdk.StartupCallbacks.push(callback); };");

                cacheBuilder.Append(string.Format("$tdk.AppName = '{0}';", Application.Name));
                cacheBuilder.Append(string.Format("$tdk.AppAlias = '{0}';", Application.Alias));
                cacheBuilder.Append(string.Format("$tdk.BaseUrl = '{0}';", Page.BaseUrl));
                cacheBuilder.Append(string.Format("$tdk.HomeUrl = '{0}';", Page.HomeUrl));
                cacheBuilder.Append(string.Format("$tdk.ContentPath = '{0}';", url.Content("~/Content")));

                cacheBuilder.Append(string.Format("$tdk.fnGetControllerUrl = function(controllerName) {{ return '{0}/' + controllerName; }};", Page.BaseUrl));
                cacheBuilder.Append(string.Format("$tdk.fnGetContentPath = function(path) {{ return '{0}/' + path; }};", url.Content("~/Content")));
                cacheBuilder.Append(string.Format("$tdk.fnGetActionUrl = function(controllerName, actionName) {{ return '{0}/' + controllerName + '/' + actionName; }};", Page.BaseUrl));

                if (ApplicationSettings.Instance.Menu.Enabled)
                {
                    cacheBuilder.Append(string.Format("$tdk.Menus = {0};", Page.MenuJsMap));
                }

                _InitializationCache = cacheBuilder.ToString();
            }

            StringBuilder stringBuilder = new StringBuilder("<script type='text/javascript'>");
            stringBuilder.Append(_InitializationCache);
            stringBuilder.Append(string.Format("$tdk.PageTitle = '{0}';", Page.Title));            
            stringBuilder.Append(string.Format("$tdk.ScreenID = '{0}';", Page.ScreenID));
            stringBuilder.Append(string.Format("$tdk.ScreenUrl = '{0}';", Page.ScreenUrl));            
            stringBuilder.Append(string.Format("$tdk.AjaxExtensionUrl = '{0}';", Page.AjaxExtensionUrl)); 
            stringBuilder.Append(string.Format("$tdk.fnGetPageActionUrl = function(actionName) {{ return '{0}/' + actionName; }};", Page.ScreenUrl));

            if (ApplicationSettings.Instance.Security.EnableAuthentication)
            {
                User user = Page.User;
                if (!user.IsNull())
                {
                    stringBuilder.Append("$tdk.User = {};");
                    stringBuilder.Append(string.Format("$tdk.User.Id = '{0}';", user.Id));
                    stringBuilder.Append(string.Format("$tdk.User.RegistrationNumber = '{0}';", user.RegistrationNumber));
                    stringBuilder.Append(string.Format("$tdk.User.Username = '{0}';", user.Username));
                    if (!user.Emails.IsNullOrEmpty())
                    {
                        stringBuilder.Append(string.Format("$tdk.User.Email = '{0}';", user.Emails.First()));
                    }
                }
            }            

            stringBuilder.Append("</script>");
            return new MvcHtmlString(stringBuilder.ToString());
        }
    }
}
