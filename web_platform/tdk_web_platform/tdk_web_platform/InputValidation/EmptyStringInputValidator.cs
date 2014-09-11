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

namespace Toyota.Common.Web.Platform
{
    public class EmptyStringInputValidator: IInputValidator
    {
        public const string NAME = "EmptyString";

        public string GetName()
        {
            return NAME;
        }

        public IList<InputValidationResult> Validate(IDictionary<string, string> paramMap)
        {            
            if (paramMap.Count > 0)
            {
                List<InputValidationResult> results = new List<InputValidationResult>();
                string value;
                foreach (string key in paramMap.Keys)
                {
                    value = paramMap[key];
                    if (string.IsNullOrEmpty(value))
                    {
                        results.Add(new InputValidationResult() { 
                            Invalid = true,
                            Key = key,
                            Value = value,
                            Message = string.Format("{0} can not be empty", key.Replace('-', ' '))
                        });
                    }
                }
                return results;
            }

            return null;
        }
    }
}
