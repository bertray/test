﻿///
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
    public class BaseAction: IAction
    {
        public BaseAction(string name)
        {
            SetName(name);
        }

        private string name;
        public void SetName(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }
    }
}
