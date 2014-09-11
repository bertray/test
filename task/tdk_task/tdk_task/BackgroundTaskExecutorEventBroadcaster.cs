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
    public class BackgroundTaskExecutorEventBroadcaster
    {
        private List<IBackgroundTaskExecutorListener> Listeners;
        private Object ListenerLock { set; get; }

        public BackgroundTaskExecutorEventBroadcaster()
        {
            Listeners = new List<IBackgroundTaskExecutorListener>();
            ListenerLock = new Object();
        }

        public void AddListener(IBackgroundTaskExecutorListener listener)
        {
            if (!Listeners.Contains(listener))
            {
                Listeners.Add(listener);
            }
        }
        public void RemoveListener(IBackgroundTaskExecutorListener listener)
        {
            Listeners.Remove(listener);
        }
        public IList<IBackgroundTaskExecutorListener> GetListeners()
        {
            return Listeners.AsReadOnly();
        }
        protected void NotifyListeners(BackgroundTaskExecutorEvent evt) 
        {
            IBackgroundTaskExecutorListener[] listenerArray = null;
            lock (ListenerLock)
            {
                if (Listeners.Count > 0)
                {
                    listenerArray = Listeners.ToArray();
                }
            }

            if (listenerArray != null)
            {
                foreach (IBackgroundTaskExecutorListener listener in listenerArray)
                {
                    listener.BackgroundTaskExecutorChanged(evt);
                }
            }
        }
    }
}
