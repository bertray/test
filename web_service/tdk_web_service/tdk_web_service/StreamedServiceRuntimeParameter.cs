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
using System.Runtime.Serialization;

namespace Toyota.Common.Web.Service
{
    public class StreamedServiceRuntimeParameter: ServiceRuntimeParameter, IDisposable
    {
        [DataMember]
        public Stream Data { set; get; }

        public void Dispose()
        {
            if (!Data.IsNull())
            {
                Data.Close();
            }
        }
    }
}
