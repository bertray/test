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
    public interface IProxyLookupEventBroadcaster
    {
        void AddEventListener(IProxyLookupEventListener listener);
        void RemoveEventListener(IProxyLookupEventListener listener);
        void AddEventListener(Action<ProxyLookupEvent> action);
        void RemoveEventListener(Action<ProxyLookupEvent> action);
    }
}
