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
using System.Web.Mvc.Html;

namespace Toyota.Common.Web.Platform
{
    public class UIPaginator: UIComponent
    {
        public UIPaginator(HtmlHelper htmlHelper, UIPaginatorSettings settings): base(htmlHelper)
        {
            Settings = (settings != null) ? settings : new UIPaginatorSettings();
        }
        public UIPaginator(HtmlHelper htmlHelper) : this(htmlHelper, null) { }

        public UIPaginatorSettings Settings { private set; get; }
        protected override System.Web.Mvc.MvcHtmlString GenerateHtml()
        {
            return PartialExtensions.Partial(_HtmlHelper, "tdk/UIPaginator", Settings);
        }
    }
}
