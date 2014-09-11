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
    public abstract class PatrolTask: IPatrolTask
    {
        private int interval;
        private string name;

        public void SetInterval(int interval)
        {
            this.interval = interval;
        }
        public int GetInterval()
        {
            return interval;
        }

        public void SetName(string name)
        {
            this.name = name;
        }
        public string GetName()
        {
            return name;
        }

        public abstract void Execute(PatrolParameter param);
    }
}
