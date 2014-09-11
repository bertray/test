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
    public class BrowserSettings
    {
        public BrowserSettings()
        {
            BlockedAgents = new List<BrowserAgent>();
        }

        public bool EnableRestriction { set; get; }
        public IList<BrowserAgent> BlockedAgents { private set; get; }
        public string BlockingControllerName { set; get; }
    }
}
