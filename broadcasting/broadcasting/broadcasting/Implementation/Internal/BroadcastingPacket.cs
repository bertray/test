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
    public class BroadcastingPacket: IBroadcastingPacket
    {
        private string id;
        private string topicName;
        private IBroadcastingStation station;
        private object data;

        public BroadcastingPacket(string topicName, IBroadcastingStation station, object data) : this(Guid.NewGuid().ToString(), topicName, station, data) { }
        public BroadcastingPacket(string id, string topicName, IBroadcastingStation station, object data)
        {
            this.id = id;
            this.topicName = topicName;
            this.data = data;
            this.station = station;
        }

        public string GetId()
        {
            return id;
        }

        public void SetData(object data)
        {
            this.data = data;
        }
        public object GetData()
        {
            return data;
        }

        public void SetTopicName(string name)
        {
            this.topicName = name;
        }
        public string GetTopicName()
        {
            return topicName;
        }

        public IBroadcastingStation GetStation()
        {
            return station;
        }
    }
}
