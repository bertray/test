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
using System.ServiceModel;

namespace Toyota.Common.Web.Service
{    
    public class ServiceParameter: BaseServiceParameter
    {
        public ServiceParameter() : this(null, data => { }) { }
        public ServiceParameter(string command) : this(command, param => { }) { }
        public ServiceParameter(string command, Action<JsonDataMap> paramAction) : base(command, paramAction) { }

        public ServiceRuntimeParameter ToRuntime()
        {
            ServiceRuntimeParameter param = new ServiceRuntimeParameter();
            param.Command = Command;
            param.ParameterString = Parameters.ToString();
            return param;
        }

        public void FromRuntime(ServiceRuntimeParameter param)
        {
            Command = null;
            Parameters.Clear();
            if (!param.IsNull())
            {
                Command = param.Command;
                Parameters.Clear();
                Parameters.FromString(param.ParameterString);
            }
        }

        public static ServiceParameter Create(ServiceRuntimeParameter runtimeParam)
        {            
            ServiceParameter param = new ServiceParameter();
            if (!runtimeParam.IsNull())
            {
                param.Command = runtimeParam.Command; 
                param.Parameters.Clear();
                param.Parameters.FromString(runtimeParam.ParameterString);
            }
            return param;
        }
    }
}
