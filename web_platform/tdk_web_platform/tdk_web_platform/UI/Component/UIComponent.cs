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
using System.ComponentModel.DataAnnotations;
using Toyota.Common.Utilities;
using System.Web.Mvc;

namespace Toyota.Common.Web.Platform
{    
    public abstract class UIComponent
    {
        public UIComponent(HtmlHelper htmlHelper)
        {
            _HtmlHelper = htmlHelper;
        }

        protected HtmlHelper _HtmlHelper { set; get; }        

        public virtual MvcHtmlString GetHtml()
        {
            MvcHtmlString markup = GenerateHtml();
            if (markup == null)
            {
                markup = new MvcHtmlString(string.Empty);
            }
            return markup;
        }
        public virtual void Render(ViewContext viewContext)
        {
            MvcHtmlString markup = GetHtml();
            viewContext.Writer.WriteLine(markup);
        }
        public virtual void Render()
        {
            if (_HtmlHelper != null)
            {
                Render(_HtmlHelper.ViewContext);
            }            
        }
        protected abstract MvcHtmlString GenerateHtml();
    }
}
