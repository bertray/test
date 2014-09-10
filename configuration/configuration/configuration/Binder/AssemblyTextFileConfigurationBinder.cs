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
using System.Reflection;

namespace Toyota.Common.Configuration.Binder
{
    /// <summary>
    /// An implementation of configuration binder that
    /// persists configurations into an embedded text file.
    /// </summary>
    public class AssemblyTextFileConfigurationBinder: BaseTextFileConfigurationBinder
    {        
        private string namespacePath;
        private Assembly assembly;
                
        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="label">Label of the configuration binder</param>
        /// <param name="rootNamespace">Root namespace where the text file placed</param>
        /// <param name="assembly">Assembly context where the text file embedded into</param>
        public AssemblyTextFileConfigurationBinder(string label, string rootNamespace, Assembly assembly) : base(label) 
        {
            this.namespacePath = rootNamespace;
            this.assembly = assembly;
        }

        /// <summary>
        /// <see cref="Toyota.Common.Configuration.IConfigurationBinder"/>
        /// </summary>
        public override void Save()
        {
            
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
            Stream stream = assembly.GetManifestResourceStream(assemblyPath);
            Load(stream);
        }

    }
}
