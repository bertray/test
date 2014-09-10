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
    public static class StringExtensions
    {
        public static string ToEmptyStringIfNull(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str;
            }
            return string.Empty;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
