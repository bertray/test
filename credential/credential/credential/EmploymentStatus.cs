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
using System.Security.Cryptography;

namespace Toyota.Common.Credential
{
    public class EmploymentStatus
    {
        private int _hash;

        protected EmploymentStatus(string value) 
        {
            Value = value;
            _hash = 0;
            if (!string.IsNullOrEmpty(value))
            {
                byte[] hashBytes = Encoding.UTF8.GetBytes(Value);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(hashBytes);
                }
                if (hashBytes.Length < 4)
                {
                    byte[] temp = new byte[4];
                    hashBytes.CopyTo(temp, 0);
                    hashBytes = temp;
                }
                _hash = BitConverter.ToInt32(hashBytes, 0);
            }
        }

        public string Value { private set; get; }

        public override bool Equals(object obj)
        {
            if (!obj.IsNull() && (obj is EmploymentStatus))
            {
                return ((EmploymentStatus)obj).Value.Equals(Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _hash;
        }

        public static readonly EmploymentStatus Permanent = new EmploymentStatus("permanent");
        public static readonly EmploymentStatus Contract = new EmploymentStatus("contract");
        public static readonly EmploymentStatus External = new EmploymentStatus("external");
        public static readonly EmploymentStatus ICT = new EmploymentStatus("ict");
    }
}
