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
using System.Threading;
using System.Runtime.Serialization;
using Toyota.Common.Utilities;

namespace Toyota.Common.Task
{
    [Serializable]
    public class BackgroundTaskParameter
    {
        private IDictionary<string, string> paramMap;

        public BackgroundTaskParameter()
        {
            paramMap = new Dictionary<string, string>();
        }

        public void Add(string key, object value, Type type)
        {
            string strValue = JSON.ToString(value, type);
            Add(key, strValue);
        }

        public void Add(string key, string value)
        {
            if (paramMap.ContainsKey(key))
            {
                paramMap[key] = value;
            }
            else
            {
                paramMap.Add(key, value);
            }
        }

        public void Remove(string key)
        {
            if (paramMap.ContainsKey(key))
            {
                paramMap.Remove(key);
            }
        }

        public string Get(string key)
        {
            if (paramMap.ContainsKey(key))
            {
                return paramMap[key];
            }

            return null;
        }

        public T Get<T>(string key)
        {
            object result = null;
            if (paramMap.ContainsKey(key))
            {
                return JSON.ToObject<T>(paramMap[key]);
            }

            return (T) result;
        }

        public override string ToString()
        {
            string paramString = JSON.ToString<IDictionary<string, string>>(paramMap);
            paramString = paramString.Replace(" ", "&#32;");
            paramString = paramString.Replace("\"","&quot;");            
            return paramString;
        }

        public static BackgroundTaskParameter FromString(string str)
        {            
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("&#32;", " ");                
                str = str.Replace("&quot;", "\"");                
                BackgroundTaskParameter runtime = new BackgroundTaskParameter();                
                IDictionary<string, string> runtimeParam = JSON.ToObject<IDictionary<string, string>>(str);                
                foreach (string key in runtimeParam.Keys)
                {
                    runtime.Add(key, runtimeParam[key]);                    
                }
                return runtime;
            }
            return null;
        }
    }
}
