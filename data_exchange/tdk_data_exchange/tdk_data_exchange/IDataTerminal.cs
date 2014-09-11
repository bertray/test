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
    public interface IDataTerminal
    {
        void RegisterBus(IDataBus bus);
        void RemoveBus(IDataBus bus);
        void RemoveBus(string name);

        IDataBus GetBus(string name);
        IList<IDataBus> GetBuses();

        void Push(params DataPacket[] packets);
        DataPacket Pull(string busName, string id, IDictionary<string, object> parameters);
        DataPacket Pull(string busName, string id, Action<IDictionary<string, object>> actionParameter);
        void Open();
        void Close();
    }
}
