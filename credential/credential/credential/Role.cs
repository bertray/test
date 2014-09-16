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
            SessionTimeout = 30;
            Functions = new List<AuthorizationFunction>();
        }

        public string Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public int? SessionTimeout { set; get; }
        public UserSystem System { set; get; }
        public IList<AuthorizationFunction> Functions { set; get; }

        public string _SystemId
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string[] fractions = value.Split(':');
                    if ((fractions != null) && (fractions.Length >= 4))
                    {
                        System = new UserSystem()
                        {
                            Id = fractions[0],
                            Name = fractions[1],
                            Url = fractions[2],
                            Description = fractions[3]
                        };

                        return;
                    }
                }
                System = null;
            }

            get
            {
                if (System != null)
                {
                    return System.Id;
                }
                return null;
            }
        }
    }
}
