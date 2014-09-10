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

namespace Toyota.Common.Configuration
{
    /// <summary>
    /// Resembles a composite configuration that consists of one or more configuration.
    /// <see cref="Toyota.Common.Configuration.ConfigurationItem"/>
    /// 
    /// Configuration Cabinet stores one or more Configuration Binder.
    /// Think it like an office drawer cabinet which stores paper binders.
    /// Configuration Binder is like a paper binder which group one or more
    /// Configuration Item (which, in this analogy, is a paper).
    /// </summary>
    public class CompositeConfigurationItem: ConfigurationItem
    {
        private const string CONCATENATION_SEPARATOR = "|";
        private const string CONCATENATION_KEY_VALUE_SEPARATOR = ":";

        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        public CompositeConfigurationItem() : this(null) { }
        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="key">Configuration key</param>
        public CompositeConfigurationItem(string key) : this(key, null) { }
        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="key">Configuration key</param>
        /// <param name="description">Configuration description</param>
        public CompositeConfigurationItem(string key, string description): this(key, description, null) { }
        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="key">Configuration key</param>
        /// <param name="description">Configuration description</param>
        /// <param name="configItems">Configurations to be kept</param>
        public CompositeConfigurationItem(string key, string description, params ConfigurationItem[] configItems)
        {
            Items = new Dictionary<string, ConfigurationItem>();
            Key = key;
            Description = description;

            if ((configItems != null) && (configItems.Length > 0))
            {
                foreach (ConfigurationItem config in configItems)
                {
                    Items.Add(config.Key, config);
                }
            }
        }
        
        private IDictionary<string, ConfigurationItem> Items { set; get; }
        /// <summary>
        /// Adds a configuration
        /// </summary>
        /// <param name="config">Configuration to be added</param>
        public void AddItem(ConfigurationItem config)
        {
            if (config != null)
            {
                if (Items.ContainsKey(config.Key))
                {
                    Items[config.Key] = config;
                }
                else
                {
                    Items.Add(config.Key, config);
                }
            }
        }

        /// <summary>
        /// Removes a configuration
        /// </summary>
        /// <param name="key">Configuration key</param>
        public void RemoveItem(string key)
        {
            Items.Remove(key);
        }

        /// <summary>
        /// Gets a configuration
        /// </summary>
        /// <param name="key">Configuration key</param>
        /// <returns>Configuration instance of found</returns>
        public ConfigurationItem GetItem(string key)
        {
            if (Items.ContainsKey(key))
            {
                return Items[key];
            }
            return null;
        }

        /// <summary>
        /// Gets all stored configuration
        /// </summary>
        /// <returns>Configuration instances</returns>
        public ConfigurationItem[] GetItems()
        {
            if (Items.Count > 0)
            {
                return Items.Values.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Checks whether any configuration exists
        /// </summary>
        public bool HasItems
        {
            get
            {
                return Items.Count > 0;
            }
        }

        /// <summary>
        /// Checks if a configuration with given key exists
        /// </summary>
        /// <param name="key">Configuration key</param>
        /// <returns>true if configuration found</returns>
        public bool HasItem(string key)
        {
            return GetItem(key) != null;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.ConfigurationItem"/>
        /// </summary>
        public override string Value
        {
            get
            {
                if (Items.Count > 0)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (ConfigurationItem item in Items.Values)
                    {
                        stringBuilder.Append(CONCATENATION_SEPARATOR + " " + string.Format("{0}{1} {2}", item.Key, CONCATENATION_KEY_VALUE_SEPARATOR,item.Value));
                    }

                    string result = stringBuilder.ToString().Trim();
                    return result.Substring(1, result.Length);
                }
                return base.Value;
            }
            set
            {
                base.Value = value;
            }
        }

        /// <summary>
        /// A string representation of this configuration
        /// </summary>
        /// <returns>String formatted information</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(Description))
            {
                stringBuilder.AppendLine(string.Format("# {0} ({1})", Key, Description));
            }
            else
            {
                stringBuilder.AppendLine(string.Format("# {0}", Key));
            }

            if (Items.Count > 0)
            {
                foreach (ConfigurationItem item in Items.Values)
                {
                    stringBuilder.AppendLine(item.ToString());
                }
            }

            return stringBuilder.ToString();
        }
    }
}
