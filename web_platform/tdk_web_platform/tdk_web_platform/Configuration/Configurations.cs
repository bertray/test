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
    public class Configurations
    {
        private static Configurations instance = null;
        private Configurations() { }

        public static Configurations Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Configurations();
                }
                return instance;
            }
        }

        private static string _singleSignOnUrl = null;
        public string SingleSignOn_Url
        {
            get
            {
                if (string.IsNullOrEmpty(_singleSignOnUrl))
                {
                    IConfigurationBinder binder = ApplicationConfigurationCabinet.Instance.GetBinder(GlobalConstants.Instance.CONFIGURATION_SINGLE_SIGN_ON_BINDER);
                    ConfigurationItem item = binder.GetConfiguration(GlobalConstants.Instance.CONFIGURATION_SINGLE_SIGN_ON_URL);
                    if (item != null)
                    {
                        _singleSignOnUrl =  item.Value;
                    }
                }                
                return _singleSignOnUrl;
            }
        }
    }
}
