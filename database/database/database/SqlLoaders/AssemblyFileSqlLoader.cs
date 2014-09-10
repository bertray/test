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
using System.Reflection;
using System.IO;

namespace Toyota.Common.Database
{
    /// <summary>
    /// Loads SQL script from .sql files inside an assembly.
    /// </summary>
    public class AssemblyFileSqlLoader: BaseFileSqlLoader
    {        
        private Assembly assembly;           

        /// <summary>
        /// Creates instance of this class.
        /// </summary>
        /// <param name="assembly">Assembly container of the script files</param>
        /// <param name="rootNamespace">Root namespace which contains the script files</param>
        /// <param name="extension">Script files extension</param>
        public AssemblyFileSqlLoader(Assembly assembly, string rootNamespace): base(rootNamespace)
        {
            this.assembly = assembly;            
        }

        /// <summary>
        /// <see cref="Toyota.Common.Database.ISqlLoader"/>
        /// </summary>
        public override string LoadScript(string name)
        {
            string fileName = name + "." + EXTENSION;
            Stream stream = assembly.GetManifestResourceStream(RootPath + "." + fileName);
            if(stream != null) {
                StreamReader reader = new StreamReader(stream);
                string query = reader.ReadToEnd();
                reader.Close();
                return query.Trim();
            }
            return null;
        }
    }
}
