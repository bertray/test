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

namespace Toyota.Common.Messaging
{
    public class ThreadedMessage: Message
    {
        public ThreadedMessage()
        {
            Messages = new List<Message>();
        }

        public IList<Message> Messages { set; get; }
    }
}
