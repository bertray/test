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
    /// Types of database context's execution mode
    /// </summary>
    public enum DBContextExecutionMode
    {
        /// <summary>
        /// Direct means the database context will executes the SQL context as is.
        /// Hence if we pass a string of SQL script, then it will executes the string directly
        /// without consulting with the SQL Loaders.
        /// </summary>
        Direct, 

        /// <summary>
        /// ByName means the database context will consults with the SQL Loaders first
        /// before executing the SQL context, and then executes whatever SQL script
        /// returned by the SQL Loaders.
        /// </summary>
        ByName
    }
}
