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
using Toyota.Common.Broadcasting;

namespace Toyota.Common.Web.Platform
{
    public class ApplicationMessageBus: IBroadcastingTerminal
    {
        private static readonly ApplicationMessageBus instance = new ApplicationMessageBus();
        private IBroadcastingTerminal terminal = null;

        private ApplicationMessageBus() 
        {
            terminal = ProviderRegistry.Instance.Get<IBroadcastingTerminal>();
        }

        public static ApplicationMessageBus Instance
        {
            get
            {
                return instance;
            }
        }

        public IBroadcastingStation SystemStation
        {
            get
            {
                return terminal.GetStation("System");
            }
        }

        public string GetName()
        {
            return terminal.GetName();
        }

        public void RegisterStation(IBroadcastingStation station)
        {
            terminal.RegisterStation(station);
        }

        public void RemoveStation(IBroadcastingStation station)
        {
            terminal.RemoveStation(station);
        }

        public IBroadcastingStation GetStation(string name)
        {
            return terminal.GetStation(name);
        }

        public IList<IBroadcastingStation> GetStations()
        {
            return terminal.GetStations();
        }

        public void Broadcast(params IBroadcastingPacket[] packets)
        {
            terminal.Broadcast(packets);
        }
    }
}
