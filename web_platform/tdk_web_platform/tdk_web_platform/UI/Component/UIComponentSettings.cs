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
    public class UIComponentSettings: IJsPropertiesProvider
    {
        public UIComponentSettings()
        {
            jsProperties = new List<UIJavascriptMapProperty>();
            _Attributes = new Dictionary<string, object>();
            _Attributes.Add("id", null);
            _Attributes.Add("name", null);
            CssClass = new List<string>();
            InverseCssClass = "inverse";
            ColorType = UIColorType.Default;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Id is required")]
        public string Id
        {
            set { _Attributes["id"] = value; }
            get { return (string)_Attributes["id"]; }
        }
        public string Name
        {
            set { _Attributes["name"] = value; }
            get {
                string name = (string)_Attributes["name"];
                if (string.IsNullOrEmpty(name)) 
                {
                    _Attributes["name"] = Id;
                }
                return (string)_Attributes["name"]; 
            }
        }
        public string Caption { set; get; }
        public bool IsInverted
        {
            set
            {
                if (!CssClass.Contains(InverseCssClass))
                {
                    CssClass.Add(InverseCssClass);
                }
                else
                {
                    CssClass.Remove(InverseCssClass);
                }
            }
            get
            {
                return CssClass.Contains(InverseCssClass);
            }
        }
        protected string InverseCssClass { set; get; }
        public UIColorType ColorType { set; get; }

        private IDictionary<string, object> _Attributes;
        public IList<KeyValuePair<string, object>> GetAttributes
        {
            get
            {
                IList<KeyValuePair<string, object>> result = null;
                if (!_Attributes.IsNullOrEmpty())
                {
                    result = new List<KeyValuePair<string, object>>();
                    foreach (string key in _Attributes.Keys)
                    {
                        result.Add(new KeyValuePair<string, object>(key, _Attributes[key]));
                    }
                }
                return result;
            }
        }
        public void AddAttribute(string id, object value)
        {
            if (!_Attributes.ContainsKey(id))
            {
                _Attributes.Add(id, value);
            }
            else
            {
                _Attributes[id] = value;
            }
        }
        public KeyValuePair<string, T> GetAttribute<T>(string id)
        {
            if (_Attributes.ContainsKey(id))
            {
                return new KeyValuePair<string, T>(id, (T)_Attributes[id]);
            }
            return new KeyValuePair<string, T>(null, default(T));
        }
        public IList<string> CssClass { private set; get; }

        public MvcHtmlString PrintCssClass()
        {
            StringBuilder sCss = new StringBuilder();
            if (!CssClass.IsNullOrEmpty())
            {
                foreach (string css in CssClass)
                {
                    sCss.Append(" " + css);
                }
                sCss.Append(" ");
            }
            return new MvcHtmlString(sCss.ToString());
        }
        public MvcHtmlString PrintAttributes()
        {
            StringBuilder sAttr = new StringBuilder();
            if (!_Attributes.IsNullOrEmpty())
            {
                foreach (string attr in _Attributes.Keys)
                {
                    sAttr.Append(string.Format("{0}=\"{1}\" ", attr, _Attributes[attr]));
                }
            }
            return new MvcHtmlString(sAttr.ToString());
        }
        public MvcHtmlString PrintAttribute(string key)
        {
            if (_Attributes.ContainsKey(key))
            {
                string value = (string) _Attributes["name"];
                if (!string.IsNullOrEmpty(value))
                {
                    return new MvcHtmlString(string.Format("{0}=\"{1}\" ", key, _Attributes[key]));
                }                
            }
            return new MvcHtmlString(string.Empty);
        }
        public MvcHtmlString PrintJsProperties()
        {
            IList<UIJavascriptMapProperty> props = GetJsProperties();
            if (props.Count > 0)
            {
                StringBuilder map = new StringBuilder();
                map.Append('{');
                string sVal;
                foreach (UIJavascriptMapProperty p in props)
                {
                    sVal = Convert.ToString(p.Value);
                    if (p.Value is bool)
                    {
                        sVal = sVal.ToLower();
                    }
                    if (p.AsString)
                    {
                        map.Append(string.Format("{0}: \"{1}\",", p.Key, sVal));
                    }
                    else
                    {
                        map.Append(string.Format("{0}: {1},", p.Key, sVal));
                    }
                }
                map.Remove(map.Length - 1, 1);
                map.Append('}');
                return new MvcHtmlString(map.ToString());
            }
            return new MvcHtmlString(string.Empty);
        }

        private IList<UIJavascriptMapProperty> jsProperties;
        public virtual IList<UIJavascriptMapProperty> GetJsProperties()
        {
            return jsProperties;
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
