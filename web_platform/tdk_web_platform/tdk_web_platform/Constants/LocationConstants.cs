﻿///
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
    public class LocationConstants
    {        
        public string Configuration
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/Configurations");
            }
        }

        public string SQLStatement
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/SQL");
            }
        }

        public string Content
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/Content");
            }
        }
    }
}
