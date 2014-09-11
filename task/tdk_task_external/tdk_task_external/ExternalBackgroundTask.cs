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
using Toyota.Common.Task;

namespace Toyota.Common.Task.External
{
    public class ExternalBackgroundTask: PeriodicBackgroundTask
    {
        private string regId;
        public string RegistryId 
        {
            set
            {
                regId = value;
                Parameters.Add("tREG-ID", regId);
            }
            get
            {
                return regId;
            }
        }
        public string Command { set; get; }
    }
}
