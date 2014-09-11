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
    public class UIComboBox: UIComponent
    {
        public UIComboBox(HtmlHelper htmlHelper, UIComboBoxSettings settings): base(htmlHelper)
        {
            Settings = (settings != null) ? settings : new UIComboBoxSettings();
        }
        public UIComboBox(HtmlHelper htmlHelper) : this(htmlHelper, null) { }

        public UIComboBoxSettings Settings { private set; get; }
        public UIComboBox DataSource(IEnumerable dataSource)
        {
            if (dataSource != null)
            {
                IEnumerator enumerator = dataSource.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Settings.AddItem(enumerator.Current);
                }
            }
            return this;
        }
        protected override System.Web.Mvc.MvcHtmlString GenerateHtml()
        {
            return PartialExtensions.Partial(_HtmlHelper, "tdk/UIComboBox", Settings);
        }
    }
}
