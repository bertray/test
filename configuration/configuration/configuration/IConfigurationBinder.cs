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
    /// Specification for a configuration binder.
    /// 
    /// Configuration Cabinet stores one or more Configuration Binder.
    /// Think it like an office drawer cabinet which stores paper binders.
    /// Configuration Binder is like a paper binder which group one or more
    /// Configuration Item (which, in this analogy, is a paper).
    /// </summary>
    public interface IConfigurationBinder
    {
        /// <summary>
        /// Gets the label of this configuration binder
        /// </summary>
        /// <returns>Configuration binder's label</returns>
        string GetLabel();

        /// <summary>
        /// Adds a configuration
        /// </summary>
        /// <param name="configuration">Configuration to be added</param>
        void AddConfiguration(ConfigurationItem configuration);

        /// <summary>
        /// Removes a configuration
        /// </summary>
        /// <param name="key">Configuration key</param>
        void RemoveConfiguration(string key);

        /// <summary>
        /// Marks a configuration as transient
        /// </summary>
        /// <param name="key">Configuration key</param>
        void MarkAsTransient(string key);

        /// <summary>
        /// Marks a configuration as persisted
        /// </summary>
        /// <param name="key">Configuration key</param>
        void UnmarkAsTransient(string key);

        /// <summary>
        /// Marks all available configuration as transient
        /// </summary>
        void MarkAllAsTransient();

        /// <summary>
        /// Marks all available configuration as persisted
        /// </summary>
        void MarkAllAsPersisted();

        /// <summary>
        /// Removes all configuration marked as persisted
        /// </summary>
        void RemoveNonPersistedConfigurations();

        /// <summary>
        /// Gets a configuration
        /// </summary>
        /// <param name="key">Configuration key</param>
        /// <returns>Matched configuration</returns>
        ConfigurationItem GetConfiguration(string key);

        /// <summary>
        /// Gets available configurations
        /// </summary>
        /// <returns>Configuration instances</returns>        
        ConfigurationItem[] GetConfigurations();

        /// <summary>
        /// Get available configuration keys
        /// </summary>
        /// <returns></returns>
        IList<string> GetKeys();

        /// <summary>
        /// Saves configurations to persistence media.
        /// </summary>
        void Save();

        /// <summary>
        /// Loads configurations from persistence media.
        /// </summary>
        void Load();
    }
}
