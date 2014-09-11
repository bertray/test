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
using Toyota.Common.Configuration;
using System.Web;
using Toyota.Common.Configuration.Binder;

namespace Toyota.Common.Web.Platform
{
    public class TextFileLocalizedCollection: ILocalizedWordCollection
    {
        private IConfigurationBinder collectionBinder;
        private string defaultCode;
        private string path;
        private string label;

        public TextFileLocalizedCollection(string path, string label)
        {
            this.path = path;
            this.label = label;
        }

        public void SetDefaultCode(string code)
        {
            defaultCode = code;
            collectionBinder = new DifferentialTextFileConfigurationBinder(label, code, path);
            collectionBinder.Load();
            collectionBinder.Save();
        }

        public string GetDefaultCode()
        {
            return defaultCode;
        }

        public string Translate(string key)
        {
            ConfigurationItem item = collectionBinder.GetConfiguration(key);
            if (item != null)
            {
                return item.Value;
            }            
            return null;
        }

        public IDictionary<string, string> GetAll()
        {
            ConfigurationItem[] items = collectionBinder.GetConfigurations();
            if (items != null)
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach (ConfigurationItem item in items)
                {
                    result.Add(item.Key, item.Value);
                }
                return result;
            }
            return null;
        }

        public IList<string> GetKeys()
        {
            ConfigurationItem[] items = collectionBinder.GetConfigurations();
            if (items != null)
            {
                List<string> result = new List<string>();
                foreach (ConfigurationItem item in items)
                {
                    result.Add(item.Key);
                }
                return result;
            }
            return null;
        }
    }
}
