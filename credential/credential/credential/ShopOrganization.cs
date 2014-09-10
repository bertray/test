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
using Toyota.Common.Utilities;

namespace Toyota.Common.Credential
{
    public class ShopOrganization: OrganizationStructure
    {
        public ShopOrganization()
        {
            Lines = new List<LineOrganization>();
        }

        public virtual IList<LineOrganization> Lines { private set; get; }
        public virtual bool HasLine(string id)
        {
            return Lines.IsElementExists(line => {
                return line.Id.Equals(id);
            });
        }
        public virtual LineOrganization GetLine(string id)
        {
            return Lines.FindElement(line => {
                return line.Id.Equals(id) ? line : null;
            });
        }
    }
}
