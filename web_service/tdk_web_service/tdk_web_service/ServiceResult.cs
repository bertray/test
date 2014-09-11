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
    public class ServiceResult: BaseServiceResult
    {
        public ServiceResult() : base() { }
        public ServiceResult(Action<JsonDataMap> dataAction) : base(dataAction) { }

        public ServiceRuntimeResult ToRuntime()
        {
            ServiceRuntimeResult result = new ServiceRuntimeResult();
            if (!result.IsNull())
            {
                result.DataString = Data.ToString();
            }
            return result;
        }

        public void FromRuntime(ServiceRuntimeResult result)
        {
            Status = ServiceStatus.OK;
            Data.Clear();
            if (!result.IsNull())
            {
                Data.Clear();
                Data.FromString(result.DataString);
            }
        }

        public static ServiceResult Create(ServiceRuntimeResult runtimeResult)
        {
            if (!runtimeResult.IsNull())
            {
                ServiceResult result = new ServiceResult();
                result.Data.Clear();
                result.Data.FromString(runtimeResult.DataString);

                return result;
            }
            return null;
        }
    }
}
