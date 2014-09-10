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
    /// An SQL script loader. 
    /// It is submitted to the implementater where the SQL script will be loaded from.
    /// </summary>
    public interface ISqlLoader
    {
        /// <summary>
        /// Loads SQL script by referring to its name.
        /// This method should suppress all exeption occurs when loading
        /// the SQL script so the program flow can be retain.
        /// If the loader cannot find any suitable script then just
        /// return null value to the caller.
        /// </summary>
        /// <param name="name">Name of SQL script</param>
        /// <returns>SQL script</returns>
        string LoadScript(string name);
    }
}
