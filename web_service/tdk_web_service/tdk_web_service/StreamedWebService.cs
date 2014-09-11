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
using System.IO;
using System.Web;
using System.ServiceModel.Activation;
using System.ServiceModel;
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class StreamedWebService: IStreamedWebService
    {
        public StreamedWebService()
        {
            Commands = new ServiceCommandPool();
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                context.Response.BufferOutput = false;
            }
        }

        protected ServiceCommandPool Commands { set; get; }

        public StreamedServiceRuntimeResult Execute(StreamedServiceRuntimeParameter parameter)
        {
            try
            {
                StreamedServiceRuntimeResult result = new StreamedServiceRuntimeResult();
                if (!parameter.IsNull())
                {
                    result = _ExecuteCommand(parameter);

                }
                return result;
            }
            catch (Exception ex)
            {
                string faultMessage;
                if(ex.InnerException != null) 
                {
                    faultMessage = string.Format("{0}. {1}", ex.Message, ex.InnerException.Message);
                } else {
                    faultMessage = ex.Message;
                }
                throw new FaultException(faultMessage);
            }
        }

        public void Dispose() {
            Commands.Clear();
        }

        private StreamedServiceRuntimeResult _ExecuteCommand(StreamedServiceRuntimeParameter parameter)
        {
            if (!parameter.IsNull())
            {
                string commandName = parameter.Command;
                if (!string.IsNullOrEmpty(commandName))
                {
                    IServiceCommand command = Commands.GetCommand(parameter.Command);
                    if (!command.IsNull() && (command is StreamedServiceCommand))
                    {
                        StreamedServiceResult result = ((StreamedServiceCommand)command).Execute(StreamedServiceParameter.Create(parameter));
                        if (result != null)
                        {
                            return result.ToRuntime();
                        }
                    }
                }
            }            

            return null;
        }        
    }
}
