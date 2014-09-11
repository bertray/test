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

namespace Toyota.Common.Task
{
    public class BackgroundTaskRegistryEventBroadcaster
    {
        private List<IBackgroundTaskRegistryListener> listeners;
        private Object listenerLock;

        public BackgroundTaskRegistryEventBroadcaster()
        {
            listeners = new List<IBackgroundTaskRegistryListener>();
            listenerLock = new Object();
        }

        public void AddListener(IBackgroundTaskRegistryListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void RemoveListener(IBackgroundTaskRegistryListener listener)
        {
            listeners.Remove(listener);
        }

        public IList<IBackgroundTaskRegistryListener> GetListeners()
        {
            return listeners.AsReadOnly();
        }

        protected void NotifyListeners(BackgroundTaskRegistryEvent evt)
        {
            IBackgroundTaskRegistryListener[] listenerArray = null;
            lock (listenerLock)
            {
                if (listeners.Count > 0)
                {
                    listenerArray = listeners.ToArray();
                }
            }

            if (listenerArray != null)
            {
                foreach (IBackgroundTaskRegistryListener listener in listenerArray)
                {
                    listener.BackgroundTaskRegistryChanged(evt);
                }
            }
        }
    }
}
