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
    public class ActionServiceCommand: ServiceCommand
    {
        private Func<ServiceParameter, ServiceResult> _Action { set; get; }

        public ActionServiceCommand(string name, Func<ServiceParameter, ServiceResult> _Action)
            : base(name)
        {
            this._Action = _Action;
        }

        public override ServiceResult Execute(ServiceParameter parameter)
        {
            return _Action.Invoke(parameter);
        }
    }
}
