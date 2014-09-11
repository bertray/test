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
using Toyota.Common.Lookup;

namespace Toyota.Common.Web.Platform
{
    public class ObjectPool
    {
        private static readonly ObjectPool instance = new ObjectPool();
        
        private ObjectPool() 
        {            
            Lookup = new SimpleProxyLookup();
            Factory = new InstanceManager();
            Factory.AttachToProxyLookup(Lookup);
        }

        public static ObjectPool Instance
        {
            get
            {
                return instance;
            }
        }

        public InstanceManager Factory { private set; get; }
        public IProxyLookup Lookup { private set; get; }
    }
}
