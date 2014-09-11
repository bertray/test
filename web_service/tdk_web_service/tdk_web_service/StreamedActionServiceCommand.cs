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

namespace Toyota.Common.Web.Service
{
    public class StreamedActionServiceCommand: StreamedServiceCommand
    {
        private Func<StreamedServiceParameter, StreamedServiceResult> _Action { set; get; }

        public StreamedActionServiceCommand(string name, Func<StreamedServiceParameter, StreamedServiceResult> _Action)
            : base(name)
        {
            this._Action = _Action;
        }

        public override StreamedServiceResult Execute(StreamedServiceParameter parameter)
        {
            return _Action.Invoke(parameter);
        }
    }
}
