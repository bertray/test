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
using System.Web.Mvc;

namespace Toyota.Common.Web.Platform
{
    public static class RazorHelperExtension
    {
        private static readonly PlatformUtilities extension = new PlatformUtilities();

        public static PlatformUtilities Toyota(this HtmlHelper helper) 
        {
            extension.Helper = helper;
            return extension;
        }
    }
}
