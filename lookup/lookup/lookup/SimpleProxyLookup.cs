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
    [Serializable]
    public class SimpleProxyLookup: SimpleProxyLookupEventBroadcaster, IProxyLookup
    {
        private IList<ILookup> lookups;
        private string name;

        public SimpleProxyLookup(string name)
        {
            this.name = name;
            lookups = new List<ILookup>();
        }
        public SimpleProxyLookup(): this("SimpleProxyLookup") { }

        public void AddLookup(ILookup lookup)
        {
            if (!lookups.Contains(lookup))
            {
                lookups.Add(lookup);
                BroadcastEvent(new ProxyLookupEvent() { 
                    Broadcaster = this,
                    Lookup = lookup,
                    Type = ProxyLookupEventType.Lookup_Added
                });
            }
        }
        public ILookup GetLookup(string name)
        {
            foreach (ILookup lookup in lookups)
            {
                if (lookup.GetName().Equals(name))
                {
                    return lookup;
                }
            }
            return null;
        }
        public void RemoveLookup(ILookup lookup)
        {
            lookups.Remove(lookup);
            BroadcastEvent(new ProxyLookupEvent()
            {
                Broadcaster = this,
                Lookup = lookup,
                Type = ProxyLookupEventType.Lookup_Removed
            });
        }
        public void RemoveLookup(string name)
        {
            ILookup lookup = GetLookup(name);
            RemoveLookup(lookup);
        }

        public string GetName()
        {
            return name;
        }

        public T Get<T>()
        {
            T lookupObject;
            foreach (ILookup lookup in lookups)
            {
                lookupObject = lookup.Get<T>();
                if (lookupObject != null)
                {
                    return lookupObject;
                }
            }

            object fallback = null;
            return (T) fallback;
        }
        public IList<T> GetAll<T>()
        {            
            List<T> resultList = new List<T>();
            IList<T> lookupObjectList;
            foreach (ILookup lookup in lookups)
            {
                lookupObjectList = lookup.GetAll<T>();
                if (lookupObjectList != null)
                {
                    resultList.AddRange(lookupObjectList);
                }
            }

            if (resultList.Count > 0)
            {
                return resultList;
            }

            resultList = null;
            return (IList<T>) resultList;
        }

        public void Remove(object obj)
        {
            foreach (ILookup lkp in lookups)
            {
                lkp.Remove(obj);
            }
        }

        public void Remove<T>()
        {
            foreach (ILookup lkp in lookups)
            {
                lkp.Remove<T>();
            }
        }

        public void Remove<T>(Predicate<T> matchedCondition)
        {
            foreach (ILookup lkp in lookups)
            {
                lkp.Remove<T>(matchedCondition);
            }
        }
    }
}
