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

namespace Toyota.Common.Web.Platform
{
    public class ConfigurationConstants
    {
        public ConfigurationConstants()
        {
            System = new SystemConfigurationConstants();
            Database = new DatabaseConfigurationConstants();
            Locale = new LocalizationConfigurationConstants();
            DataUpload = new DataUploadConfigurationConstants();
            Session = new SessionConfigurationConstants();
        }

        public SystemConfigurationConstants System { private set; get; }
        public DatabaseConfigurationConstants Database { private set; get; }
        public LocalizationConfigurationConstants Locale { private set; get; }
        public DataUploadConfigurationConstants DataUpload { private set; get; }
        public SessionConfigurationConstants Session { private set; get; } 
    }
}
