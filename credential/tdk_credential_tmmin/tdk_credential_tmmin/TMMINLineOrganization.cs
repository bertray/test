using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toyota.Common.Utilities;

namespace Toyota.Common.Credential.TMMIN
{
    public class TMMINLineOrganization: LineOrganization
    {
        public TMMINLineOrganization()
        {
            _Shifts = new Dictionary<ShiftOrganizationType, ShiftOrganization>();
        }

        private IDictionary<ShiftOrganizationType, ShiftOrganization> _Shifts { set; get; }

        public override IList<ShiftOrganization> Shifts
        {
            get
            {
                return _Shifts.Values.ToList().AsReadOnly();
            }
        }
        public override bool HasShift(ShiftOrganizationType type)
        {
            return _Shifts.ContainsKey(type);
        }
        public override ShiftOrganization GetShift(ShiftOrganizationType type)
        {
            if (_Shifts.ContainsKey(type))
            {
                return _Shifts[type];
            }
            return null;
        }
        public void AddShift(ShiftOrganization shift)
        {
            if (!shift.IsNull())
            {
                ShiftOrganizationType type = shift.Type;
                if (_Shifts.ContainsKey(type))
                {
                    _Shifts[type] = shift;
                }
                else
                {
                    _Shifts.Add(type, shift);
                }
            }
        }
    }
}
