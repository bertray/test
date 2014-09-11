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
    public class DataUploadSettings
    {
        public DataUploadSettings() { }

        public string ValidationResultPath 
        {
            get
            {
                IConfigurationBinder binder = ApplicationConfigurationCabinet.Instance.GetBinder(GlobalConstants.Instance.Configuration.DataUpload.Name);
                ConfigurationItem rootConfig = binder.GetConfiguration(GlobalConstants.Instance.Configuration.DataUpload.RootFolder);
                ConfigurationItem validationConfig = binder.GetConfiguration(GlobalConstants.Instance.Configuration.DataUpload.ValidationResultFolder);
                return ApplicationSettings.Instance.Deployment.HomeFolderLocation + "\\" + rootConfig.Value + "\\" + validationConfig.Value;
            }
        }
    }
}
