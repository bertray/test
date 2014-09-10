using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Toyota.Common.Configuration.Binder
{
    /// <summary>
    /// An implementation of configuration binder that
    /// persists configurations into an embedded text file.
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
    public class DifferentialAssemblyTextFileConfigurationBinder : BaseTextFileConfigurationBinder
    {
        private string namespacePath;
        private Assembly assembly;
        private string marker;

        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="label">Label of the binder</param>
        /// <param name="marker">Marker of the binder that resembles a unique differentiation id</param>
        /// <param name="rootNamespace">Root namespace where the text file placed</param>
        /// <param name="assembly">Assembly context where the text file embedded into</param>
        public DifferentialAssemblyTextFileConfigurationBinder(string label, string marker, string rootNamespace, Assembly assembly)
            : base(label) 
        {
            this.namespacePath = rootNamespace;
            this.assembly = assembly;
            this.marker = marker;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public override void Save()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public override void Load()
        {
            if (string.IsNullOrEmpty(namespacePath))
            {
                return;
            }
            if (assembly == null)
            {
                return;
            }

            string assemblyPath = namespacePath + "." + GetLabel() + "." + EXTENSION;
            if (!string.IsNullOrEmpty(marker))
            {
                assemblyPath = namespacePath + "." + GetLabel() + "-" + marker + "." + EXTENSION;
            }
            Stream stream = assembly.GetManifestResourceStream(assemblyPath);
            Load(stream);
        }
    }
}
