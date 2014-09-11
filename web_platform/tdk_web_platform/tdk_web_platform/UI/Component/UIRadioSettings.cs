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
    public class UIRadioSettings: UIComponentSettings
    {
        public UIRadioSettings()
        {
            ClientSideEvent = new UIRadioClientSideEvent();
        }

        public bool Checked { set; get; }        
        public UIRadioClientSideEvent ClientSideEvent { private set; get; }

        public override IList<UIJavascriptMapProperty> GetJsProperties()
        {
            return base.GetJsProperties().ExtendAsList(props =>
            {
                props.Merge(ClientSideEvent.GetJsProperties());
                props.AddIfAllowed(new UIJavascriptMapProperty(false, "checked", Checked), Checked);                
            });
        }
    }
}
