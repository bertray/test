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
    [Serializable]
    public class SimpleProxyLookupEventBroadcaster: IProxyLookupEventBroadcaster
    {
        private List<IProxyLookupEventListener> listeners;
        private List<Action<ProxyLookupEvent>> actions;

        public SimpleProxyLookupEventBroadcaster()
        {
            listeners = new List<IProxyLookupEventListener>();
            actions = new List<Action<ProxyLookupEvent>>();
        }

        public void AddEventListener(IProxyLookupEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }
        public void RemoveEventListener(IProxyLookupEventListener listener)
        {
            listeners.Remove(listener);
        }
        public void AddEventListener(Action<ProxyLookupEvent> action)
        {
            if ((action != null) && (!actions.Contains(action)))
            {
                actions.Add(action);
            }
        }
        public void RemoveEventListener(Action<ProxyLookupEvent> action)
        {
            if (action != null)
            {
                actions.Remove(action);
            }
        }

        protected void BroadcastEvent(ProxyLookupEvent evt)
        {
            if (listeners.Count > 0)
            {
                IList<IProxyLookupEventListener> _listeners;
                lock (listeners)
                {
                    _listeners = listeners.AsReadOnly();
                }
                if (_listeners != null)
                {
                    foreach (IProxyLookupEventListener l in _listeners)
                    {
                        l.ProxyLookupChanged(evt);
                    }
                }

                IList<Action<ProxyLookupEvent>> _actions;
                lock (actions)
                {
                    _actions = actions.AsReadOnly();
                }
                if (_actions != null)
                {
                    foreach (Action<ProxyLookupEvent> act in _actions)
                    {
                        act.Invoke(evt);
                    }
                }
            }
        }        
    }
}
