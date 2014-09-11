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

namespace Toyota.Common.Web.Platform
{
    public class ProviderRegistry
    {
        private static readonly ProviderRegistry instance = new ProviderRegistry();
        public static ProviderRegistry Instance
        {
            get
            {
                return instance;
            }
        }

        public void Register<T>(Type implementationType, params object[] arguments)
        {
            ObjectPool.Instance.Factory.RegisterSingleton<T>(implementationType, arguments);
        }

        public void Register<T>(Type implementationType)
        {
            Register<T>(implementationType, null);
        }

        public void Remove<T>()
        {
            ObjectPool.Instance.Factory.RemoveSingleton<T>();
        }

        public T Get<T>()
        {
            return ObjectPool.Instance.Factory.GetInstance<T>();
        }
    }
}
