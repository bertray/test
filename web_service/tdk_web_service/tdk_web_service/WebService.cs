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
using System.ServiceModel.Activation;
using System.ServiceModel;
using Toyota.Common.Utilities;

namespace Toyota.Common.Web.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WebService: IWebService
    {
        public WebService()
        {
            Commands = new ServiceCommandPool();
        }

        protected ServiceCommandPool Commands { set; get; }
                
        public virtual ServiceRuntimeResult Execute(ServiceRuntimeParameter parameter)
        {
            if(!Commands.IsNull() && !parameter.IsNull() && !parameter.Command.IsNull()) 
            {
                try
                {
                    IServiceCommand command = Commands.GetCommand(parameter.Command);
                    if (command != null)
                    {
                        ServiceResult result = ((ServiceCommand)command).Execute(ServiceParameter.Create(parameter));
                        if (result != null)
                        {
                            return result.ToRuntime();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string faultMessage;
                    if (ex.InnerException != null)
                    {
                        faultMessage = string.Format("{0}. {1}", ex.Message, ex.InnerException.Message);
                    }
                    else
                    {
                        faultMessage = ex.Message;
                    }
                    throw new FaultException(faultMessage);
                }                
            }            
            
            return null;
        }

        public virtual void Dispose() { }
    }
}
