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
    public class BroadcastingTopic: IBroadcastingTopic
    {
        private string name;
        private List<IBroadcastingTopicListener> topicListeners;

        public BroadcastingTopic(string name)
        {
            this.name = name;
            topicListeners = new List<IBroadcastingTopicListener>();
        }

        public string Name { set; get; }

        public void AddListener(IBroadcastingTopicListener listener)
        {
            if (!topicListeners.Contains(listener))
            {
                topicListeners.Add(listener);
            }
        }
        public void RemoveListener(IBroadcastingTopicListener listener)
        {
            if (topicListeners.Contains(listener))
            {
                topicListeners.Remove(listener);
            }
        }
        public IList<IBroadcastingTopicListener> GetListeners()
        {
            if (topicListeners.Count > 0)
            {
                return topicListeners.AsReadOnly();
            }

            return null;
        }

        public string GetName()
        {
            return name;
        }       
    }
}
