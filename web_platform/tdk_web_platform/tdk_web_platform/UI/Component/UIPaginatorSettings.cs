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
    public class UIPaginatorSettings: UIComponentSettings
    {
        public UIPaginatorSettings() 
        {
            ClientSideEvent = new UIPaginatorClientSideEvent();
            PageOptionCount = 3;
            Page = 1;
        }

        public UIPaginatorClientSideEvent ClientSideEvent { set; get; }
        public long Page { set; get; }
        public long PageCount { set; get; }

        private int _pageOptCount;
        public int PageOptionCount 
        {
            set 
            {
                _pageOptCount = value;
                if ((_pageOptCount % 2) == 0)
                {
                    _pageOptCount++;
                }
            }
            get
            {
                return _pageOptCount;
            }
        }

        public override IList<UIJavascriptMapProperty> GetJsProperties()
        {
            return base.GetJsProperties().ExtendAsList(props =>
            {
                props.Merge(ClientSideEvent.GetJsProperties());
                props.AddIfAllowed(new UIJavascriptMapProperty(false, "page", Page), Page >= 0);
                props.AddIfAllowed(new UIJavascriptMapProperty(false, "pageCount", PageCount), PageCount >= 0);
                props.AddIfAllowed(new UIJavascriptMapProperty(false, "pageOptCount", PageOptionCount), PageOptionCount > 0);
            });
        }
    }
}
