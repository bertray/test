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
using Toyota.Common.Credential;

namespace Toyota.Common.Web.Platform
{
    public class SecuritySettings
    {
        public SecuritySettings()
        {
            LoginController = "Login";
            ForgotPasswordController = "ForgotPassword";
            IgnoreAuthorization = true;
        }

        public string LoginController { set; get; }
        public string UnauthorizedController { set; get; }
        public string ForgotPasswordController { set; get; }
        public bool EnableAuthentication { set; get; }
        public bool IgnoreAuthorization { set; get; }
        public bool EnableSingleSignOn { set; get; }
        public bool SimulateAuthenticatedSession { set; get; }
        public User SimulatedAuthenticatedUser { set; get; }
        public bool UseCustomAuthenticationRule { set; get; }
        public string SSOServiceUrl { set; get; }
        public string SSOSessionStoragePath { set; get; }
    }
}
