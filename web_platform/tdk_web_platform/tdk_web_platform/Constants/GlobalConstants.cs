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
using Toyota.Common.Configuration;

namespace Toyota.Common.Web.Platform
{
    class GlobalConstants
    {
        private GlobalConstants() 
        {
            Location = new LocationConstants();
            RequestParameter = new RequestParameterConstants();
            Configuration = new ConfigurationConstants();            
        }

        private static readonly GlobalConstants instance = new GlobalConstants();
        public static GlobalConstants Instance
        {
            get
            {
                return instance;
            }
        }

        public string SECURITY_COOKIE_SESSIONID
        {
            get { return "_tdk_ck_sessionId"; }
        }
        public string SECURITY_SALT_SESSION_ID
        {
            get
            {
                IConfigurationBinder binder = ApplicationConfigurationCabinet.Instance.GetBinder("Encryption");
                if (binder != null)
                {
                    ConfigurationItem config = binder.GetConfiguration("SessionId");
                    if (config != null)
                    {
                        return config.Value;
                    }
                }
                return "HFz127EDVNL5t9K7";
            }
        }
        public string CONFIGURATION_SINGLE_SIGN_ON_BINDER
        {
            get { return "SingleSignOn"; }
        }
        public string CONFIGURATION_SINGLE_SIGN_ON_URL
        {
            get { return "Url"; }
        }
        public string CONFIGURATION_SYSTEM_NAME
        {
            get { return "Application-Name"; }
        }
        public string CONFIGURATION_SYSTEM_ALIAS
        {
            get { return "Application-Alias"; }
        }

        public RequestParameterConstants RequestParameter { private set; get; }
        public LocationConstants Location { private set; get; }
        public ConfigurationConstants Configuration { private set; get; }        
    }
}
