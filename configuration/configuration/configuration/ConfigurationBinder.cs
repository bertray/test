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

namespace Toyota.Common.Configuration
{
    /// <summary>
    /// Default implementation of a configuration binder.
    /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
    /// 
    /// Configuration Cabinet stores one or more Configuration Binder.
    /// Think it like an office drawer cabinet which stores paper binders.
    /// Configuration Binder is like a paper binder which group one or more
    /// Configuration Item (which, in this analogy, is a paper).
    /// </summary>
    public abstract class ConfigurationBinder: IConfigurationBinder
    {
        protected const string EXTENSION = "config";

        private string label;
        private IDictionary<string, ConfigurationItem> configurationMap;

        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="label">Configuration label</param>
        public ConfigurationBinder(string label)
        {
            this.label = label;
            this.configurationMap = new Dictionary<string, ConfigurationItem>();
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public void AddConfiguration(ConfigurationItem configuration)
        {
            string key = configuration.Key;
            if (configurationMap.ContainsKey(key))
            {
                configurationMap[key] = configuration;
            }
            else
            {
                configurationMap.Add(key, configuration);
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public void RemoveConfiguration(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                configurationMap.Remove(key);
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public ConfigurationItem GetConfiguration(string key)
        {
            if (!string.IsNullOrEmpty(key) && configurationMap.ContainsKey(key))
            {
                return configurationMap[key];
            }

            return null;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public ConfigurationItem[] GetConfigurations()
        {
            if (configurationMap.Count > 0)
            {
                return configurationMap.Values.ToArray();
            }

            return null;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public string GetLabel()
        {
            return label;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public IList<string> GetKeys()
        {
            if (configurationMap.Count > 0)
            {
                return configurationMap.Keys.ToList().AsReadOnly();
            }
            return null;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public void MarkAsTransient(string key)
        {
            if (configurationMap.ContainsKey(key))
            {
                configurationMap[key].Transient = true;
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public void UnmarkAsTransient(string key)
        {
            if (configurationMap.ContainsKey(key))
            {
                configurationMap[key].Transient = false;
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public void MarkAllAsTransient()
        {
            foreach (ConfigurationItem config in configurationMap.Values)
            {
                config.Transient = true;
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public void MarkAllAsPersisted()
        {
            foreach (ConfigurationItem config in configurationMap.Values)
            {
                config.Transient = false;
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public void RemoveNonPersistedConfigurations()
        {
            IList<string> keys = new List<string>();
            foreach (ConfigurationItem config in configurationMap.Values)
            {
                if (config.Transient)
                {
                    keys.Add(config.Key);
                }
            }

            foreach (string k in keys)
            {
                configurationMap.Remove(k);
            }
        }
    }
}
