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
    /// An implementation of configuration binder that
    /// persists configurations into an xml file.
    /// </summary>
    public class XmlFileConfigurationBinder: BaseXmlFileConfigurationBinder
    {
        private string path;
        public XmlFileConfigurationBinder(string label, string path) : base(label) 
        {
            this.path = path;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public override void Load()
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            string fullPath = path + "\\" + GetLabel() + "." + EXTENSION;
            if (File.Exists(fullPath))
            {
                Stream stream = File.OpenRead(fullPath);
                Load(stream);
            }
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public override void Save()
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            string fullPath = path + "\\" + GetLabel() + "." + EXTENSION;
            File.WriteAllText(fullPath, string.Empty);
            Stream stream = File.OpenWrite(fullPath);
            Save(stream);
        }
    }
}
