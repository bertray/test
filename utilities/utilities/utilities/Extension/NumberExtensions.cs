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
    public static class NumberExtensions
    {
        public static int IncrementIfAllowed(this int number, bool allowed)
        {
            if (allowed)
            {
                number++;
            }
            return number;
        }
    }
}
