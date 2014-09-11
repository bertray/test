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
using Toyota.Common.Credential;

namespace Toyota.Common.Messaging
{
    public class Message
    {
        public string Id { set; get; }
        public string Text { set; get; }
        public User Author { set; get; }
        public User Recipient { set; get; }
        public DateTime Date { set; get; }
        public bool Read { set; get; }
    }
}
