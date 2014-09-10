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
    public interface IBroadcastingStation
    {
        string GetName();

        void RegisterTopic(IBroadcastingTopic topic);
        void RemoveTopic(IBroadcastingTopic topic);
        void RemoveTopic(string name);
        IBroadcastingTopic GetTopic(string name);
        bool HasTopic(string name);

        void Broadcast(params IBroadcastingPacket[] packets);
        IBroadcastingPacket GeneratePacket(string topicName, object data);
    }
}
