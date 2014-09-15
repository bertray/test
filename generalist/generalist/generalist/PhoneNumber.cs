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
    public class PhoneNumber
    {
        public PhoneNumber() : this(PhoneNumberCategory.Work, null) { }
        public PhoneNumber(string number) : this(PhoneNumberCategory.Work, number) { }
        public PhoneNumber(PhoneNumberCategory category, string number)
        {
            Category = category;
            Number = number;
        }

        public PhoneNumberCategory Category { set; get; }
        public string Number { set; get; }
    }
}
