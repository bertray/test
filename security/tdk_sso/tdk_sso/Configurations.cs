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
using System.Web;

namespace Toyota.Common.SSO
{
    internal class Configurations
    {
        private Configurations() { }

        private static Configurations instance = null;
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

        public string DevelopmentPhase { set; get; }
        public string UserDBConnectionString { set; get; }
        public string ServiceDBConnectionString { set; get; }
        public int WalkerWorkingPeriod { set; get; }
    }
}