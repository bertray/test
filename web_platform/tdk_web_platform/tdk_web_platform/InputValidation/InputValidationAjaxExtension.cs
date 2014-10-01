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
using System.Web.Mvc;
using System.Web;
using Toyota.Common.Utilities;
using Toyota.Common.Lookup;

namespace Toyota.Common.Web.Platform
{
    public class InputValidationAjaxExtension: IAjaxExtension
    {
        public string GetName()
        {
            return "Input-Validation";
        }

        public ActionResult Execute(HttpRequestBase request, HttpResponseBase response, HttpSessionStateBase session)
        {            
            string name = request.Params[GlobalConstants.Instance.RequestParameter.Select];
            if (!string.IsNullOrEmpty(name))
            {
                IInputValidator validator = InputValidatorRegistry.Instance.Get(name);
                if (validator != null)
                {
                    string value = request.Params[GlobalConstants.Instance.RequestParameter.Value];
                    IDictionary<string, string> paramMap = new Dictionary<string, string>();
                    string[] parameters = value.Split(';');
                    if ((parameters != null) && (parameters.Length > 0))
                    {
                        string[] pair;
                        foreach (string p in parameters)
                        {
                            pair = p.Split(':');
                            if ((pair != null) && (pair.Length == 2))
                            {
                                paramMap.Add(pair[0], pair[1]);
                            }
                        }
                    }

                    IList<InputValidationResult> result = validator.Validate(paramMap);
                    return new ContentResult() { Content = JSON.ToString<IList<InputValidationResult>>(result) };
                }
            }

            return new ContentResult() { Content = string.Empty };
        }
    }
}
