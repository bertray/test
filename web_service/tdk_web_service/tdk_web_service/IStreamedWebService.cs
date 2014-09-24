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
using System.ServiceModel;
using System.IO;
using System.ServiceModel.Activation;

namespace Toyota.Common.Web.Service
{    
    public interface IStreamedWebService: IDisposable
    {        
        StreamedServiceRuntimeResult Execute(StreamedServiceRuntimeParameter parameter);
    }
}
