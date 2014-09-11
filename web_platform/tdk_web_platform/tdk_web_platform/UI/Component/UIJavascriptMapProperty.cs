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

namespace Toyota.Common.Web.Platform
{
    public class UIJavascriptMapProperty
    {
        public UIJavascriptMapProperty(bool asString, string key, object value)
        {
            AsString = asString;
            Key = key;
            Value = value;
        }
        public UIJavascriptMapProperty() : this(true, null, null) { }

        public string Key { set; get; }
        public object Value { set; get; }
        public bool AsString { set; get; }
    }
}
