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

namespace Toyota.Common.Database
{
    /// <summary>
    /// Basic construct of file-based SQL loader. 
    /// It's using .sql extension.
    /// </summary>
    public abstract class BaseFileSqlLoader: ISqlLoader
    {
        protected const string EXTENSION = "sql";

        /// <summary>
        /// Constructs instance of this class
        /// </summary>
        /// <param name="rootPath">Root path of the SQL file</param>
        public BaseFileSqlLoader(string rootPath)
        {
            RootPath = rootPath;
        }

        /// <summary>
        /// Root path of the SQL file.
        /// </summary>
        public string RootPath { set; get; }

        /// <summary>
        /// <see cref="Toyota.Common.Database.ISqlLoader"/>
        /// </summary>
        public abstract string LoadScript(string name);
    }
}
