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

namespace Toyota.Common.Lookup
{
    public interface IProxyLookup: IProxyLookupEventBroadcaster
    {
        void AddLookup(ILookup lookup);
        ILookup GetLookup(string name);
        void RemoveLookup(ILookup lookup);
        void RemoveLookup(string name);

        string GetName();
        T Get<T>();
        IList<T> GetAll<T>();

        void Remove(object obj);
        void Remove<T>();
        void Remove<T>(Predicate<T> matchedCondition);
    }
}
