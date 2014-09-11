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

namespace Toyota.Common.DataExchange
{
    public abstract class DataBus: IDataBus
    {
        private string name;
        private List<IDataBusEventListener> listeners;

        public DataBus(string name)
        {
            this.name = name;
            this.listeners = new List<IDataBusEventListener>();
        }

        public string GetName()
        {
            return name;
        }

        public void AddEventListener(IDataBusEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }
        public void RemoveEventListener(IDataBusEventListener listener)
        {
            listeners.Remove(listener);
        }
        protected void NotifyEventListener(DataBusEvent evt)
        {
            if (listeners.Count > 0)
            {
                IDataBusEventListener[] arrListeners = null;
                lock (listeners)
                {
                    arrListeners = listeners.ToArray();
                }

                foreach (IDataBusEventListener lst in arrListeners)
                {
                    lst.DataBusEventBroadcasted(evt);
                }
            }
        }

        public abstract void Push(DataPacket packet);
        public abstract DataPacket Pull(string id, IDictionary<string, object> parameters);
        public abstract void Close();
        public abstract void Open();
    }
}
