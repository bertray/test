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

namespace Toyota.Common.Web.Platform
{
    public class ApplicationRazorExtension: BaseRazorExtension
    {
        public MvcHtmlString Name
        {
            get
            {                
                return new MvcHtmlString(ApplicationSettings.Instance.Name);
            }
        }
        public MvcHtmlString Alias
        {
            get
            {
                return new MvcHtmlString(ApplicationSettings.Instance.Alias);
            }
        }
        public MvcHtmlString OwnerName
        {
            get
            {
                return new MvcHtmlString(ApplicationSettings.Instance.OwnerName);
            }
        }
        public MvcHtmlString OwnerAlias
        {
            get
            {
                return new MvcHtmlString(ApplicationSettings.Instance.OwnerAlias);
            }
        }
        public MvcHtmlString OwnerEmail
        {
            get
            {
                return new MvcHtmlString(ApplicationSettings.Instance.OwnerEmail);
            }
        }
    }
}
