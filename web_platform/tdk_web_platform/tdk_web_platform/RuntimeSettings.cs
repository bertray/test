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
    public class RuntimeSettings
    {
        public RuntimeSettings()
        {
            Browser = new BrowserSettings();
            Mode = RuntimeMode.Online;
            HomeController = "Home";
            EnableMobileSupport = false;
        }

        public BrowserSettings Browser { private set; get; }
        public RuntimeMode Mode { set; get; }
        public string OfflineModeController { set; get; }
        public string MaintenanceModeController { set; get; }
        public string HomeController { set; get; }
        public bool EnableMobileSupport { set; get; } 
    }
}
