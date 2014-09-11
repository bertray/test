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
using Toyota.Common.Lookup;

namespace Toyota.Common.Web.Platform
{
    public class InputValidatorRegistry
    {        
        private ILookup lookup;

        private InputValidatorRegistry() 
        {
            lookup = new SimpleLookup("Input-Validator");
            ObjectPool.Instance.Lookup.AddLookup(lookup);
        }

        private static InputValidatorRegistry instance = new InputValidatorRegistry();
        public static InputValidatorRegistry Instance
        {
            get
            {
                return instance;
            }
        }

        public void Add(IInputValidator validator)
        {
            if (validator != null)
            {
                lookup.Add(validator);
            }
        }

        public void Remove(IInputValidator validator)
        {
            Remove(validator.GetName());
        }

        public void Remove(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IList<IInputValidator> validators = lookup.GetAll<IInputValidator>();
                if (validators != null)
                {
                    IInputValidator validator = null;
                    foreach (IInputValidator v in validators)
                    {
                        if (v.GetName().Equals(name))
                        {
                            validator = v;
                            break;
                        }
                    }

                    if (validator != null)
                    {
                        lookup.Remove(validator);
                    }
                }
            }
        }

        public IList<IInputValidator> GetAll()
        {
            return lookup.GetAll<IInputValidator>();
        }

        public IInputValidator Get(String name)
        {
            IList<IInputValidator> validators = lookup.GetAll<IInputValidator>();
            if (validators != null)
            {
                foreach (IInputValidator v in validators)
                {
                    if (v.GetName().Equals(name))
                    {
                        return v;
                    }
                }
            }

            return null;
        }
    }
}
