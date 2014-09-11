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

namespace Toyota.Common.Web.Service
{
    public class BaseServiceParameter
    {
        public BaseServiceParameter() : this(null, data => { }) { }
        public BaseServiceParameter(string command) : this(command, param => { }) { }
        public BaseServiceParameter(string command, Action<JsonDataMap> paramAction)
        {
            Parameters = new JsonDataMap();
            if (paramAction != null)
            {
                paramAction.Invoke(Parameters);
            }

            if (!string.IsNullOrEmpty(command))
            {
                Command = command;
            }
        }
        
        public virtual string Command { set; get; }
        public JsonDataMap Parameters { private set; get; }
    }
}
