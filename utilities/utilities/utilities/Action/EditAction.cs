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
    public class EditAction: BaseAction, IEditAction
    {
        protected Action<object> _Action { private set; get; }

        public EditAction(string name, Action<object> action): base(name)
        {
            _Action = action;
        }

        public void Edit(object param)
        {
            if (_Action != null)
            {
                _Action.Invoke(param);
            }
        }
    }
}
