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
using System.Reflection;
using Toyota.Common.Utilities;
using Toyota.Common.Lookup;
using System.Web;

namespace Toyota.Common.Web.Platform
{
    public static class PlatformExtensions
    {
        public static bool IsNullOrEmpty(this MvcHtmlString str)
        {
            return (str.IsNull() || string.IsNullOrEmpty(str.ToString()));
        }
        public static MvcHtmlString ToJsMapPair(this object obj, string propName)
        {
            return ToJsMapPair(obj, propName, true);
        }
        public static MvcHtmlString ToJsMapPair(this object obj, string propName, bool asString)
        {
            return ToJsMapPair(obj, propName, asString, null);
        }
        public static MvcHtmlString ToJsMapPair(this object obj, string propName, bool asString, string defaultValue)
        {
            string outVal;
            return ToJsMapPair(obj, propName, asString, defaultValue, out outVal);
        }
        public static MvcHtmlString ToJsMapPair(this object obj, string propName, bool asString, string defaultValue, out string value)
        {
            string resultStr = string.Empty;
            value = string.Empty;
            if (obj != null)
            {
                PropertyInfo property = obj.GetType().GetProperty(propName);
                if (property != null)
                {
                    value = Convert.ToString(property.GetValue(obj, null));
                               
                }
                else if (defaultValue != null)
                {
                    value = defaultValue;
                }

                if ((property != null) || (defaultValue != null))
                {
                    if (!asString)
                    {
                        resultStr = string.Format("{0}: {1}", propName, value);
                    }
                    else
                    {
                        resultStr = string.Format("{0}: \"{1}\"", propName, value);
                    }
                }                
            }
            
            return new MvcHtmlString(resultStr);
        }        
        public static MvcHtmlString ToHtmlAttribute(this object obj, string propName)
        {
            return ToHtmlAttribute(obj, propName, false);
        }
        public static MvcHtmlString ToHtmlAttribute(this object obj, string propName, bool autoGenerate)
        {
            string result;
            return ToHtmlAttribute(obj, propName, autoGenerate, out result);
        }
        public static MvcHtmlString ToHtmlAttribute(this object obj, string propName, bool autoGenerate, out string value)
        {
            string resultStr = string.Empty;
            value = string.Empty;
            if (obj != null)
            {
                PropertyInfo property = obj.GetType().GetProperty(propName);
                if (property != null)
                {
                    value = Convert.ToString(property.GetValue(obj, null));
                    resultStr = string.Format("{0}=\"{1}\"", property.Name, value);
                }
                else if (autoGenerate)
                {
                    value = Guid.NewGuid().ToString();
                    resultStr = string.Format("{0}=\"{1}\"", propName, value);
                }
            }
            else
            {
                value = Guid.NewGuid().ToString();
                resultStr = string.Format("{0}=\"{1}\"", propName, value);
            }
            return new MvcHtmlString(resultStr);
        }
        public static MvcHtmlString PrintProperty(this object obj, string propName)
        {
            string value;
            return PrintProperty(obj, propName, out value);
        }
        public static MvcHtmlString PrintProperty(this object obj, string propName, out string value)
        {
            string resultStr = string.Empty;
            value = string.Empty;
            if (obj != null)
            {
                PropertyInfo property = obj.GetType().GetProperty(propName);
                if (property != null)
                {
                    value = Convert.ToString(property.GetValue(obj, null));
                    resultStr = value;
                }
            }
            return new MvcHtmlString(resultStr);
        }
        public static IEnumerable<KeyValuePair<string, object>> ExtractKeyValueList(this object obj, string propName)
        {
            if (obj != null)
            {                
                PropertyInfo property = obj.GetType().GetProperty(propName);
                if (property != null)
                {
                    foreach (KeyValuePair<string, object> objPair in (List<KeyValuePair<string, object>>)property.GetValue(obj, null))
                    {
                        yield return objPair;
                    }
                }
            }
        }
        public static MvcHtmlString ToMvcHtmlString(this string str) 
        {
            return ToMvcHtmlString(str, string.Empty);
        }
        public static MvcHtmlString ToMvcHtmlString(this string str, string fallbackString)
        {
            return new MvcHtmlString(str != null ? str : fallbackString);
        }

        public static void Write(this HtmlHelper helper, MvcHtmlString str)
        {
            if (!helper.IsNull())
            {
                helper.ViewContext.Writer.Write(str);
            }            
        }
        public static void Write(this HtmlHelper helper, string str)
        {
            Write(helper, new MvcHtmlString(str));
        }
        public static void WriteLine(this HtmlHelper helper, MvcHtmlString str)
        {
            if (!helper.IsNull())
            {
                helper.ViewContext.Writer.WriteLine(str);
            }            
        }
        public static void WriteLine(this HtmlHelper helper, string str)
        {
            WriteLine(helper, new MvcHtmlString(str));
        }

        public static ILookup Lookup(this HttpSessionStateBase session)
        {
            if (!session.IsNull())
            {
                return (ILookup)session[SessionKeys.LOOKUP];
            }
            return null;
        }
    }
}
