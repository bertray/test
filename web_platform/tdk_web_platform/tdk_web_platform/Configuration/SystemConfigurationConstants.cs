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
    public class SystemConfigurationConstants
    {
        public string Name { get { return "System"; } }
        public string DeploymentContext { get { return "Deployment-Context"; } }
        public string DevelopmentStage { get { return "Development-Stage"; } }        
        public string HomeFolder { get { return "Home-Folder"; } }
    }
}
