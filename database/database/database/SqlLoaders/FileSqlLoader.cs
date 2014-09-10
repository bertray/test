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

namespace Toyota.Common.Database
{
    /// <summary>
    /// Loads SQL script from a .sql file.
    /// </summary>
    public class FileSqlLoader: BaseFileSqlLoader
    {
        /// <summary>
        /// Creates instance of this class
        /// </summary>
        /// <param name="rootPath">Root path of the SQL file</param>
        public FileSqlLoader(string rootPath) : base(rootPath) { }

        /// <summary>
        /// This loader will only load SQL files inside the root path 
        /// by one level, it will not search subdirectories inside the root path.
        /// <see cref="Toyota.Common.Database.ISqlLoader"/>
        /// </summary>
        public override string LoadScript(string name)
        {
            string filePath = RootPath + "\\" + name + "." + EXTENSION;
            FileStream stream = null;
            StreamReader reader = null;
            string result = null;
            try
            {
                stream = File.OpenRead(filePath);
                reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }
            catch (Exception ex) 
            { 
            }
            finally 
            {
                if (stream != null)
                {
                    stream.Close();
                    if (reader != null)
                    {
                        reader.Close();
                    } 
                }
            }

            return result;
        }
    }
}
