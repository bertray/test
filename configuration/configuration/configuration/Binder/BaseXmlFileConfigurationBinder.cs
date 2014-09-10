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
using System.Xml;

namespace Toyota.Common.Configuration.Binder
{
    /// <summary>
    /// A partial implementation of configuration binder that
    /// persists configurations into xml file.
    /// </summary>
    public class BaseXmlFileConfigurationBinder: ConfigurationBinder
    {
        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="label">Label of the binder</param>
        public BaseXmlFileConfigurationBinder(string label) : base(label) { }

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
        protected void Load(Stream stream)
        {
            if (stream != null)
            {
                XmlDocument document = new XmlDocument();
                document.Load(stream);
                
                try
                {
                    XmlNodeList nodeList = document.GetElementsByTagName("configuration");
                    if ((nodeList != null) && (nodeList.Count > 0))
                    {                        
                        XmlNodeList subNodeList;
                        XmlNode subNode;
                        ConfigurationItem configItem;
                        XmlNode rootNode = nodeList.Item(0);
                        IList<ConfigurationItem> compositeItems;
                        XmlNodeList compositeItemNodeList;
                        int compositeValidElementCounter;
                        foreach (XmlNode childNode in rootNode.ChildNodes)
                        {
                            if ((childNode.NodeType != XmlNodeType.Comment) && childNode.Name.Equals("configuration-item"))
                            {
                                compositeItems = null;
                                compositeValidElementCounter = 0;
                                configItem = new ConfigurationItem();
                                subNodeList = childNode.ChildNodes;                                
                                for (int i = subNodeList.Count - 1; i >= 0; i--)
                                {                                    
                                    subNode = subNodeList[i];
                                    if(subNode.NodeType != XmlNodeType.Comment) 
                                    {
                                        if (subNode.Name.Equals("key"))
                                        {
                                            configItem.Key = subNode.InnerText.Trim();
                                        }
                                        else if (subNode.Name.Equals("value")) {
                                            compositeItemNodeList = subNode.ChildNodes;
                                            foreach (XmlNode compositeNode in compositeItemNodeList)
                                            {
                                                if (compositeNode is XmlElement)
                                                {
                                                    compositeValidElementCounter++;
                                                }
                                                if (compositeValidElementCounter > 0)
                                                {
                                                    break;
                                                }
                                            }

                                            if (compositeValidElementCounter > 0)
                                            {
                                                compositeItems = new List<ConfigurationItem>();
                                                foreach (XmlNode compositeNode in compositeItemNodeList)
                                                {
                                                    compositeItems.Add(new ConfigurationItem() { 
                                                        Key = compositeNode.Name.Trim(),
                                                        Value = compositeNode.InnerText.Trim()
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                configItem.Value = subNode.InnerText.Trim();                                            
                                            }                                            
                                        }
                                        else if (subNode.Name.Equals("description"))
                                        {
                                            configItem.Description = subNode.InnerText.Trim();
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(configItem.Key))
                                {
                                    if (compositeItems != null)
                                    {
                                        AddConfiguration(new CompositeConfigurationItem(configItem.Key, configItem.Description, compositeItems.ToArray()));
                                    }
                                    else
                                    {
                                        AddConfiguration(configItem);
                                    }                                    
                                }
                            }
                        }
                    }
                }
                finally
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Loads all configuration from a stream of persistence media.
        /// </summary>
        /// <param name="stream">Stream of persistence media to be used</param>
        protected void Save(Stream stream)
        {
            if (stream != null)
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "\t";
                XmlWriter writer = XmlWriter.Create(stream, settings);

                try
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("configuration");
                                        
                    ConfigurationItem[] configurations = GetConfigurations();
                    if (configurations != null)
                    {
                        CompositeConfigurationItem compositeConfig;
                        ConfigurationItem[] compositeItems;
                        bool composite;
                        IList<ConfigurationItem> volatileConfigs = new List<ConfigurationItem>();
                        foreach (ConfigurationItem config in configurations)
                        {
                            if (config.Transient)
                            {
                                volatileConfigs.Add(config);
                                continue;
                            }

                            composite = config is CompositeConfigurationItem;
                            writer.WriteStartElement("configuration-item");
                            writer.WriteElementString("key", config.Key);
                            if (composite)
                            {
                                writer.WriteStartElement("value");
                                compositeConfig = (CompositeConfigurationItem)config;
                                compositeItems = compositeConfig.GetItems();
                                if (compositeItems != null)
                                {
                                    foreach (ConfigurationItem item in compositeItems)
                                    {
                                        writer.WriteElementString(item.Key, item.Value);
                                    }
                                }
                                writer.WriteEndElement();
                            }
                            else
                            {
                                writer.WriteElementString("value", config.Value);
                            }
                            
                            writer.WriteElementString("description", config.Description);                            
                            writer.WriteEndElement();
                        }

                        foreach (ConfigurationItem config in volatileConfigs)
                        {
                            RemoveConfiguration(config.Key);
                        }
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                finally
                {
                    writer.Close();
                    stream.Close();
                }
            }
        }
    }
}
