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

namespace Toyota.Common.Lookup
{
    public class LookupEvent
    {
        public ILookup Broadcaster { set; get; }
        public LookupEventType Type { set; get; }
        public object Instance { set; get; }
    }
}
