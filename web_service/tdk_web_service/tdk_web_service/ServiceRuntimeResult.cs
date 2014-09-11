﻿///
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
using System.Runtime.Serialization;

namespace Toyota.Common.Web.Service
{
    [DataContract(Namespace = "Toyota.Common.Web.Service")]
    public class ServiceRuntimeResult
    {
        [DataMember]
        public string DataString { set; get; }
    }
}
