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
    public interface IBroadcastingTerminal
    {
        string GetName();

        void RegisterStation(IBroadcastingStation station);
        void RemoveStation(IBroadcastingStation station);
        IBroadcastingStation GetStation(string name);
        IList<IBroadcastingStation> GetStations();

        void Broadcast(params IBroadcastingPacket[] packets);
    }
}
