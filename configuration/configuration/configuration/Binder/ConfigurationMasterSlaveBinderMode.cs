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

namespace Toyota.Common.Configuration
{
    /// <summary>
    /// Master-Slave Configuration Binder operation mode
    /// </summary>
    public enum ConfigurationMasterSlaveBinderMode
    {
        /// <summary>
        /// States that the operation will be performed by using Master binder as priority.
        /// </summary>
        Master,

        /// <summary>
        /// States that the operation will be performed by using Slave binder as priority.
        /// </summary>
        Slave
    }
}
