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
using Toyota.Common.Lookup;
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Platform
{
    public static class WebExtension
    {
        public static ILookup Lookup(this HttpSessionStateBase session)
        {
            if (!session.IsNull())
            {
                return (ILookup) session[SessionKeys.LOOKUP];
            }
            return null;
        }
    }
}
