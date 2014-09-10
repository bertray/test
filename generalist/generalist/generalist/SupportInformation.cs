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

namespace Toyota.Common.Generalist
{
    public class SupportInformation
    {
        public SupportInformation(string name) : this(name, null, null, null) { }
        public SupportInformation(string name, PhoneNumber phone, string email, string chatId)
        {
            Name = name;
            Phone = phone;
            EmailAddress = email;
            ChatId = chatId;
        }

        public string Name { set; get; }
        public PhoneNumber Phone { set; get; }
        public string EmailAddress { set; get; }
        public string ChatId { set; get; }
    }
}
