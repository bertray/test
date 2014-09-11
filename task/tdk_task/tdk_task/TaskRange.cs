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

namespace Toyota.Common.Task
{
    public class TaskRange
    {
        public TaskRange()
        {
            Time = TimeSpan.MinValue;
        }

        public byte? Interval { set; get; }
        public TimeSpan Time { set; get; }

        public bool HasExecutionInterval()
        {
            return (Interval != null) && (Interval.Value > 0);
        }
    }
}
