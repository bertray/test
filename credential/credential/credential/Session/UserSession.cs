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
using Toyota.Common.Lookup;
using Toyota.Common.Utilities;

namespace Toyota.Common.Credential
{    
    public class UserSession
    {
        public UserSession()
        {
            Id = Guid.NewGuid().ToString();
            Data = new SimpleLookup();
        }

        public string Id { set; get; }
        public string Username { set; get; }
        public DateTime LoginTime { set; get; }
        public DateTime? LogoutTime { set; get; }
        public int? Timeout { set; get; }
        public string Location { set; get; }
        public string ClientAgent { set; get; }
        public int? LockTimeout { set; get; }
        public bool Locked { set; get; }
        public DateTime? LockTime { set; get; }
        public DateTime? UnlockTime { set; get; }
        public ILookup Data { set; get; }
    }
}
