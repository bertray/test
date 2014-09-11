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
    public class DataTerminal: IDataTerminal
    {
        private IDictionary<string, IDataBus> buses;

        public DataTerminal()
        {
            buses = new Dictionary<string, IDataBus>();
        }

        public void RegisterBus(IDataBus bus)
        {
            if (bus != null)
            {
                string name = bus.GetName();
                if (buses.ContainsKey(name))
                {
                    buses[name] = bus;
                }
                else
                {
                    buses.Add(name, bus);
                }
            }
        }

        public void RemoveBus(IDataBus bus)
        {
            if (bus != null)
            {
                buses.Remove(bus.GetName());
            }
        }

        public void RemoveBus(string name)
        {
            if (buses.ContainsKey(name))
            {
                buses.Remove(name);
            }
        }

        public IDataBus GetBus(string name)
        {
            if (buses.ContainsKey(name))
            {
                return buses[name];
            }
            return null;
        }

        public IList<IDataBus> GetBuses()
        {
            return buses.Values.ToList().AsReadOnly();
        }

        public void Push(params DataPacket[] packets)
        {
            if ((packets != null) && (packets.Length > 0))
            {
                foreach (DataPacket packet in packets)
                {
                    if (buses.ContainsKey(packet.BusName))
                    {
                        buses[packet.BusName].Push(packet);
                    }
                }
            }            
        }
        public DataPacket Pull(string busName, string id, IDictionary<string, object> parameters)
        {
            if (buses.ContainsKey(busName))
            {
                return buses[busName].Pull(id, parameters);
            }
            return null;
        }
        public DataPacket Pull(string busName, string id, Action<IDictionary<string, object>> actionParameter)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            if (actionParameter != null)
            {
                actionParameter.Invoke(parameters);
            }
            return Pull(busName, id, parameters);
        }
        
        public void Open()
        {
            foreach (IDataBus bus in buses.Values)
            {
                bus.Open();
            }
        }

        public void Close()
        {
            foreach (IDataBus bus in buses.Values)
            {
                bus.Close();
            }
        }        
    }
}
