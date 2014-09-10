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
    /// The binder will have a unique identifier to differentiate
    /// different context on the same schema.
    /// 
    /// <example>
    /// Imagine if we have three different development stages i.e. Development (DEV), Testing (TS),
    /// and Production (PROD), and we have a database configuration that may use different address
    /// between development states buat with the same configuration schema. 
    /// 
    /// Using this class, we can have three file named (for example) DB-DEV.config, DB-TS.config, and
    /// DB-PROD.config with the same configuration schema inside but with different database address
    /// configured. Then, we can use appropriate file by defining the marker of this binder passed 
    /// into the constructor.
    /// </example>
    /// </summary>
    public class DifferentialXmlConfigurationBinder : BaseXmlFileConfigurationBinder
    {
        private string path;
        private string marker;

        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="label">Label of the binder</param>
        /// <param name="marker">Marker of the binder that resembles a unique differentiation id</param>
        /// <param name="path">The path where the file stored</param>
        public DifferentialXmlConfigurationBinder(string label, string marker, string path)
            : base(label) 
        {
            this.path = path;
            this.marker = marker;
        }

        public override void Load()
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            string fullPath = path + "\\" + GetLabel() + "." + EXTENSION;
            if(!string.IsNullOrEmpty(marker)) 
            {
                fullPath = path + "\\" + GetLabel() + "-" + marker + "." + EXTENSION;
            }

            if (File.Exists(fullPath))
            {
                Stream stream = File.OpenRead(fullPath);
                Load(stream);
            }
        }

        public override void Save()
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            string fullPath = path + "\\" + GetLabel() + "." + EXTENSION;
            if (!string.IsNullOrEmpty(marker))
            {
                fullPath = path + "\\" + GetLabel() + "-" + marker + "." + EXTENSION;
            }

            File.WriteAllText(fullPath, string.Empty);
            Stream stream = File.OpenWrite(fullPath);
            Save(stream);
        }
    }
}
