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
    public class UIClientSideEvent: IJsPropertiesProvider
    {
        public UIClientSideEvent()
        {
            jsProperties = new List<UIJavascriptMapProperty>();
        }

        public string OnInitialized { set; get; }
        protected string NormalizeCallback(string callback)
        {
            if (!string.IsNullOrEmpty(callback))
            {
                callback = callback.Trim();
                if (!callback.StartsWith("function"))
                {
                    string[] lines = callback.Split(';');
                    if (!lines.IsNullOrEmpty())
                    {
                        if (lines.Length > 1)
                        {
                            callback = string.Format("function(s,e) {{ {0} }}", callback);
                        }
                        else
                        {
                            if (!callback.EndsWith(")"))
                            {
                                callback = string.Format("function(s,e) {{ {0}(s,e); }}", callback);
                            }
                        }
                    }
                }
            }            
            return callback;
        }

        private IList<UIJavascriptMapProperty> jsProperties;
        public virtual IList<UIJavascriptMapProperty> GetJsProperties()
        {
            return jsProperties.ExtendAsList(props =>
            {
                props.AddIfStringIsNotNullOrEmpty(new UIJavascriptMapProperty(false, "onInitialized", NormalizeCallback(OnInitialized)), OnInitialized);
            });
        }
        public UIJavascriptMapProperty GetJsProperty(string key)
        {
            if (!key.IsNullOrEmpty())
            {
                foreach (UIJavascriptMapProperty prop in jsProperties)
                {
                    if (prop.Key.Equals(key))
                    {
                        return prop;
                    }
                }
            }
            return null;
        }
    }
}
