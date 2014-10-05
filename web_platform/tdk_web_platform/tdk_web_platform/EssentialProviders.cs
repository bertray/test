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
using Toyota.Common.Credential;

namespace Toyota.Common.Web.Platform
{
    public sealed class EssentialProviders
    {
        private EssentialProviders() { }

        private static EssentialProviders instance = null;
        public static EssentialProviders Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EssentialProviders();
                }
                return instance;
            }
        }

        public IUserProvider UserProvider { set; get; }
        public IAuthorizationRule AuthorizationRule { set; get; }
    }
}
