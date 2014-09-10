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
    /// An implementation of configuration binder specification that bridge
    /// two configurations with Master-Slave style organization. 
    /// The Master Binder acts as the primary binder and backed-up 
    /// by the Slave Binder. If one or more configuration requested
    /// don't exist in the Master Binder, then the Slave Binder will be consulted.
    /// 
    /// NOTE: THIS CLASS IS STILL IN ALPHA STAGE
    /// </summary>
    public class ConfigurationMasterSlaveBinder: IConfigurationBinder
    {
        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="label"></param>
        public ConfigurationMasterSlaveBinder(string label)
        {
            this.label = label;
            Mode = ConfigurationMasterSlaveBinderMode.Master;
        }

        /// <summary>
        /// Master Configuration Binder
        /// </summary>
        public IConfigurationBinder Master { set; get; }

        /// <summary>
        /// Slave Configuration Binder
        /// </summary>
        public IConfigurationBinder Slave { set; get; }

        /// <summary>
        /// Binder's mode. This determines what binder to be prioritized,
        /// the master or the slave. Particularly used by <c>AddConfiguration</c>, <see cref="AddConfiguration"/>.
        /// </summary>
        public ConfigurationMasterSlaveBinderMode Mode { set; get; }

        private string label;
        public string GetLabel()
        {
            return label;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>.
        /// If the mode is Master then the passed configuration will be added to master configuration, vice versa.
        /// </summary>
        public void AddConfiguration(ConfigurationItem configuration)
        {
            if (Mode == ConfigurationMasterSlaveBinderMode.Master)
            {
                if (Master != null)
                {
                    Master.AddConfiguration(configuration);
                }
            }
            else
            {
                if (Slave != null)
                {
                    Slave.AddConfiguration(configuration);
                }
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>.
        /// Master binder will be consulted first, if nothing found then
        /// the Slave will be consulted.
        /// </summary>
        public ConfigurationItem GetConfiguration(string key)
        {
            ConfigurationItem config = null;
            if (Master != null)
            {
                config = Master.GetConfiguration(key);
            }

            if ((config == null) && (Slave != null))
            {
                config = Slave.GetConfiguration(key);
            }

            return config;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>.
        /// The returned results will be the combination from the Master and the Slave.
        /// </summary>
        public ConfigurationItem[] GetConfigurations()
        {
            List<ConfigurationItem> result = new List<ConfigurationItem>();            
            if (Slave != null)
            {
                ConfigurationItem[] slaveConfigs = Slave.GetConfigurations();
                if (slaveConfigs != null)
                {
                    result.AddRange(slaveConfigs);
                }
            }

            if (Master != null)
            {
                ConfigurationItem[] masterConfigs = Master.GetConfigurations();
                if (masterConfigs != null)
                {
                    List<ConfigurationItem> removedSlaveConfigs = new List<ConfigurationItem>();
                    string masterKey;
                    foreach (ConfigurationItem masterConfig in masterConfigs)
                    {
                        masterKey = masterConfig.Key;
                        foreach (ConfigurationItem slaveConfig in result)
                        {
                            if (slaveConfig.Key.Equals(masterKey))
                            {
                                removedSlaveConfigs.Add(slaveConfig);
                            }
                        }
                    }

                    foreach (ConfigurationItem removedItem in removedSlaveConfigs)
                    {
                        result.Remove(removedItem);
                    }

                    result.AddRange(masterConfigs);
                }
            }

            if (result.Count > 0)
            {
                return result.ToArray();
            }
            return null;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>.
        /// </summary>
        public void Save()
        {
            if (Mode == ConfigurationMasterSlaveBinderMode.Master)
            {
                if (Master != null)
                {
                    Master.Save();
                }
            }
            else
            {
                if (Slave != null)
                {
                    Slave.Save();
                }
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>.
        /// </summary>
        public void Load()
        {
            if (Mode == ConfigurationMasterSlaveBinderMode.Master)
            {
                if (Master != null)
                {
                    Master.Load();
                }
            }
            else
            {
                if (Slave != null)
                {
                    Slave.Load();
                }
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>.
        /// </summary>
        public IList<string> GetKeys()
        {
            List<string> result = new List<string>();

            if (Master != null)
            {
                IList<string> masterKeys = Master.GetKeys();
                if (masterKeys != null)
                {
                    result.AddRange(masterKeys);
                }
            }

            if (Slave != null)
            {
                IList<string> slaveKeys = Slave.GetKeys();
                if (slaveKeys != null)
                {
                    foreach (string key in slaveKeys)
                    {
                        if (!result.Contains(key))
                        {
                            result.Add(key);
                        }
                    }
                }
            }

            if (result.Count > 0)
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>.
        /// </summary>
        public void RemoveConfiguration(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>.
        /// </summary>
        public void MarkAsTransient(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>.
        /// </summary>
        public void UnmarkAsTransient(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>.
        /// </summary>
        public void MarkAllAsTransient()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>.
        /// </summary>
        public void MarkAllAsPersisted()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>.
        /// </summary>
        public void RemoveNonPersistedConfigurations()
        {
            throw new NotImplementedException();
        }
    }
}
