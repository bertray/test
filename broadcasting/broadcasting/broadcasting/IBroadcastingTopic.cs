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
    public interface IBroadcastingTopic
    {
        string GetName();

        void AddListener(IBroadcastingTopicListener listener);        
        void RemoveListener(IBroadcastingTopicListener listener);        
        IList<IBroadcastingTopicListener> GetListeners();
    }
}
