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

namespace Toyota.Common.Credential
{
    public class Role
    {
        public Role()
        {
            SessionTimeout = 2;
            Functions = new List<AuthorizationFunction>();
        }

        public string Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public int? SessionTimeout { set; get; }
        public IList<AuthorizationFunction> Functions { set; get; }
    }
}
