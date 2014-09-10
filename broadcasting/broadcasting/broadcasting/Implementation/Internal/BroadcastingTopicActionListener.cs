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
    public class BroadcastingTopicActionListener: IBroadcastingTopicListener
    {
        private Action<IBroadcastingPacket> action;

        public BroadcastingTopicActionListener(Action<IBroadcastingPacket> action)
        {
            this.action = action;
        }

        public void PacketBroadcasted(IBroadcastingPacket packet)
        {
            if (action != null)
            {
                action.Invoke(packet);
            }
        }
    }
}
