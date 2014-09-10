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

namespace Toyota.Common.Utilities
{
    public class CleanAction: BaseAction, ICleanAction
    {
        protected Action<object> _Action { private set; get; }

        public CleanAction(string name, Action<object> action): base(name)
        {
            _Action = action;
        }

        public void Clean(object param)
        {
            if (_Action != null)
            {
                _Action.Invoke(param);
            }
        }

        public string Name { private set; get; }
    }
}
