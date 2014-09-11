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

namespace Toyota.Common.Logging
{
    internal class LoggingSinkType
    {
        public Type Type { set; get; }
        public string Name { set; get; }
        public bool IsDefault { set; get; }
        public object[] Arguments { set; get; }
    }
}
