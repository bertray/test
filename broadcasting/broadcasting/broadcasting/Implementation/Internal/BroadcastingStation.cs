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
    public class BroadcastingStation: IBroadcastingStation
    {
        private string name;
        private IDictionary<string, IBroadcastingTopic> topics;
        private Random random;

        public BroadcastingStation(string name)
        {
            this.name = name;
            this.random = new Random();
            topics = new Dictionary<string, IBroadcastingTopic>();            
        }

        public string GetName()
        {
            return name;
        }

        public void RegisterTopic(IBroadcastingTopic topic)
        {
            string name = topic.GetName();
            if (!topics.ContainsKey(name))
            {
                topics.Add(name, topic);
            }
        }

        public void RemoveTopic(IBroadcastingTopic topic)
        {
            RemoveTopic(topic.GetName());            
        }

        public void RemoveTopic(string name)
        {
            if (topics.ContainsKey(name))
            {
                topics.Remove(name);
            }
        }

        public IBroadcastingTopic GetTopic(string name)
        {
            if (topics.ContainsKey(name))
            {
                return topics[name];
            }

            return null;
        }

        public bool HasTopic(string name)
        {
            return topics.ContainsKey(name);
        }

        public void Broadcast(params IBroadcastingPacket[] packets)
        {
            if ((packets != null) && (packets.Length > 0) && (topics.Count > 0))
            {
                string topicName;
                IBroadcastingTopic topic;
                IList<IBroadcastingTopicListener> listeners;
                IBroadcastingTopicListener listener;
                foreach (IBroadcastingPacket p in packets)
                {
                    topicName = p.GetTopicName();
                    if (topics.ContainsKey(topicName))
                    {
                        topic = topics[topicName];
                        listeners = topic.GetListeners();
                        if (listeners != null)
                        {
                            for (int i = listeners.Count - 1; i >= 0; i--)
                            {
                                listener = listeners[i];
                                listener.PacketBroadcasted(p);
                            }
                        }
                    }
                }
            }
        }

        public IBroadcastingPacket GeneratePacket(string topicName, object data)
        {
            return new BroadcastingPacket(Guid.NewGuid().ToString(), topicName, this, data);
        }
    }
}
