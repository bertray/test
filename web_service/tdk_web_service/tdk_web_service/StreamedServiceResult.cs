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
    public class StreamedServiceResult: BaseServiceResult
    {
        public StreamedServiceResult() : this(data => { }) { }
        public StreamedServiceResult(Action<JsonDataMap> dataAction) : base(dataAction) { }

        public Stream DataStream { set; get; }

        public StreamedServiceRuntimeResult ToRuntime()
        {
            StreamedServiceRuntimeResult result = new StreamedServiceRuntimeResult();
            if (!result.IsNull())
            {
                result.Data = DataStream;
                if (!Data.IsNull())
                {
                    result.DataString = Data.ToString();
                }
            }
            return result;
        }

        public void FromRuntime(StreamedServiceRuntimeResult result)
        {
            Status = ServiceStatus.OK;
            Data.Clear();
            if (!result.IsNull())
            {                
                Data.Clear();
                Data.FromString(result.DataString);
                DataStream = result.Data;
            }
        }

        public static StreamedServiceResult Create(StreamedServiceRuntimeResult runtimeResult)
        {
            if (!runtimeResult.IsNull())
            {
                StreamedServiceResult result = new StreamedServiceResult();
                result.Data.Clear();
                result.Data.FromString(runtimeResult.DataString);
                result.DataStream = runtimeResult.Data;

                return result;
            }
            return null;
        }
    }
}
