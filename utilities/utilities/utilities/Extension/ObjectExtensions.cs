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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Collections;
using System.Dynamic;
using System.ComponentModel;

namespace Toyota.Common.Utilities
{
    public static class ObjectExtensions
    {
        public static bool HasProperty(this object obj, string name)
        {
            if (!obj.IsNull())
            {
                return (obj.GetType().GetProperty("name") != null);
            }
            return false;
        }
        public static bool IsNull(this object obj)
        {
            return (obj == null);
        }
        public static Stream ToStream<T>(this T obj) 
        {
            if(obj != null) 
            {
                MemoryStream stream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return stream;
            }
            return null;
        }
        public static byte[] ToBytes(this object obj)
        {
            Stream stream = ToStream(obj);
            if (stream != null)
            {
                byte[] data = stream.ToBytes();
                stream.Close();
            }
            return null;
        }
        public static T FromStream<T>(this T obj, Stream stream)
        {
            object result = null;
            if (stream != null)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                result = formatter.Deserialize(stream);
            }
            return (T) result;
        }
        public static T FromBytes<T>(this T obj, byte[] bytes)
        {
            object result = null;
            if (!bytes.IsNullOrEmpty())
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(bytes, 0, bytes.Length);
                result = FromStream<T>(obj, stream);
                stream.Close();
            }
            return (T) result;
        }
        public static string ToUrlQueryString(this object obj, string baseUrl) 
        {            
            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrEmpty(baseUrl))
            {
                if (baseUrl.EndsWith("/"))
                {
                    result.Append(baseUrl.Substring(0, baseUrl.Length - 1));
                }
                else
                {
                    result.Append(baseUrl);
                }                
            }

            if (!obj.IsNull())
            {
                Type type = obj.GetType();
                PropertyInfo[] properties = type.GetProperties();
                if (!properties.IsNullOrEmpty())
                {
                    result.Append("?");
                    Type propType;
                    object propValue;
                    foreach (PropertyInfo prop in properties)
                    {
                        propValue = prop.GetValue(obj, null);
                        if (propValue.IsNull())
                        {
                            continue;
                        }

                        propType = prop.PropertyType;
                        if (propType.IsArray)
                        {
                            Array arrValue = (Array)propValue;
                            for (int i = 0; i < arrValue.Length; i++)
                            {
                                result.Append(string.Format("{0}={1}&", prop.Name, Convert.ToString(arrValue.GetValue(i))));
                            }
                        }
                        else if (propType.IsAssignableFrom(typeof(IEnumerable)) || propType.IsAssignableFrom(typeof(ICollection)))
                        {                            
                            IEnumerator enumerator;
                            if (propType.IsAssignableFrom(typeof(IEnumerable)))
                            {
                                IEnumerable enumerable = (IEnumerable)propValue;
                                enumerator = enumerable.GetEnumerator();
                            }
                            else
                            {
                                ICollection collection = (ICollection)propType;
                                enumerator = collection.GetEnumerator();
                            }
                            
                            enumerator.MoveNext();
                            object val = enumerator.Current;
                            while (!val.IsNull())
                            {
                                result.Append(string.Format("{0}={1}&", prop.Name, Convert.ToString(val)));
                                enumerator.MoveNext();
                            }
                        }
                        else
                        {
                            result.Append(string.Format("{0}={1}&", prop.Name, Convert.ToString(propValue)));
                        }
                    }
                    result.Remove(result.Length - 1, 1);
                }
            }
            return result.ToString();
        }        
        public static IDictionary<string, object> GetPropertyMap(this object obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            if (obj != null)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);                
                foreach (PropertyDescriptor prop in properties)
                {
                    map.Add(prop.Name, prop.GetValue(obj));
                }
            }
            return map;
        }
        public static T Define<T>(this T obj, Action<T> action) 
        {
            if (!obj.IsNull() & !action.IsNull())
            {
                action.Invoke(obj);
            }
            return obj;
        }

        public static string ToEmptyStringIfNull(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str;
            }
            return string.Empty;
        }
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static bool StringEquals(this string src, string tg)
        {
            if (!src.IsNull() && !tg.IsNull())
            {
                return src.Equals(tg);
            }
            return false;
        }
        public static bool StringEqualsIgnoreCase(this string src, string tg)
        {
            if (!src.IsNull() && !tg.IsNull())
            {
                return src.Equals(tg, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
