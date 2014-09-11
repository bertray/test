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
using System.Web.Mvc.Html;
using System.Web.Mvc;

namespace Toyota.Common.Web.Platform
{
    public class UIRadio: UIComponent
    {
        public UIRadio(HtmlHelper htmlHelper, UIRadioSettings settings): base(htmlHelper)
        {
            Settings = (settings != null) ? settings : new UIRadioSettings();            
        }
        public UIRadio(HtmlHelper htmlHelper) : this(htmlHelper, null) { }

        public UIRadioSettings Settings { private set; get; }        

        protected override System.Web.Mvc.MvcHtmlString GenerateHtml()
        {
            return PartialExtensions.Partial(_HtmlHelper, "tdk/UIRadio", Settings);
        }
    }
}
