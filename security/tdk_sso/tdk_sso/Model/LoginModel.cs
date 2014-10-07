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
using System.Web;

namespace Toyota.Common.SSO
{
    internal class LoginModel
    {
        public string Username { set; get; }
        public string Id { set; get; }
        public DateTime LoginTime { set; get; }
        public bool Locked { set; get; }
        public DateTime LockTime { set; get; }
        public DateTime UnlockTime { set; get; }
        public int SessionTimeout { set; get; }
        public int LockTimeout { set; get; }
        public int MaximumLogin { set; get; }
        public DateTime LastActive { set; get; }
    }
}