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

namespace Toyota.Common.Credential.TMMIN
{
    internal class AuthorizationModel
    {
        public string RoleId { set; get; }
        public string Username { set; get; }
        public string DivisionCode { set; get; }
        public UserSystem System { set; get; }        

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
