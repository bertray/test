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

namespace Toyota.Common.Broadcasting
{
    public class BroadcastingTerminal: IBroadcastingTerminal
    {
        private string name;
        private IDictionary<string, IBroadcastingStation> stations;

        public BroadcastingTerminal(string name)
        {
            this.name = name;
            stations = new Dictionary<string, IBroadcastingStation>();
        }

        public void RegisterStation(IBroadcastingStation station)
        {
            string name = station.GetName();
            if (!string.IsNullOrEmpty(name))
            {
                if (!stations.ContainsKey(name))
                {
                    stations.Add(name, station);
                }
                else
                {
                    stations[name] = station;
                }
            }
        }

        public void RemoveStation(IBroadcastingStation station)
        {
            string name = station.GetName();
            if(stations.ContainsKey(name)) 
            {
                stations.Remove(name);
            }
        }

        public IBroadcastingStation GetStation(string name)
        {
            if (stations.ContainsKey(name))
            {
                return stations[name];
            }
            return null;
        }

        public IList<IBroadcastingStation> GetStations()
        {
            if (stations.Count > 0)
            {
                return stations.Values.ToList().AsReadOnly();
            }

            return null;
        }

        public string GetName()
        {
            return name;
        }

        public void Broadcast(params IBroadcastingPacket[] packets)
        {
            if ((packets != null) && (stations.Count > 0))
            {
                foreach (IBroadcastingStation station in stations.Values)
                {
                    station.Broadcast(packets);
                }
            }
        }
    }
}
