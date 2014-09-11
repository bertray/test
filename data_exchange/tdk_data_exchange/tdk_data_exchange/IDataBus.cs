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
    public interface IDataBus
    {
        string GetName();

        void AddEventListener(IDataBusEventListener listener);
        void RemoveEventListener(IDataBusEventListener listener);

        void Push(DataPacket packets);
        DataPacket Pull(string id, IDictionary<string, object> parameters);

        void Close();
        void Open();
    }
}
