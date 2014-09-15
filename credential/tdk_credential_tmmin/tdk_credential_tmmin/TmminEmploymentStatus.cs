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
    public class TmminEmploymentStatus: EmploymentStatus
    {
        protected TmminEmploymentStatus(string value) : base(value) { }

        public static readonly EmploymentStatus Contract_1_1 = new TmminEmploymentStatus("contract_1.1");
        public static readonly EmploymentStatus Contract_1_2 = new TmminEmploymentStatus("contract_1.2");
        public static readonly EmploymentStatus Contract_2 = new TmminEmploymentStatus("contract_2");
    }
}
