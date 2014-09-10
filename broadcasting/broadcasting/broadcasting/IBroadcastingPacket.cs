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
    public interface IBroadcastingPacket
    {
        string GetId();
        string GetTopicName();        
        IBroadcastingStation GetStation();

        void SetData(object data);
        object GetData();
    }
}
