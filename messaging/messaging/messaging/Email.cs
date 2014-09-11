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
    public class Email: Message
    {
        public Email()
        {
            CarbonCopies = new List<string>();
        }

        public IList<string> CarbonCopies { set; get; }
        public string Subject { set; get; }
        public string RecipientEmail { set; get; }
        public string AuthorEmail { set; get; }
    }
}
