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

namespace Toyota.Common.Configuration.Binder
{
    /// <summary>
    ///  An implementation of configuration binder specification
    ///  that does not persists any of its configuration into persistence media.
    ///  In formal understanding, it means the data will be kept in the memory
    ///  as long as the application need it.
    /// </summary>
    public class VolatileConfigurationBinder: ConfigurationBinder
    {
        public VolatileConfigurationBinder(string label) : base(label) { }

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
            
        }
    }
}
