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
    public class AuthorizationRazorExtension
    {
        public bool IsAuthorizedToAccessFunction(string functionId)
        {
            User user = Helper.Toyota().Page.User;
            if (user != null)
            {
                string loweredFunctionId = functionId.ToLower();
                if (user.Roles != null)
                {
                    foreach (Role role in user.Roles)
                    {
                        if (role.Functions != null)
                        {
                            foreach (AuthorizationFunction function in role.Functions)
                            {
                                if (function.Id.ToLower().Equals(loweredFunctionId))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }                
            }
            return false;
        }
        public bool IsAuthorizedToAccessFeature(string functionId, string featureId)
        {
            User user = Helper.Toyota().Page.User;
            if (IsAuthorizedToAccessFunction(functionId) && (user != null))
            {
                string loweredFunctionId = functionId.ToLower();
                string loweredFeatureId = featureId.ToLower();
                if (user.Roles != null)
                {
                    foreach (Role role in user.Roles)
                    {
                        if (role.Functions != null)
                        {
                            foreach (AuthorizationFunction function in role.Functions)
                            {
                                if (function.Id.ToLower().Equals(loweredFunctionId) && (function.Features != null))
                                {
                                    foreach (AuthorizationFeature feature in function.Features)
                                    {
                                        if (feature.Id.ToLower().Equals(loweredFeatureId))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        public bool IsAuthorizedToAccessQualifier(string functionId, string featureId, string key)
        {
            if (IsAuthorizedToAccessFunction(functionId))
            {
                string q = GetFeatureQualifier(functionId, featureId, key);
                return !string.IsNullOrEmpty(q);
            }
            return false;
        }
        public bool IsAuthorizedToAccessQualifier(string functionId, string featureId, string key, string value)
        {
            if (IsAuthorizedToAccessFunction(functionId))
            {
                string q = GetFeatureQualifier(functionId, featureId, key);
                return !string.IsNullOrEmpty(q) && q.Equals(value);
            }
            return false;
        }

        public bool IsAuthorizedToAccessScreen()
        {
            string screenId = Helper.Toyota().Page.ScreenID.ToHtmlString();
            if (!string.IsNullOrEmpty(screenId))
            {
                return IsAuthorizedToAccessFunction(screenId);
            }
            return false;
        }        
        public bool IsAuthorizedToAccessScreenFeature(string featureId)
        {
            string screenId = Helper.Toyota().Page.ScreenID.ToHtmlString();
            if (IsAuthorizedToAccessScreen())
            {
                return IsAuthorizedToAccessFeature(screenId, featureId);
            }
            return false;
        }
        public bool IsAuthorizedToAccessScreenQualifier(string featureId, string key)
        {
            string screenId = Helper.Toyota().Page.ScreenID.ToHtmlString();
            if (IsAuthorizedToAccessScreen())
            {
                return IsAuthorizedToAccessQualifier(screenId, featureId, key);
            }
            return false;
        }
        public bool IsAuthorizedToAccessScreenQualifier(string featureId, string key, string value)
        {
            string screenId = Helper.Toyota().Page.ScreenID.ToHtmlString();
            if (IsAuthorizedToAccessScreen())
            {
                return IsAuthorizedToAccessQualifier(screenId, featureId, key, value);
            }
            return false;
        }

        public string GetFeatureQualifier(string functionId, string featureId, string key)
        {
            User user = Helper.Toyota().Page.User;
            if (IsAuthorizedToAccessFunction(functionId) && (user != null))
            {
                string loweredFunctionId = functionId.ToLower();
                string loweredFeatureId = featureId.ToLower();

                if (!user.Roles.IsNullOrEmpty())
                {
                    foreach (Role role in user.Roles)
                    {
                        if (role.Functions != null)
                        {
                            foreach (AuthorizationFunction function in role.Functions)
                            {
                                if (function.Id.ToLower().Equals(loweredFunctionId) && (function.Features != null))
                                {
                                    foreach (AuthorizationFeature feature in function.Features)
                                    {
                                        if (feature.Id.ToLower().Equals(loweredFeatureId) && (feature.Qualifiers != null))
                                        {
                                            foreach (AuthorizationFeatureQualifier qualifier in feature.Qualifiers)
                                            {
                                                if (qualifier.Key.Equals(key))
                                                {
                                                    return qualifier.Qualifier;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        public string GetScreenFeatureQualifier(string featureId, string key)
        {
            string screenId = Helper.Toyota().Page.ScreenID.ToHtmlString();
            if (IsAuthorizedToAccessScreen() && !string.IsNullOrEmpty(screenId))
            {
                return GetFeatureQualifier(screenId, featureId, key);
            }
            return null;
        }

        public HtmlHelper Helper { set; get; }
    }
}
