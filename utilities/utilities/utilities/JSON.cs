///
/// <author>lufty.abdillah@gmail.com</author>
/// <summary>
/// Toyota .Net Development Kit
/// Copyright (c) Toyota Motor Manufacturing Indonesia, All Right Reserved.
/// </summary>
/// 
using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack.Text;

namespace Toyota.Common.Utilities
{
    public class JSON
    {
        public static string ToString(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return JsonSerializer.SerializeToString(obj);
        }

        public static string ToString(object obj, Type type)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return JsonSerializer.SerializeToString(obj, type);
        }

        public static string ToString<T>(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return JsonSerializer.SerializeToString<T>((T)obj); 
        }
        
        public static T ToObject<T>(string str)
        {
            object result = null;
            if (!string.IsNullOrEmpty(str))
            {                 
                result = JsonSerializer.DeserializeFromString<T>(str); 
            } 
            return (T) result;
        }

        public static object ToObject(string str, Type type)
        {
            if(!string.IsNullOrEmpty(str)) 
            {
                return JsonSerializer.DeserializeFromString(str, type);
            }
            return null;
        }
    }
}
