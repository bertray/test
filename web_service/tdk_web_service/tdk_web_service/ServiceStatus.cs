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

namespace Toyota.Common.Web.Service
{
    public enum ServiceStatus
    {
        Success,
        Success_With_Warning,
        Ready,
        Continue,
        Confirmed,
        OK,
        Approved,
        Denied,
        Finished,
        Busy,        
        Aborted,
        Error,
        Unavailable
    }
}
