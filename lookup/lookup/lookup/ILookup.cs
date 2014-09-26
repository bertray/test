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
    public interface ILookup: ILookupEventBroadcaster
    {
        string GetName();
        void Add(object obj);
        void Remove(object obj);
        void Remove<T>();
        void Remove<T>(Predicate<T> matchedCondition);
        T Get<T>();
        IList<T> GetAll<T>();
        IList<object> GetAll();

        bool IsEventSuppressed { set; get; }
        void Clear();
    }
}
