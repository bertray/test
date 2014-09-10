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
    public class InstanceManager
    {
        private ILookup lookup;
        private IDictionary<Type, Type> implementationMap;

        public InstanceManager()
        {
            lookup = new SimpleLookup();
            implementationMap = new Dictionary<Type, Type>();
        }

        public void RegisterSingleton<T>(Type implementationType, params object[] arguments)
        {
            IList<T> result = lookup.GetAll<T>();
            if (result != null)
            {
                foreach (T obj in result)
                {
                    lookup.Remove(obj);
                }
            }
            Type type = Type.GetType(implementationType.AssemblyQualifiedName);
            object objInstance = null;
            if ((arguments != null) && (arguments.Length > 0))
            {
                objInstance = (T)Activator.CreateInstance(type, arguments);                
            } else {
                objInstance = (T)Activator.CreateInstance(type);
            }
                
            if (objInstance != null)
            {
                lookup.Add(objInstance);
            }
        }

        public void RemoveSingleton<T>()
        {
            lookup.Remove<T>();
        }

        public void Register<T>(Type implementationType)
        {
            if (implementationType == null)
            {
                return;
            }

            Type type = typeof(T);
            if (implementationMap.ContainsKey(type))
            {
                implementationMap[type] = implementationType;
            }
            else
            {
                implementationMap.Add(type, implementationType);
            }
        }

        public void Remove<T>()
        {
            Type type = typeof(T);
            if (implementationMap.ContainsKey(type))
            {
                implementationMap.Remove(type);
            }
        }

        public T GetInstance<T>(params object[] arguments)
        {
            T obj = lookup.Get<T>();
            if (obj == null)
            {
                Type type = typeof(T);
                if (implementationMap.ContainsKey(type))
                {
                    if ((arguments != null) && (arguments.Length > 0))
                    {
                        obj = (T)Activator.CreateInstance(type, arguments);
                    }
                    else
                    {
                        obj = (T)Activator.CreateInstance(type);
                    }                    
                }
            }

            return obj;
        }

        public T GetInstance<T>()
        {
            return GetInstance<T>(null);
        }

        public void AttachToProxyLookup(IProxyLookup proxyLookup)
        {
            proxyLookup.AddLookup(lookup);
        }

        public void DetachFromProxyLookup(IProxyLookup proxyLookup)
        {
            proxyLookup.RemoveLookup(lookup);
        }
    }
}
