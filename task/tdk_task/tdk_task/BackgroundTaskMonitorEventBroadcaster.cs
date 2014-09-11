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
    public class BackgroundTaskMonitorEventBroadcaster
    {
        private Object listenerLock;
        private List<IBackgroundTaskMonitorListener> listeners;

        public BackgroundTaskMonitorEventBroadcaster()
        {
            listeners = new List<IBackgroundTaskMonitorListener>();
            listenerLock = new Object();
        }

        public void AddListener(IBackgroundTaskMonitorListener lst)
        {
            if (!listeners.Contains(lst))
            {
                listeners.Add(lst);
            }
        }
        public void RemoveListener(IBackgroundTaskMonitorListener lst)
        {
            listeners.Remove(lst);
        }
        public IList<IBackgroundTaskMonitorListener> GetListeners()
        {
            return listeners.AsReadOnly();
        }
        protected void NotifyListeners(BackgroundTaskMonitorEvent evt)
        {
            IBackgroundTaskMonitorListener[] listenerArray = null;
            lock (listenerLock)
            {
                if (listeners.Count > 0)
                {
                    listenerArray = listeners.ToArray();
                }                
            }

            if (listenerArray != null)
            {
                foreach (IBackgroundTaskMonitorListener lst in listenerArray)
                {
                    lst.BackgroundTaskMonitorChanged(evt);
                }
            }
        }
    }
}
