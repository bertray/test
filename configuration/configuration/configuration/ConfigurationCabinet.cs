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
    /// Default implementation of a configuration cabinet.
    /// <see cref="Toyota.Common.Configuration.IConfigurationCabinet"/>
    /// 
    /// Configuration Cabinet stores one or more Configuration Binder.
    /// Think it like an office drawer cabinet which stores paper binders.
    /// Configuration Binder is like a paper binder which group one or more
    /// Configuration Item (which, in this analogy, is a paper).
    /// </summary>
    public class ConfigurationCabinet: IConfigurationCabinet
    {
        private string label;
        private IDictionary<string, IConfigurationBinder> binderMap;

        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="label">Configuration label</param>
        public ConfigurationCabinet(string label)
        {
            this.label = label;
            this.binderMap = new Dictionary<string, IConfigurationBinder>();
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationCabinet"/>
        /// </summary>
        public void AddBinder(IConfigurationBinder binder)
        {
            string label = binder.GetLabel();
            if (binderMap.ContainsKey(label))
            {
                binderMap[label] = binder;
            }
            else
            {
                binderMap.Add(label, binder);
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationCabinet"/>
        /// </summary>
        public IConfigurationBinder GetBinder(string label)
        {
            if (binderMap.ContainsKey(label))
            {
                return binderMap[label];
            }

            return null;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationCabinet"/>
        /// </summary>
        public IConfigurationBinder[] GetBinderByConfigurationKey(string key)
        {
            if (binderMap.Count > 0)
            {
                IList<IConfigurationBinder> matchedConfigurations = new List<IConfigurationBinder>();
                ConfigurationItem[] configurations;
                ConfigurationItem config;
                foreach (IConfigurationBinder binder in binderMap.Values)
                {
                    configurations = binder.GetConfigurations();
                    if (configurations != null)
                    {
                        for (int i = configurations.Length - 1; i >= 0; i--)
                        {
                            config = configurations[i];
                            if (config.Key.Equals(key))
                            {
                                matchedConfigurations.Add(binder);
                                break;
                            }
                        }
                    }
                }

                if (matchedConfigurations.Count > 0)
                {
                    return matchedConfigurations.ToArray();
                }
            }

            return null;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationCabinet"/>
        /// </summary>
        public IConfigurationBinder[] GetBinders()
        {
            if (binderMap.Count > 0)
            {
                return binderMap.Values.ToArray();
            }

            return null;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationCabinet"/>
        /// </summary>
        public string GetLabel()
        {
            return label;
        }
    }
}
