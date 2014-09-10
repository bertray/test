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
using System.Collections;

namespace Toyota.Common.Configuration
{
    /// <summary>
    /// Resembles a configuration.
    /// 
    /// Configuration Cabinet stores one or more Configuration Binder.
    /// Think it like an office drawer cabinet which stores paper binders.
    /// Configuration Binder is like a paper binder which group one or more
    /// Configuration Item (which, in this analogy, is a paper).
    /// </summary>
    public class ConfigurationItem
    {
        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        public ConfigurationItem() { }
        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="key"Configuration key></param>
        /// <param name="value">Configuration value</param>
        public ConfigurationItem(string key, string value) : this(null, key, value) { }
        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="description">Configuration description</param>
        /// <param name="key">Configuration key</param>
        /// <param name="value">Configuration value</param>
        public ConfigurationItem(string description, string key, string value)
        {
            Key = key;
            Value = value;
            Description = description;
            Transient = false;
        }

        /// <summary>
        /// Key of the configuration
        /// </summary>
        public string Key { set; get; }

        /// <summary>
        /// Value of the configuration
        /// </summary>
        public virtual string Value { set; get; }

        /// <summary>
        /// Description of the configuration
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// States whether the configuration will be persisted
        /// into the persistence media or not.
        /// True value means the configuration will not be persisted.
        /// </summary>
        public bool Transient { set; get; }

        /// <summary>
        /// A string representation of this configuration
        /// </summary>
        /// <returns>String formatted information</returns>
        public override string ToString()
        {
            return string.Format("{0} = {1} ({2})", Key, Value, Description);
        }
    }
}
