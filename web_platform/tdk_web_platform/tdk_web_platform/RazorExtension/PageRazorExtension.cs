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
using System.Security.Policy;
using System.IO;
using System.Dynamic;
using System.Web.Routing;
using System.Reflection;

namespace Toyota.Common.Web.Platform
{
    public class PageRazorExtension: BaseRazorExtension
    {
        public MvcHtmlString Title
        {
            get
            {
                return new MvcHtmlString((string) Helper.ViewData["_tdkScreenTitle"]);
            }
        }
        public MvcHtmlString BaseUrl
        {
            get
            {
                return new MvcHtmlString((string)Helper.ViewData["_tdkBaseUrl"]);
            }
        }        
        public MvcHtmlString ScreenID
        {
            get
            {
                return new MvcHtmlString((string)Helper.ViewData["_tdkScreenID"]);
            }
        }
        public MvcHtmlString ControllerName
        {
            get
            {
                return new MvcHtmlString((string)Helper.ViewData["_tdkControllerName"]);
            }
        }
        public MvcHtmlString ScreenUrl
        {
            get
            {
                return new MvcHtmlString(BaseUrl + "/" + ControllerName);
            }
        }
        public MvcHtmlString HomeUrl
        {
            get
            {
                return new MvcHtmlString(BaseUrl + "/" + ApplicationSettings.Instance.Runtime.HomeController);
            }
        }
        public MvcHtmlString LoginUrl
        {
            get
            {
                return new MvcHtmlString(BaseUrl + "/" + ApplicationSettings.Instance.Security.LoginController);
            }
        }
        public MvcHtmlString LoginActionUrl
        {
            get
            {
                return new MvcHtmlString(BaseUrl + "/" + ApplicationSettings.Instance.Security.LoginController + "/Login");
            }
        }
        public MvcHtmlString AjaxLoginActionUrl
        {
            get
            {
                return new MvcHtmlString(BaseUrl + "/" + ApplicationSettings.Instance.Security.LoginController + "/AjaxLogin");
            }
        }
        public MvcHtmlString LogoutActionUrl
        {
            get
            {
                return new MvcHtmlString(BaseUrl + "/" + ApplicationSettings.Instance.Security.LoginController + "/Logout");
            }
        }
        public MvcHtmlString CheckSessionLockActionUrl
        {
            get
            {
                return new MvcHtmlString(BaseUrl + "/" + ApplicationSettings.Instance.Security.LoginController + "/IsLocked");
            }
        }
        public MvcHtmlString SessionUnlockActionUrl
        {
            get
            {
                return new MvcHtmlString(BaseUrl + "/" + ApplicationSettings.Instance.Security.LoginController + "/Unlock");
            }
        }
        public MvcHtmlString SessionLockActionUrl
        {
            get
            {
                return new MvcHtmlString(BaseUrl + "/" + ApplicationSettings.Instance.Security.LoginController + "/Lock");
            }
        }
        public MvcHtmlString ForgotPasswordUrl
        {
            get
            {
                return new MvcHtmlString(BaseUrl + "/" + ApplicationSettings.Instance.Security.ForgotPasswordController);
            }
        }
        public MvcHtmlString AjaxExtensionUrl
        {
            get
            {
                return new MvcHtmlString(ScreenUrl + "/AjExt");
            }
        }

        public MvcHtmlString GetControllerUrl(string controllerName)
        {
            return GetControllerUrl(controllerName, null);
        }
        public MvcHtmlString GetControllerUrl(string controllerName, object routeValues)
        {
            return new MvcHtmlString(routeValues.ToUrlQueryString(BaseUrl + "/" + controllerName));
        }
        public MvcHtmlString GetActionUrl(string controllerName, string actionName, object routeValues)
        {
            return new MvcHtmlString(routeValues.ToUrlQueryString(BaseUrl + "/" + controllerName + "/" + actionName));
        }
        public MvcHtmlString GetActionUrl(string actionName, object routeValues)
        {
            return new MvcHtmlString(routeValues.ToUrlQueryString(ScreenUrl + "/" + actionName));
        }
        public MvcHtmlString GetActionUrl(string actionName)
        {
            return new MvcHtmlString(ScreenUrl + "/" + actionName);
        }
                
        public User User
        {
            get
            {
                User user = (User)Helper.ViewData["_tdkAuthenticatedUser"];
                if(user != null) 
                {
                    return user;
                }
                return null;
            }
        }
        public IList<ScreenMessage> ScreenMessages
        {
            get
            {
                object objMessages = Helper.ViewData["_tdkScreenMessages"];
                if (!objMessages.IsNull())
                {
                    return (IList<ScreenMessage>) objMessages;
                }
                return null;
            }
        }
        public MvcHtmlString SubmitMessage(ScreenMessage[] messages)
        {            
            StringBuilder stringBuilder = new StringBuilder();
            if (!messages.IsNullOrEmpty())
            {
                List<ScreenMessage> messageList = new List<ScreenMessage>();
                messageList.AddRange(messages);
                UrlHelper url = new UrlHelper(Helper.ViewContext.RequestContext);
                stringBuilder.Append(
                    "$.ajax({ " +
                    "   url: '" + AjaxExtensionUrl + "'," +
                    "   type: 'POST'," +
                    "   data: { " +
                    "       name: 'Screen-Message-Submission'," +
                    "       Messages: '" + JSON.ToString<IList<ScreenMessage>>(messageList) + "'" +
                    "   }," +
                    "   success: function() { " +
                    "       $.ajax({ " +
                    "           url: '" + AjaxExtensionUrl + "', " +
                    "           type: 'POST'," +
                    "           data: { " +
                    "               name: 'Screen-Message'" + 
                    "           }," +
                    "           success: function (feedback) {" +
                    "               $('body').tdkMessageDisplayer({" +
                    "                   DisableEyeCandy: false," + 
                    "                   ImageBasePath: '" + url.Content("~/Content/Image/Layout") + "'," +
                    "                   Messages: $.parseJSON(feedback)" + 
                    "               });" +
                    "           }" +
                    "       });" +
                    "   } " +
                    "});"
                );
            }            

            return new MvcHtmlString(stringBuilder.ToString());
        }        
        public MenuList Menu
        {
            get
            {
                object objMenuList = Helper.ViewData["_tdkApplicationMenu"];
                if (objMenuList != null)
                {
                    return (MenuList)objMenuList;
                }
                return null;
            }
        }

        private MenuList _authorizedMenu = null;
        public MenuList AuthorizedMenu
        {
            get
            {
                if (!_authorizedMenu.IsNull())
                {
                    return _authorizedMenu;
                }

                MenuList menu = Menu;
                User user = User;

                if (user.IsNull())
                {
                    return menu;
                }

                if (!menu.IsNull())
                {
                    _authorizedMenu = new MenuList();
                    bool authorized;
                    bool userHasRole = !user.Roles.IsNullOrEmpty();
                    foreach (Menu m in menu)
                    {
                        authorized = false;                        
                        if (m.Roles.IsNullOrEmpty() && !m.IsRestricted)
                        {
                            authorized = true;
                        }
                        else if(userHasRole)
                        {                            
                            foreach (Role role in user.Roles)
                            {
                                authorized |= IsMenuAuthorized(m, role);
                            }                            
                        }
                        authorized |= FilterAuthorizedMenuItem(m, user.Roles);
                        if (authorized)
                        {
                            _authorizedMenu.Add(m);
                        }
                    }
                    return _authorizedMenu;                  
                }
                return menu;
            }
        }
        private bool FilterAuthorizedMenuItem(Menu parent, IList<Role> roles)
        {
            if (!parent.IsNull() && !roles.IsNullOrEmpty() && parent.HasChildren())
            {
                bool authorized;
                bool hasAuthorizedItem = false;
                IList<Menu> unauthorizeds = new List<Menu>();
                foreach (Menu submenu in parent.Children)
                {
                    if (parent.IsRestricted)
                    {
                        submenu.IsRestricted = parent.IsRestricted;
                    }

                    authorized = false;
                    if (submenu.Roles.IsNullOrEmpty() && !parent.IsRestricted)
                    {
                        authorized = true;
                    }
                    else
                    {
                        foreach (Role r in roles)
                        {
                            authorized |= IsMenuAuthorized(submenu, r);
                        }
                    }
                    authorized |= FilterAuthorizedMenuItem(submenu, roles);
                    hasAuthorizedItem |= authorized;
                    if(!authorized)
                    {
                        unauthorizeds.Add(submenu);
                    }
                }

                if (!unauthorizeds.IsNullOrEmpty())
                {
                    foreach (Menu _m in unauthorizeds)
                    {
                        parent.RemoveChildren(_m);
                    }
                }

                return hasAuthorizedItem;
            }

            return false;
        }
        private bool IsMenuAuthorized(Menu menu, Role role)
        {
            if (menu.Roles.IsNullOrEmpty() && !menu.IsRestricted)
            {
                return true;
            }
            if(role.IsNull()) 
            {
                return false;
            }

            string roleId = role.Id;
            bool authorized = false;
            AuthorizationFunction _mfunction;
            AuthorizationFunction _function;
            AuthorizationFeature _mfeature;
            AuthorizationFeature _feature;
            AuthorizationFeatureQualifier _mqualifier;
            AuthorizationFeatureQualifier _qualifier;
            foreach (Role mrole in menu.Roles)
            {
                _mfunction = null;
                _function = null;
                _mfeature = null;
                _feature = null;
                _mqualifier = null;
                _qualifier = null;

                if (!mrole.Id.Equals(roleId))
                {
                    authorized |= false;
                    continue;
                }

                if (mrole.Functions.IsNullOrEmpty())
                {
                    authorized |= true;
                    continue;
                }
                if (role.Functions.IsNullOrEmpty())
                {
                    authorized |= false;
                    continue;
                }

                mrole.Functions.FindAgainst(role.Functions, (tfunc, ofunc) => {
                    return tfunc.Id.StringEquals(ofunc.Id);
                }, (tfunc, ofunc) => { 
                    _mfunction = tfunc;
                    _function = ofunc;
                });
                if (_mfunction.IsNull())
                {
                    authorized |= false;
                    continue;
                }
                
                authorized |= true;

                if (_mfunction.Features.IsNullOrEmpty())
                {
                    continue;
                }
                if (_function.Features.IsNullOrEmpty())
                {
                    authorized = false;
                    continue;
                }

                foreach (AuthorizationFunction func in role.Functions)
                {
                    _mfunction.Features.FindAgainst(func.Features, (tfeat, ofeat) =>
                    {
                        return tfeat.Id.StringEquals(ofeat.Id);
                    }, (tfeat, ofeat) => {
                        _mfeature = tfeat;
                        _feature = ofeat;
                    });
                }
                if (_mfeature.IsNull())
                {
                    authorized = false;
                    continue;
                }
                if (_mfeature.Qualifiers.IsNullOrEmpty())
                {
                    continue;
                }
                if (_feature.Qualifiers.IsNullOrEmpty())
                {
                    authorized = false;
                    continue;
                }

                _mfeature.Qualifiers.FindAgainst(_feature.Qualifiers, (tq, oq) =>
                {
                    return tq.Key.StringEquals(oq.Key) && tq.Qualifier.StringEquals(oq.Qualifier);
                }, (tq, oq) =>
                {
                    _mqualifier = tq;
                    _qualifier = oq;
                });
                if (_mqualifier.IsNull())
                {
                    authorized = false;
                    continue;
                }
                if (!_mqualifier.Qualifier.StringEquals(_qualifier.Qualifier))
                {
                    authorized = false;
                }
            }
            return authorized;
        }
        private dynamic BuildMenuItem(Menu menu)
        {
            dynamic jsMenu = new ExpandoObject();
            jsMenu.Id = menu.Id;
            jsMenu.Text = menu.Text;
            jsMenu.Url = menu.NavigateUrl;
            jsMenu.Visible = Convert.ToString(menu.Visible).ToLower();
            jsMenu.Enabled = Convert.ToString(menu.Enabled).ToLower();            
            jsMenu.Separator = Convert.ToString(menu.Separator).ToLower();
            if (!string.IsNullOrEmpty(menu.Glyph))
            {
                jsMenu.Glyph = menu.Glyph;
            }
            if (!string.IsNullOrEmpty(menu.IconUrl))
            {
                jsMenu.IconUrl = menu.IconUrl;
            }
            if (!string.IsNullOrEmpty(menu.Callback))
            {
                jsMenu.Callback = menu.Callback;
            }
            if (!string.IsNullOrEmpty(menu.OpeningTarget))
            {
                jsMenu.OpeningTarget = menu.OpeningTarget;
            }
            if (!string.IsNullOrEmpty(menu.Description))
            {
                jsMenu.Description = menu.Description;
            }
            return jsMenu;
        }
        private List<dynamic> BuildMenuList(MenuList menuList)
        {
            IList<Menu> menuItems = menuList;
            List<dynamic> jsList = new List<dynamic>();
            dynamic jsMenuItem;
            foreach (Menu menu in menuItems)
            {
                jsMenuItem = BuildMenuItem(menu);
                if (menu.HasChildren())
                {
                    jsMenuItem.Menus = BuildMenuList(menu.Children);
                }
                jsList.Add(jsMenuItem);
            }
            if (jsList.Count > 0)
            {
                return jsList;
            }
            return null;
        }
        public MvcHtmlString MenuJsMap
        {            
            get 
            {
                MenuList menuList = Menu;
                if (!menuList.IsNullOrEmpty())
                {
                    List<dynamic> jsList = BuildMenuList(menuList);
                    IDictionary<string, dynamic> jsMap = new Dictionary<string, dynamic>();
                    jsMap.Add("Items", jsList != null ? jsList : new List<dynamic>());

                    return new MvcHtmlString(JSON.ToString<IDictionary<string, dynamic>>(jsMap));
                }

                return new MvcHtmlString("{}");
            }            
        }        

        public IDictionary<string, string> ForwardedParameters
        {
            get
            {
                return (IDictionary<string, string>)Helper.ViewData["_tdkForwardedParameters"];
            }
        }
        public bool HasForwardedParameter(string key)
        {
            return ForwardedParameters.ContainsKey(key);
        }
        public string GetForwardedParameterValue(string key)
        {
            IDictionary<string, string> paramMap = ForwardedParameters;
            if (paramMap.ContainsKey(key))
            {
                return paramMap[key];
            }
            return null;
        }
                
        public MvcHtmlString SourcePage
        {
            get
            {
                return new MvcHtmlString(ForwardedParameters.ContainsKey("tdkSrcPg") ? ForwardedParameters["tdkSrcPg"] : string.Empty);
            }
        }
        public MvcHtmlString SourcePageUrl
        {
            get
            {
                string sourcePage = SourcePage.ToString();
                if (string.IsNullOrEmpty(sourcePage))
                {
                    sourcePage = ApplicationSettings.Instance.Runtime.HomeController;
                }
                return new MvcHtmlString(BaseUrl + "/" + sourcePage);
            }
        }
        public MvcHtmlString ForwardToAction(string actionName)
        {
            return Forward(ControllerName.ToString(), actionName, param => { });
        }
        public MvcHtmlString ForwardToAction(string actionName, Action<IDictionary<string, object>> paramAction)
        {
            return Forward(ControllerName.ToString(), actionName, paramAction);
        }
        public MvcHtmlString Forward(string controllerName) 
        {
            return Forward(controllerName, param => { });
        }
        public MvcHtmlString Forward(string controllerName, string actionName)
        {
            return Forward(controllerName, actionName, param => { });
        }
        public MvcHtmlString Forward(string controllerName, Action<IDictionary<string, object>> paramAction) 
        {
            return Forward(controllerName, null, paramAction);   
        }
        public MvcHtmlString Forward(string controllerName, string actionName, Action<IDictionary<string, object>> paramAction)
        {
            IDictionary<string, object> param = new Dictionary<string, object>();
            paramAction.Invoke(param);
            param.Add("tdkSrcPg", ControllerName);

            StringBuilder paramBuilder = new StringBuilder();
            foreach (string key in param.Keys)
            {
                paramBuilder.Append(string.Format("_p{0}={1}&", key, Convert.ToString(param[key])));
            }
            paramBuilder.Remove(paramBuilder.Length - 1, 1);

            if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionName))
            {
                paramBuilder.Insert(0, GetActionUrl(controllerName, actionName));
            }
            else if(!string.IsNullOrEmpty(controllerName))
            {
                paramBuilder.Insert(0, string.Format("{0}?", GetControllerUrl(controllerName)));
            }

            return new MvcHtmlString(paramBuilder.ToString());
        }

        public MvcHtmlString PullFileUrl(string controllerName, string section, string name)
        {
            return GetActionUrl(controllerName, "PullFile", new { section = section, name = name });
        }
        public MvcHtmlString PullFileUrl(string section, string name)
        {
            return GetActionUrl("PullFile", new { section = section, name = name });
        }
        public MvcHtmlString PullImageUrl(string controllerName, string section, string name)
        {
            return GetActionUrl(controllerName, "PullImage", new { section = section, name = name });
        }
        public MvcHtmlString PullImageUrl(string section, string name)
        {
            return GetActionUrl("PullImage", new { section = section, name = name });
        }
        public MvcHtmlString PullUserPictureUrl(string controllerName, string name)
        {
            return GetActionUrl(controllerName, "PullUserPicture", new { name = name });
        }
        public MvcHtmlString PullUserPictureUrl(string name)
        {
            return GetActionUrl("PullUserPicture", new { name = name });
        }
    }
}
