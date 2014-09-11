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
    public class UIDataTable: UIComponent
    {
        public UIDataTable(HtmlHelper htmlHelper, UIDataTableSettings settings): base(htmlHelper)
        {
            Settings = (settings != null) ? settings : new UIDataTableSettings();
        }
        public UIDataTable(HtmlHelper htmlHelper) : this(htmlHelper, null) { }

        public UIDataTableSettings Settings { private set; get; }

        public UIDataTable DataSource(IEnumerable source)
        {
            Settings.DataSource = source;
            return this;
        }

        protected override System.Web.Mvc.MvcHtmlString GenerateHtml()
        {
            return PartialExtensions.Partial(_HtmlHelper, "tdk/UIDataTable", Settings);
        }
    }
}
