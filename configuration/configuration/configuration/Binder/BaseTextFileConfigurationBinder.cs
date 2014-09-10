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

namespace Toyota.Common.Configuration.Binder
{
    /// <summary>
    /// A partial implementation of configuration binder that
    /// persists configurations into text file.
    /// </summary>
    public abstract class BaseTextFileConfigurationBinder: ConfigurationBinder
    {
        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="label">Label of the binder</param>
        public BaseTextFileConfigurationBinder(string label) : base(label) { }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public override void Save() { }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public override void Load() { }

        /// <summary>
        /// Saves all configuration into a stream of persistence media.
        /// </summary>
        /// <param name="stream">Stream of persistence media to be used</param>
        protected void Save(Stream stream)
        {
            if (stream != null)
            {
                StreamWriter writer = new StreamWriter(stream);
                try
                {
                    ConfigurationItem[] configurations = GetConfigurations();
                    if (configurations != null)
                    {
                        IList<ConfigurationItem> volatileConfigs = new List<ConfigurationItem>();
                        foreach (ConfigurationItem config in configurations)
                        {
                            if (config.Transient)
                            {
                                volatileConfigs.Add(config);
                                continue;
                            }

                            writer.WriteLine(string.Format("# {0}", config.Description));
                            writer.WriteLine(string.Format(".{0} = {1}", config.Key, config.Value));
                            writer.WriteLine();
                        }

                        foreach (ConfigurationItem config in volatileConfigs)
                        {
                            RemoveConfiguration(config.Key);
                        }
                    }
                }
                finally
                {
                    writer.Close();
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Loads all configuration from a stream of persistence media.
        /// </summary>
        /// <param name="stream">Stream of persistence media to be used</param>
        protected void Load(Stream stream)
        {
            if (stream != null)
            {
                StreamReader reader = new StreamReader(stream);
                try
                {
                    string line;
                    string[] fractions;
                    string key;
                    string value;
                    ConfigurationItem configuration = null;
                    ConfigurationItem tempConfiguration = null;
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        line = line.Trim();
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (line.StartsWith("."))
                            {
                                fractions = line.Split('=');
                                if ((fractions != null) && (fractions.Length > 0))
                                {
                                    key = fractions[0].Substring(1, fractions[0].Length - 1).Trim();
                                    value = fractions[1].Trim();
                                    if (configuration == null)
                                    {
                                        configuration = new ConfigurationItem();
                                    }

                                    tempConfiguration = GetConfiguration(key);
                                    if (tempConfiguration != null)
                                    {
                                        configuration = tempConfiguration;
                                    }

                                    configuration.Key = key;
                                    configuration.Value = value;
                                    AddConfiguration(configuration);
                                    configuration = null;
                                }
                            }
                            else if (line.StartsWith("#"))
                            {
                                configuration = new ConfigurationItem();
                                configuration.Description = line.Substring(1, line.Length - 1).Trim();
                            }
                        }
                    }
                }
                finally
                {
                    reader.Close();
                    stream.Close();
                }
            }
        }
    }
}
