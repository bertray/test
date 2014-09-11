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
    public sealed class StandardAuthorizationFeatureQualifier
    {
        private StandardAuthorizationFeatureQualifier() { }

        public const string Viewer = "TDK-Viewer";
        public const string Owner = "TDK-Owner";
        public const string Availibility = "TDK-Availibility";
        public const string Filtering = "TDK-Filter";
        public const string Level = "TDK-Level";
        public const string Priority = "TDK-Priority";
    }
}
