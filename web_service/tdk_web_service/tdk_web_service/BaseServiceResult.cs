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
    public class BaseServiceResult
    {
        private const string DATA_KEY_STATUS = "_tdk_srv_res_stat";
        private const string DATA_KEY_MESSAGE = "_tdk_srv_res_message";

        public BaseServiceResult() : this(data => { }) { }
        public BaseServiceResult(Action<JsonDataMap> dataAction) 
        {
            Data = new JsonDataMap();
            if (dataAction != null)
            {
                dataAction.Invoke(Data);
            }
            Status = ServiceStatus.OK;
            Message = null;
        }

        public ServiceStatus Status
        {
            set
            {
                Data.Add<int>("_tdk_srv_res_stat", (int) value);
            }

            get
            {
                return (ServiceStatus) Data.Get<int>("_tdk_srv_res_stat");
            }
        }        
        public JsonDataMap Data { private set; get; }

        public string Message
        {
            set
            {
                Data.Add<string>(DATA_KEY_MESSAGE, value);
            }

            get
            {
                return Data.Get<string>(DATA_KEY_MESSAGE);
            }
        }
    }
}
