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

namespace Toyota.Common.Utilities
{
    public class JsonDataMap
    {
        protected IDictionary<string, string> DataMap { set; get; }

        public JsonDataMap(): this(null) { }
        public JsonDataMap(string dataString) 
        {
            FromString(dataString);
        }

        public void Add(string key, object data, Type type)
        {
            if (DataMap.ContainsKey(key))
            {
                DataMap[key] = JSON.ToString(data, type);
            }
            else
            {
                DataMap.Add(key, JSON.ToString(data, type));
            }
        }
        public void Add<T>(string key, T data)
        {
            Add(key, data, typeof(T));
        }
        public void AddRaw(string key, string json)
        {
            if (DataMap.ContainsKey(key))
            {
                DataMap[key] = json;
            }
            else
            {
                DataMap.Add(key, json);
            }
        }
        public void Remove(string key)
        {
            DataMap.Remove(key);
        }

        public object Get(string key, Type type)
        {
            if (DataMap.ContainsKey(key))
            {
                return JSON.ToObject(DataMap[key], type);
            }
            return null;
        }
        public T Get<T>(string key)
        {            
            return (T)Get(key, typeof(T));
        }
        public string GetRaw(string key)
        {
            if (DataMap.ContainsKey(key))
            {
                return DataMap[key];
            }
            return null;
        }
        public bool HasKey(string key)
        {
            return DataMap.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return DataMap.Keys; }            
        }
        public int Count
        {
            get
            {
                return DataMap.Count;
            }
        }
        public void Clear()
        {
            DataMap.Clear();
        }

        public void FromString(string dataString)
        {
            if (string.IsNullOrEmpty(dataString))
            {
                DataMap = new Dictionary<string, string>();
            }
            else
            {
                DataMap = JSON.ToObject<IDictionary<string, string>>(dataString);
            }
        }
        public override string ToString()
        {
            return JSON.ToString<IDictionary<string, string>>(DataMap);
        }
    }
}
