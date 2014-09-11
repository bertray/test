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
using System.Collections;

namespace Toyota.Common.Web.Platform
{
    public class UISummaryList: UIComponent
    {
        public UISummaryList(HtmlHelper htmlHelper, UISummaryListSettings settings)
            : base(htmlHelper)
        {
            Settings = (settings != null) ? settings : new UISummaryListSettings();
        }
        public UISummaryList(HtmlHelper htmlHelper) : this(htmlHelper, null) { }

        public UISummaryListSettings Settings { private set; get; }
        public UISummaryList DataSource(IEnumerable source)
        {
            Settings.DataSource = source;
            return this;
        }

        protected override System.Web.Mvc.MvcHtmlString GenerateHtml()
        {
            return PartialExtensions.Partial(_HtmlHelper, "tdk/UISummaryList", Settings);
        }
    }
}
