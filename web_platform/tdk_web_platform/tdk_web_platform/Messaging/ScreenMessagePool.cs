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
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Platform
{
    public class ScreenMessagePool: IClearable
    {
        private List<ScreenMessage> messageList;

        public ScreenMessagePool()
        {
            messageList = new List<ScreenMessage>();
        }

        public void Submit(params ScreenMessage[] message)
        {
            if (!message.IsNullOrEmpty())
            {
                foreach (ScreenMessage msg in message)
                {
                    if (!messageList.Contains(msg))
                    {
                        messageList.Add(msg);
                    }
                }
            }            
        }

        public void Remove(ScreenMessage message)
        {
            messageList.Remove(message);
        }

        public IList<ScreenMessage> Pull()
        {
            List<ScreenMessage> result = new List<ScreenMessage>(messageList);
            Clear(null);
            return result;
        }

        public void Clear(object param)
        {
            messageList.Clear();
        }
    }
}
