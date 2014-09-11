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

namespace Toyota.Common.Web.Platform
{
    public class PageSettings
    {
        public PageSettings(Type controllerType)
        {
            Title = string.Empty;            
            string className = controllerType.Name;
            ControllerName = className.Substring(0, className.IndexOf("Controller"));
            ScreenID = ControllerName;
            IndexPage = ScreenID;
        }

        public string Title { set; get; }
        public string ScreenID { set; get; }
        public string ControllerName { set; get; }
        public string IndexPage { set; get; }

        public void AttachToRequest(ViewDataDictionary viewData)
        {
            viewData["_tdkScreenTitle"] = Title;
            viewData["Title"] = Title;
            viewData["_tdkScreenID"] = ScreenID;
            viewData["_tdkControllerName"] = ScreenID;
        }
    }
}
