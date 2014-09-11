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
using System.Web.Mvc;
using Toyota.Common.Utilities;
using System.Dynamic;

namespace Toyota.Common.Web.Platform
{
    public class ScreenMessageAjaxExtension: IAjaxExtension
    {
        public string GetName()
        {
            return "Screen-Message";
        }

        public System.Web.Mvc.ActionResult Execute(System.Web.HttpRequestBase request, System.Web.HttpResponseBase response, System.Web.HttpSessionStateBase session)
        {
            return ScreenMessageUtil.CreateAjaxResult(session);            
        }
    }
}
