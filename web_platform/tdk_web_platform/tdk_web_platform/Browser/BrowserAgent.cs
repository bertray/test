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
using System.Web;

namespace Toyota.Common.Web.Platform
{
    public class BrowserAgent
    {
        public string Code { set; get; }
        public string Name { set; get; }
        public string Version { set; get; }

        public static BrowserAgent InternetExplorer(string version)
        {
            return new BrowserAgent()
            {
                Code = "ie",
                Name = "Internet Explorer",
                Version = version
            };
        }
        public static BrowserAgent Firefox(string version)
        {
            return new BrowserAgent()
            {
                Code = "firefox",
                Name = "Mozilla Firefox",
                Version = version
            };
        }
        public static BrowserAgent Chrome(string version)
        {
            return new BrowserAgent()
            {
                Code = "chrome",
                Name = "Google Chrome",
                Version = version
            };
        }

        public bool IsAgentEquals(HttpBrowserCapabilitiesBase browser)
        {
            string type = browser.Type.ToLower();
            string version = browser.Version.ToLower();
            return !string.IsNullOrEmpty(Code) &&
                    !string.IsNullOrEmpty(Version) &&
                    type.Contains(Code.ToLower()) &&
                    version.Contains(Version.ToLower());
        }
    }
}
