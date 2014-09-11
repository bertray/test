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
using System.Web.Routing;

namespace Toyota.Common.Web.Platform
{
    public interface IAuthenticationRule
    {
        AuthenticationRuleState Authenticate(RequestContext requestContext);
    }
}
