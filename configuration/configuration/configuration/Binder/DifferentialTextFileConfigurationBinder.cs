using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Toyota.Common.Configuration.Binder
{
    /// <summary>
    /// An implementation of configuration binder that
    /// persists configurations into a text file.
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
    public class DifferentialTextFileConfigurationBinder : BaseTextFileConfigurationBinder
    {
        private string marker;
        private string path;

        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="label">Label of the binder</param>
        /// <param name="marker">Marker of the binder that resembles a unique differentiation id</param>
        /// <param name="path">The path where the file stored</param>
        public DifferentialTextFileConfigurationBinder(string label, string marker, string path)
            : base(label)
        {
            this.marker = marker;
            this.path = path;
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
            if (!string.IsNullOrEmpty(marker))
            {
                fullPath = path + "\\" + GetLabel() + "-" + marker + "." + EXTENSION;
            }

            File.WriteAllText(fullPath, string.Empty);
            Stream stream = File.OpenWrite(fullPath);
            Save(stream);
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
            if (!string.IsNullOrEmpty(marker))
            {
                fullPath = path + "\\" + GetLabel() + "-" + marker + "." + EXTENSION;
            }

            if (File.Exists(fullPath))
            {
                Stream stream = File.OpenRead(fullPath);
                Load(stream);
            }
        }
    }
}
