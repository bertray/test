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

namespace Toyota.Common.DataExchange
{
    public class DataBusActionListener: IDataBusEventListener
    {
        private Action<DataBusEvent> action;

        public DataBusActionListener(Action<DataBusEvent> action)
        {
            this.action = action;
        }

        public void DataBusEventBroadcasted(DataBusEvent evt)
        {
            if (action != null)
            {
                action.Invoke(evt);
            }
        }
    }
}
