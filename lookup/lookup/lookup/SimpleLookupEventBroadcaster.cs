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
    public class SimpleLookupEventBroadcaster: ILookupEventBroadcaster
    {
        private List<ILookupEventListener> listeners;
        private List<Action<LookupEvent>> actions;

        public SimpleLookupEventBroadcaster()
        {
            listeners = new List<ILookupEventListener>();
            actions = new List<Action<LookupEvent>>();
        }

        public void AddEventListener(ILookupEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }
        public void RemoveEventListener(ILookupEventListener listener)
        {
            listeners.Remove(listener);
        }
        public void AddEventListener(Action<LookupEvent> action)
        {
            if ((action != null) && (!actions.Contains(action)))
            {
                actions.Add(action);
            }
        }
        public void RemoveEventListener(Action<LookupEvent> action)
        {
            if (action != null)
            {
                actions.Remove(action);
            }            
        }

        protected void BroadcastEvent(LookupEvent evt)
        {
            if (listeners.Count > 0)
            {
                IList<ILookupEventListener> _listeners;
                lock (listeners)
                {
                    _listeners = listeners.AsReadOnly();
                }
                if (_listeners != null)
                {
                    foreach (ILookupEventListener l in _listeners)
                    {
                        l.LookupChanged(evt);
                    }
                }                

                IList<Action<LookupEvent>> _actions;
                lock (actions)
                {
                    _actions = actions.AsReadOnly();
                }
                if (_actions != null)
                {
                    foreach (Action<LookupEvent> act in _actions)
                    {
                        act.Invoke(evt);
                    }
                }
            }
        }

        public IList<ILookupEventListener> GetListeners()
        {
            return listeners.AsReadOnly();
        }

        public IList<Action<LookupEvent>> GetActionListener()
        {
            return actions.AsReadOnly();
        }
    }
}
