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
using Toyota.Common.Lookup;

namespace Toyota.Common.Web.Platform
{
    [Serializable]
    internal class SSOSessionPersistence
    {
        public string Username { set; get; }
        public string Password { set; get; }
        public ILookup Data { set; get; }
    }
}
