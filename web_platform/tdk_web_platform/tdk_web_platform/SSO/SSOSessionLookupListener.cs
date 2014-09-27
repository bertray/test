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
using Toyota.Common.Lookup;

namespace Toyota.Common.Web.Platform
{
    internal class SSOSessionLookupListener: ILookupEventListener
    {
        private string id;

        public SSOSessionLookupListener(string id)
        {
            this.id = id;
        }

        public void LookupChanged(LookupEvent evt)
        {
            if ((evt.Type == LookupEventType.Instance_Added) || (evt.Type == LookupEventType.Instance_Removed))
            {
                SSOSessionStorage.Instance.Save(id, evt.Broadcaster);
            }
        }

        public static void RemoveExistingInstance(ILookup lookup)
        {
            if (lookup != null)
            {
                ILookupEventListener lst = null;
                IList<ILookupEventListener> listeners = lookup.GetListeners();
                if (listeners != null)
                {
                    foreach (ILookupEventListener l in listeners)
                    {
                        if (l is SSOSessionLookupListener)
                        {
                            lst = l;
                            break;
                        }
                    }
                    if (lst != null)
                    {
                        lookup.RemoveEventListener(lst);
                    }
                }
            }
        }
    }
}
