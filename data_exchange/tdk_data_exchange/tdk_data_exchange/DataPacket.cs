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
using System.IO;

namespace Toyota.Common.DataExchange
{
    public class DataPacket: IDisposable
    {
        public DataPacket(string id, string busName, Action<IDictionary<string, object>> actionParameters)
        {
            Parameters = new Dictionary<string, object>();
            if (actionParameters != null)
            {
                actionParameters.Invoke(Parameters);
            }
            if (string.IsNullOrEmpty(id))
            {
                Id = Guid.NewGuid().ToString();
            }            
            BusName = busName;
        }
        public DataPacket(string busName, Action<IDictionary<string, object>> actionParameters) : this(null, busName, actionParameters) { }
        public DataPacket(string busName) : this(null, busName, null) { }

        public string Id { private set;  get; }
        public string BusName { private set; get; }
        public IDictionary<string, object> Parameters { private set; get; }

        public object Data { set; get; }
        public virtual void Dispose() 
        {
            if ((Data != null) && (Data is Stream))
            {
                Stream stream = (Stream)Data;
                stream.Close();
            }
        }
    }
}
