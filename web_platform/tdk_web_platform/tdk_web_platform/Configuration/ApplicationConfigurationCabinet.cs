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
using Toyota.Common.Configuration.Binder;
using System.Web;
using System.IO;

namespace Toyota.Common.Web.Platform
{
    public class ApplicationConfigurationCabinet: ConfigurationCabinet
    {
        private static ApplicationConfigurationCabinet instance = new ApplicationConfigurationCabinet();

        private ApplicationConfigurationCabinet(): base("Application Configuration") 
        {
            string path = HttpContext.Current.Server.MapPath("~/Configurations");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }            
        }

        public static ApplicationConfigurationCabinet Instance 
        {
            get
            {
                return instance;
            }
        }
    }
}
