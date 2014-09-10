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
            Object obj = null;
            for (int i = lookupBag.Count - 1; i >= 0; i--)
            {
                obj = lookupBag[i];
                if (type.IsInstanceOfType(obj))
                {
                    break;
                }
                else
                {
                }
                obj = null;
            }

            BroadcastEvent(new LookupEvent() { 
                Type = LookupEventType.Instance_Requested,
                Broadcaster = this,
                Instance = obj
            });
            return (T)obj;
        }
                
        public IList<T> GetAll<T>()
        {
            if (lookupBag.Count == 0)
            {
                return null;
            }

            Object obj;
            Type type = typeof(T);
            List<T> lstResult = new List<T>();
            for (int i = lookupBag.Count - 1; i >= 0; i--)
            {
                obj = lookupBag[i];
                if (type.IsInstanceOfType(obj))
                {
                    lstResult.Add((T)obj);
                }
            }

            BroadcastEvent(new LookupEvent() { 
                Broadcaster = this,
                Instance = lstResult,
                Type = LookupEventType.Instances_Requested
            });

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

        private bool _isEventSuppressed = true;
        public bool IsEventSuppressed
        {
            get
            {
                return _isEventSuppressed;
            }
            set
            {
                _isEventSuppressed = value;
            }
        }

        public void Clear()
        {
            lookupBag.Clear();
        }
    }
}