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
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Platform
{
    public class UIPaginatorClientSideEvent: UIClientSideEvent
    {
        public string OnPageChanged { set; get; }

        public override IList<UIJavascriptMapProperty> GetJsProperties()
        {
            return base.GetJsProperties().ExtendAsList(props =>
            {
                props.AddIfStringIsNotNullOrEmpty(new UIJavascriptMapProperty(false, "onPageChanged", NormalizeCallback(OnPageChanged)), OnPageChanged);
            });
        }
    }
}
