///
/// <author>lufty.abdillah@gmail.com</author>
/// <summary>
/// Toyota .Net Development Kit
/// Copyright (c) Toyota Motor Manufacturing Indonesia, All Right Reserved.
/// </summary>
/// 
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Toyota.Common.Lookup
{
    [Serializable]
    public class SimpleLookup : SimpleLookupEventBroadcaster, ILookup
    {
        private List<object> lookupBag;
        private string name;

        public SimpleLookup(string name)
        {
            this.name = name;
            lookupBag = new List<object>();
        }

        public SimpleLookup(): this("SimpleLookup") { }
                
        public void Add(object obj)
        {
            if (obj != null)
            {
                lookupBag.Add(obj);
                BroadcastEvent(new LookupEvent() { 
                    Broadcaster = this,
                    Type = LookupEventType.Instance_Added,
                    Instance = obj
                });
            }
        }
                
        public void Remove(object obj)
        {
            if (obj != null)
            {
                lookupBag.Remove(obj);
                BroadcastEvent(new LookupEvent()
                {
                    Broadcaster = this,
                    Type = LookupEventType.Instance_Removed,
                    Instance = obj
                });
            }
        }
                
        public T Get<T>()
        {
            if (lookupBag.Count == 0)
            {
                return default(T);
            }

            Type type = typeof(T);
            foreach (Object obj in lookupBag)
            {
                if (type.IsInstanceOfType(obj))
                {
                    return (T) obj;
                }
            }
            return default(T);
        }
                
        public IList<T> GetAll<T>()
        {
            if (lookupBag.Count == 0)
            {
                return null;
            }
                        
            Type type = typeof(T);
            List<T> lstResult = new List<T>();
            foreach (Object obj in lookupBag)
            {
                if (type.IsInstanceOfType(obj))
                {
                    lstResult.Add((T)obj);
                }
            }

            if (lstResult.Count > 0)
            {
                return lstResult;
            }

            return null;
        }                

        public void Remove<T>()
        {
            IList<T> objList = GetAll<T>();
            if (objList != null)
            {
                foreach (T obj in objList)
                {
                    lookupBag.Remove(obj);
                    BroadcastEvent(new LookupEvent()
                    {
                        Broadcaster = this,
                        Type = LookupEventType.Instance_Removed,
                        Instance = obj
                    });
                }
            }
        }

        public string GetName()
        {
            return name;
        }

        public void Remove<T>(Predicate<T> matchedCondition)
        {
            IList<T> foundList = GetAll<T>();
            if ((foundList != null) && (foundList.Count > 0))
            {
                IList<T> removed = new List<T>();
                foreach (T tObj in foundList)
                {
                    if (matchedCondition.Invoke(tObj))
                    {
                        removed.Add(tObj);
                    }
                }
                foreach (T tObj in removed)
                {
                    Remove(tObj);
                }
            }
        }

        public void Clear()
        {
            lookupBag.Clear();
        }

        public IList<object> GetAll()
        {
            return lookupBag;
        }
    }
}