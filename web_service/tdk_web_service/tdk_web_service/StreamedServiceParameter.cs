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
using System.IO;

namespace Toyota.Common.Web.Service
{
    public class StreamedServiceParameter: BaseServiceParameter
    {
        public StreamedServiceParameter() : this(null, data => { }) { }
        public StreamedServiceParameter(string command) : this(command, param => { }) { }
        public StreamedServiceParameter(string command, Action<JsonDataMap> paramAction): base(command, paramAction) { }

        public Stream Data { set; get; }

        public StreamedServiceRuntimeParameter ToRuntime()
        {
            StreamedServiceRuntimeParameter param = new StreamedServiceRuntimeParameter();
            param.Data = Data;
            param.Command = Command;
            param.ParameterString = Parameters.ToString();
            return param;
        }

        public void FromRuntime(StreamedServiceRuntimeParameter param)
        {
            Command = null;
            Parameters.Clear();
            if (!param.IsNull())
            {
                Data = param.Data;
                Command = param.Command;
                Parameters.Clear();
                Parameters.FromString(param.ParameterString);
            }
        }

        public static StreamedServiceParameter Create(StreamedServiceRuntimeParameter runtimeParam)
        {
            StreamedServiceParameter param = new StreamedServiceParameter();
            if (!runtimeParam.IsNull())
            {
                param.Data = runtimeParam.Data;
                param.Command = runtimeParam.Command;
                param.Parameters.Clear();
                param.Parameters.FromString(runtimeParam.ParameterString);
            }
            return param;
        }
    }
}
