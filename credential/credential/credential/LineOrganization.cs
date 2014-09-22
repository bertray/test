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
    public class LineOrganization: OrganizationStructure
    {
        public LineOrganization()
        {
            Shifts = new List<ShiftOrganization>();
        }

        public virtual IList<ShiftOrganization> Shifts { private set; get; }
        public virtual bool HasShift(ShiftOrganizationType type)
        {
            return Shifts.IsElementExists(shift => {
                return shift.Type == type;
            });
        }
        public virtual ShiftOrganization GetShift(ShiftOrganizationType type)
        {
            return Shifts.FindElement(shift => {
                return shift.Type == type;
            });
        }
    }
}
