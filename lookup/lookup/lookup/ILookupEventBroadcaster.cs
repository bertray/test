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

namespace Toyota.Common.Lookup
{    
    public interface ILookupEventBroadcaster
    {
        void AddEventListener(ILookupEventListener listener);        
        void RemoveEventListener(ILookupEventListener listener);
        void AddEventListener(Action<LookupEvent> action);
        void RemoveEventListener(Action<LookupEvent> action);
        IList<ILookupEventListener> GetListeners();
        IList<Action<LookupEvent>> GetActionListener();
    }
}
