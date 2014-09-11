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
    public class TmminRole: Role
    {
        public UserSystem System { set; get; }
        public TmminArea Area { set; get; }
        public string DivisionCode { set; get; }

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
        public string _AreaId
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string[] fractions = value.Split(':');
                    if ((fractions != null) && (fractions.Length >= 2))
                    {
                        Area = new TmminArea()
                        {
                            Id = fractions[0],
                            Name = fractions[1]
                        };

                        return;
                    }
                }
                Area = null;
            }

            get
            {
                if (Area != null)
                {
                    return Area.Id;
                }
                return null;
            }
        }
    }
}
